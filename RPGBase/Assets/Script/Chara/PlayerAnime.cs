using UnityEngine;

public class PlayerAnime : CharaAnime
{
    //カメラ
    public CameraControll cameraControll;
    //ジョイスティック
    public FloatingJoystick joystick;
    // Start is called before the first frame update
    //private new void Start()

    protected new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //キーボードから入力方向を取得
        inputAxis.x = Input.GetAxis("Horizontal");
        inputAxis.y = Input.GetAxis("Vertical");
        //入力してない場合はジョイスティックから取得
        if (inputAxis.x == 0)
        {
            inputAxis.x = joystick.Horizontal;
        }
        if (inputAxis.y == 0)
        {
            inputAxis.y = joystick.Vertical;
        }

        /*
        //画面タップ時の操作
        //UIに当たったらスクロールしない
        if (Input.touchCount >= 1)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)
            {
                //画面タップしたとき
                if (Input.GetMouseButtonDown(0))
                {

                }
            }
        }
        */
        //画面タップ時の操作
        if(TouchState.instance.getTouchBegan())
        {
            Debug.Log(gameObject.name + " : " + "TouchBegan");
        }

    }

}
