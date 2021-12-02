using UnityEngine;
using UnityEngine.UI;

public class ContentPartsHolder : MonoBehaviour
{

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

    public FloatingJoystick joyStick;

    // Start is called before the first frame update
    void Start()
    {
        ContentManager.instance.PlayerObj = PlayerObj;
        ContentManager.instance.cameraController = cameraController;
        ContentManager.instance.MapLayer = MapLayer;
        ContentManager.instance.MapObj = MapObj;
        ContentManager.instance.messageWindow = messageWindow;
        ContentManager.instance.darkField = darkField;
        ContentManager.instance.touchState.joyStick = joyStick;
        ContentManager.instance.touchState.enabled = true;
        ContentManager.instance.screenManager = new ScreenManager(darkField);
        ContentManager.instance.messageManager = new MessageManager(messageWindow);
        Destroy(this.gameObject);
    }
}
