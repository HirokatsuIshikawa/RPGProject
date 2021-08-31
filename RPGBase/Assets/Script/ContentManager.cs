using UnityEngine;
using UnityEngine.UI;

public class ContentManager : MonoBehaviour
{
    static public ContentManager instance;
    public GameObject PlayerObj;
    public Transform MapLayer;
    public GameObject MapObj;

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


    public void changeMap(string mapName, Vector2 mapPos)
    {
        changingMap = true;
        GameObject obj = Instantiate(Resources.Load<GameObject>(mapName), MapLayer);
        Destroy(MapObj);
        MapObj = obj;
        MapObj.SetActive(true);
        PlayerObj.transform.position = mapPos;
        changingMap = false;
    }

}
