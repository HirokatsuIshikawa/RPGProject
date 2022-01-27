using UnityEngine;
using UnityEngine.UI;

public class ContentPartsHolder : MonoBehaviour
{
    //カメラコントローラー
    public CameraControll cameraController;
    //メッセージウィンドウ
    public MessageWindow messageWindow;
    //暗幕
    public Image darkField;
    
    // Start is called before the first frame update
    void Start()
    {
        //ContentManager.instance.PlayerObj = PlayerObj;
        ContentManager.instance.cameraController = cameraController;
        ContentManager.instance.messageWindow = messageWindow;
        ContentManager.instance.darkField = darkField;
        ContentManager.instance.touchState.enabled = true;
        ContentManager.instance.screenManager = new ScreenManager(darkField);
        ContentManager.instance.messageManager = new MessageManager(messageWindow);
        Destroy(this.gameObject);
    }
}
