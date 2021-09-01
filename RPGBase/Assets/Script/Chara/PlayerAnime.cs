using UnityEngine;

public class PlayerAnime : CharaAnime
{
    //�J����
    public CameraControll cameraControll;
    //�W���C�X�e�B�b�N
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
        //�L�[�{�[�h������͕������擾
        inputAxis.x = Input.GetAxis("Horizontal");
        inputAxis.y = Input.GetAxis("Vertical");
        //���͂��ĂȂ��ꍇ�̓W���C�X�e�B�b�N����擾
        if (inputAxis.x == 0)
        {
            inputAxis.x = joystick.Horizontal;
        }
        if (inputAxis.y == 0)
        {
            inputAxis.y = joystick.Vertical;
        }

        /*
        //��ʃ^�b�v���̑���
        //UI�ɓ���������X�N���[�����Ȃ�
        if (Input.touchCount >= 1)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)
            {
                //��ʃ^�b�v�����Ƃ�
                if (Input.GetMouseButtonDown(0))
                {

                }
            }
        }
        */
        //��ʃ^�b�v���̑���
        if(TouchState.instance.getTouchBegan())
        {
            Debug.Log(gameObject.name + " : " + "TouchBegan");
        }

    }

}
