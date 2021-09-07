using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentManager : MonoBehaviour
{
    static public ContentManager instance;

    //////////////////////////////////////////�}�l�[�W���[//////////////////////////////////////////
    //���b�Z�[�W
    public MessageManager messageManager;
    //��ʃV�[�g����
    public ScreenManager screenManager;
    //////////////////////////////////////////�I�u�W�F�N�g//////////////////////////////////////////
    //�v���C���[
    public GameObject PlayerObj;
    //�J�����R���g���[���[
    public CameraControll cameraController;
    //�}�b�v���C���[
    public Transform MapLayer;
    //�}�b�v
    public GameObject MapObj;
    //���b�Z�[�W�E�B���h�E
    public MessageWindow messageWindow;
    //�Ö�
    public Image darkField;
    //�C�x���g���t���O
    public bool nowEventFlg = false;
    //�C�x���g�̈�ڐG�t���O�A�ړ�����ȂǂɈړ����Ȃ��悤
    public bool eventAreaEntryFlg = false;
    //���݂̃C�x���g
    public EventObject.EventType nowEventType;
    //�A�N�V������p����
    public float actionCoolTime = 0.0f;

    //�}�b�v�ύX�ݒ�
    private string changeMapName;
    private Vector2 changeMapPos;
    private CharaAnime.DIRECTION changeMapDirection;

    //���j���[�t���O
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
        //�C�x���g��
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

    //�}�b�v�ړ��J�n
    public void changeMapStart(string mapName, Vector2 mapPos, CharaAnime.DIRECTION direction)
    {
        //�C�x���g�t���O
        nowEventFlg = true;
        nowEventType = EventObject.EventType.Move;
        //�ύX��ݒ�
        changeMapName = mapName;
        changeMapPos = mapPos;
        changeMapDirection = direction;
        //�t�F�[�h�C��
        screenManager.changeColor(Color.black, 0.7f, "changeMap");
    }

    //�}�b�v�ύX����
    public void changeMap()
    {
        //�}�b�v�����ւ�
        GameObject obj = Instantiate(Resources.Load<GameObject>(changeMapName), MapLayer);
        Destroy(MapObj);
        MapObj = obj;
        MapObj.SetActive(true);
        //�v���C���[�ʒu�E�����ݒ�
        PlayerAnime playerAnime = PlayerObj.GetComponent<PlayerAnime>();
        playerAnime.rigidBody.velocity = Vector2.zero;
        playerAnime.inputAxis = Vector2.zero;
        PlayerObj.transform.localPosition = new Vector3(changeMapPos.x, changeMapPos.y, 0);
        playerAnime.charaDirection = changeMapDirection;
        playerAnime.changeDirection();
        //�J�������͈͂̓����蔻���ݒ�
        cameraController.setCameraBound(MapObj.GetComponent<CompositeCollider2D>());
        //�t�F�[�h�A�E�g
        screenManager.changeColor(Color.clear, 0.7f, "changeMapEnd");
    }

    //�}�b�v�ύX�I��
    public void changeMapEnd()
    {
        eventEnd();
    }



    //�����A�N�V��������Ԏ擾
    public bool isActioning()
    {
        return nowEventFlg || menuFlg;
    }

    //�C�x���g����Ԏ擾
    public bool isEventing()
    {
        return nowEventFlg;
    }

    //�C�x���g�J�n
    public void eventStart(List<string> msg)
    {
        messageManager.messageList = msg;
        nowEventType = EventObject.EventType.Talk;
        messageManager.messageStart();
    }

    //�C�x���g�I��
    public void eventEnd()
    {
        nowEventFlg = false;
        nowEventType = EventObject.EventType.None;
        actionCoolTime = 0.3f;
    }

    //iTween�p���b�Z�[�W�Z�b�g
    private void SetMessage(int num)
    {
        messageManager.setMessageNum(num);
    }

    //���b�Z�[�W�ǂݏI���
    private void compMessage()
    {
        messageManager.compMessage();
    }


    //�Ö��Z�b�g
    private void SetValue(float alpha)
    {
        // iTween�ŌĂ΂ꂽ��A�󂯎�����l��Image�̃A���t�@�l�ɃZ�b�g
        darkField.color = new Color(0, 0, 0, alpha);
    }

    //�F�Ö�
    private void SetValue(Color color)
    {
        // iTween�ŌĂ΂ꂽ��A�󂯎�����l��Image�̃A���t�@�l�ɃZ�b�g
        darkField.color = color;
    }

}