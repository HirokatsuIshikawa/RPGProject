using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ContentManager : MonoBehaviour
{
    static public ContentManager instance;
    public GameObject PlayerObj;
    public Transform MapLayer;
    public GameObject MapObj;
    public MessageWindow messageWindow;

    public Image darkField;
    public bool changingMap = false;
    public bool moveContactFlg = false;

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
        Screen.SetResolution(1080, 1920, true);
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private string changeMapName;
    private Vector2 changeMapPos;
    private CharaAnime.DIRECTION changeMapDirection;

    public void changeMapStart(string mapName, Vector2 mapPos, CharaAnime.DIRECTION direction)
    {
        changeMapName = mapName;
        changeMapPos = mapPos;
        changeMapDirection = direction;
        changingMap = true;
        //FadeIn(0.7f, "changeMap");
        ChangeColor(Color.black, 0.7f, "changeMap");
    }

    public void changeMap()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>(changeMapName), MapLayer);
        Destroy(MapObj);
        MapObj = obj;
        MapObj.SetActive(true);
        PlayerObj.transform.localPosition = new Vector3(changeMapPos.x, changeMapPos.y, 0);
        PlayerObj.GetComponent<PlayerAnime>().charaDirection = changeMapDirection;
        //FadeOut(0.7f, "changeMapEnd");
        ChangeColor(Color.clear, 0.7f, "changeMapEnd");
    }

    public void changeMapEnd()
    {
        changingMap = false;
    }

    void ChangeColor(Color color, float time = 1.0f, string callBackStr = null)
    {
        // SetValue()�𖈃t���[���Ăяo���āA�P�b�ԂɂO����P�܂ł̒l�̒��Ԓl��n��
        //iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 1f, "time", 1f, "onupdate", "SetValue", "oncomplete", callBackStr));
        Hashtable hash = new Hashtable(){
            {"from", darkField.color},
            {"to", color},
            {"time", time},
            {"onupdate", "SetValue"}
        };
        if (callBackStr != null)
        {
            hash.Add("oncomplete", callBackStr);
        }
        iTween.ValueTo(gameObject, hash);

    }

    void FadeIn(float time = 1.0f, string callBackStr = null)
    {
        // SetValue()�𖈃t���[���Ăяo���āA�P�b�ԂɂO����P�܂ł̒l�̒��Ԓl��n��
        //iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 1f, "time", 1f, "onupdate", "SetValue", "oncomplete", callBackStr));
        Hashtable hash = new Hashtable(){
            {"from", 0f},
            {"to", 1f},
            {"time", time},
            {"onupdate", "SetValue"}
        };
        if (callBackStr != null)
        {
            hash.Add("oncomplete", callBackStr);
        }
        iTween.ValueTo(gameObject, hash);
    }
    void FadeOut(float time = 1.0f, string callBackStr = null)
    {
        // SetValue()�𖈃t���[���Ăяo���āA�P�b�ԂɂP����O�܂ł̒l�̒��Ԓl��n��        
        Hashtable hash = new Hashtable(){
            { "from", 1f},
            { "to", 0f},
            { "time", time},
            { "onupdate", "SetValue"}
        };
        if (callBackStr != null)
        {
            hash.Add("oncomplete", callBackStr);
        }
        iTween.ValueTo(gameObject, hash);
    }
    void SetValue(float alpha)
    {
        // iTween�ŌĂ΂ꂽ��A�󂯎�����l��Image�̃A���t�@�l�ɃZ�b�g
        darkField.color = new Color(0, 0, 0, alpha);
    }
    void SetValue(Color color)
    {
        // iTween�ŌĂ΂ꂽ��A�󂯎�����l��Image�̃A���t�@�l�ɃZ�b�g
        darkField.color = color;
    }
}
