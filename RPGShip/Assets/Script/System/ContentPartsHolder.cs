using UnityEngine;
using UnityEngine.UI;

public class ContentPartsHolder : MonoBehaviour
{

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
