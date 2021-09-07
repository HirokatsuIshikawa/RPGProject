using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentManager : MonoBehaviour
{
    static public ContentManager instance;

    //////////////////////////////////////////マネージャー//////////////////////////////////////////
    //メッセージ
    public MessageManager messageManager;
    //画面シート処理
    public ScreenManager screenManager;
    //////////////////////////////////////////オブジェクト//////////////////////////////////////////
    //プレイヤー
    public GameObject PlayerObj;
    //カメラコントローラー
    public CameraControll cameraController;
    //マップレイヤー
    public Transform MapLayer;
    //マップ
    public GameObject MapObj;
    //メッセージウィンドウ
    public MessageWindow messageWindow;
    //暗幕
    public Image darkField;
    //イベント中フラグ
    public bool nowEventFlg = false;
    //イベント領域接触フラグ、移動直後などに移動しないよう
    public bool eventAreaEntryFlg = false;
    //現在のイベント
    public EventObject.EventType nowEventType;
    //アクション冷却時間
    public float actionCoolTime = 0.0f;

    //マップ変更設定
    private string changeMapName;
    private Vector2 changeMapPos;
    private CharaAnime.DIRECTION changeMapDirection;

    //メニューフラグ
    public bool menuFlg = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Screen.SetResolution(540, 960, false, 60);
    }
    // Start is called before the first frame update
    void Start()
    {
        screenManager = new ScreenManager(darkField);
        messageManager = new MessageManager(messageWindow);
    }

    // Update is called once per frame
    void Update()
    {
        //イベント中
        if (isActioning())
        {
            switch (nowEventType)
            {
                case EventObject.EventType.Talk:
                    messageManager.talkUpdate();
                    break;
                case EventObject.EventType.Move:
                    break;
            }
        }

        if (actionCoolTime > 0.0f)
        {
            actionCoolTime -= Time.deltaTime;
            if (actionCoolTime < 0)
            {
                actionCoolTime = 0;
            }
        }
    }

    //マップ移動開始
    public void changeMapStart(string mapName, Vector2 mapPos, CharaAnime.DIRECTION direction)
    {
        //イベントフラグ
        nowEventFlg = true;
        nowEventType = EventObject.EventType.Move;
        //変更後設定
        changeMapName = mapName;
        changeMapPos = mapPos;
        changeMapDirection = direction;
        //フェードイン
        screenManager.changeColor(Color.black, 0.7f, "changeMap");
    }

    //マップ変更処理
    public void changeMap()
    {
        //マップ差し替え
        GameObject obj = Instantiate(Resources.Load<GameObject>(changeMapName), MapLayer);
        Destroy(MapObj);
        MapObj = obj;
        MapObj.SetActive(true);
        //プレイヤー位置・向き設定
        PlayerAnime playerAnime = PlayerObj.GetComponent<PlayerAnime>();
        playerAnime.rigidBody.velocity = Vector2.zero;
        playerAnime.inputAxis = Vector2.zero;
        PlayerObj.transform.localPosition = new Vector3(changeMapPos.x, changeMapPos.y, 0);
        playerAnime.charaDirection = changeMapDirection;
        playerAnime.changeDirection();
        //カメラ可動範囲の当たり判定を設定
        cameraController.setCameraBound(MapObj.GetComponent<CompositeCollider2D>());
        //フェードアウト
        screenManager.changeColor(Color.clear, 0.7f, "changeMapEnd");
    }

    //マップ変更終了
    public void changeMapEnd()
    {
        eventEnd();
    }



    //複数アクション中状態取得
    public bool isActioning()
    {
        return nowEventFlg || menuFlg;
    }

    //イベント中状態取得
    public bool isEventing()
    {
        return nowEventFlg;
    }

    //イベント開始
    public void eventStart(List<string> msg)
    {
        messageManager.messageList = msg;
        nowEventType = EventObject.EventType.Talk;
        messageManager.messageStart();
    }

    //イベント終了
    public void eventEnd()
    {
        nowEventFlg = false;
        nowEventType = EventObject.EventType.None;
        actionCoolTime = 0.3f;
    }

    //iTween用メッセージセット
    private void SetMessage(int num)
    {
        messageManager.setMessageNum(num);
    }

    //メッセージ読み終わり
    private void compMessage()
    {
        messageManager.compMessage();
    }


    //暗幕セット
    private void SetValue(float alpha)
    {
        // iTweenで呼ばれたら、受け取った値をImageのアルファ値にセット
        darkField.color = new Color(0, 0, 0, alpha);
    }

    //色暗幕
    private void SetValue(Color color)
    {
        // iTweenで呼ばれたら、受け取った値をImageのアルファ値にセット
        darkField.color = color;
    }

}