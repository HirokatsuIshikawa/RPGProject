using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContentManager : MonoBehaviour
{
    static public ContentManager instance;

    public enum SystemProcess
    {
        None,
        Menu,
        Talk,
        ChangeMap
    }

    //////////////////////////////////////////マネージャー//////////////////////////////////////////
    //メッセージ
    public MessageManager messageManager;
    //画面シート処理
    public ScreenManager screenManager;
    //タッチステート
    public TouchState touchState;
    //ナビメッシュ焼き
    //public NavMeshSurface2d naviSurface;
    //////////////////////////////////////////オブジェクト//////////////////////////////////////////
    //マップ
    public Map map;
    //プレイヤーマップ位置
    public PlayerPoint playerMapPoint;
    //スタート位置
    public MapPoint nowMapPoint;
    //移動先
    public MapPoint nextMapPoint;
    //
    public List<MapPoint> mapPointList;
    //
    public LineRenderer mapLine;
    //マップメニュー
    public MapMenu mapMenu;
    //はいいいえポップアップ
    public AskPopup askPopup;

    //カメラコントローラー
    public CameraControll cameraController;
    //メッセージウィンドウ
    public MessageWindow messageWindow;
    //暗幕
    public Image darkField;
    public Image backDarkField;
    //イベント中フラグ
    public SystemProcess process = SystemProcess.None;
    //アクション冷却時間
    public float actionCoolTime = 0.0f;

    //マップ変更設定
    private string changeMapName;
    private Vector2 changeMapPos;
    private CharaAnime.DIRECTION changeMapDirection;
    
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
        Screen.SetResolution(540, 960, false);
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraController.SetTargetPos(playerMapPoint.transform);
        //mapLine = new LineRenderer();
        mapLine.positionCount = mapPointList.Count;

        for (int i = 0; i < mapPointList.Count; i++)
        {
            mapLine.SetPosition(i, mapPointList[i].transform.position);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //イベント中
        if (isActioning())
        {
            switch (process)
            {
                case SystemProcess.Talk:
                    messageManager.talkUpdate();
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
        //プロセス・マップ変更
        process = SystemProcess.ChangeMap;
        //変更後設定
        changeMapName = mapName;
        changeMapPos = mapPos;
        changeMapDirection = direction;
        //フェードイン
        screenManager.changeColor(Color.black, 0.7f, "changeMap");
    }

    //複数アクション中状態取得
    public bool isActioning()
    {
        return process != SystemProcess.None;
    }
    

    //イベント開始
    public void eventStart(List<string> msg)
    {
        messageManager.setMessageList(msg);
        process = SystemProcess.Talk;
        messageManager.messageStart();
    }

    //イベント終了
    public void eventEnd()
    {
        process = SystemProcess.None;
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

    public void PushStartButton()
    {
        ChangeMainScene();
    }

    private void ChangeMainScene()
    {
        SceneManager.LoadScene("Main");
    }


    public void SetMapMenu(bool flg, MapPoint point = null)
    {
        backDarkField.gameObject.SetActive(flg);
        mapMenu.SetButtonEnable(point, nowMapPoint);
        mapMenu.gameObject.SetActive(flg);
    }

    //マップ移動


    public void MovePoint()
    {
        cameraController.setTarget(playerMapPoint.transform);
        float distance = Vector3.Distance(nowMapPoint.transform.position, mapMenu.selectMapPoint.transform.position);
        iTween.MoveTo(playerMapPoint.gameObject, iTween.Hash(
            "x", mapMenu.selectMapPoint.transform.position.x,
            "y", mapMenu.selectMapPoint.transform.position.y,
            "oncomplete", "MoveEnd",
            "oncompletetarget", this.gameObject,
            "time", distance,
            "easetype", iTween.EaseType.linear
            ));
        nextMapPoint = mapMenu.selectMapPoint;
        SetMapMenu(false);
        askPopup.ClearEvent();
        askPopup.gameObject.SetActive(false);
        nowMapPoint = null;

    }

    public void MoveEnd()
    {
        cameraController.clearTarget();
        nowMapPoint = nextMapPoint;
        nextMapPoint = null;
    }




}