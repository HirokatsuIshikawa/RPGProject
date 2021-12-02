using UnityEngine;

public class PlayerAnime : CharaAnime
{
    //ジョイスティック
    public FloatingJoystick joystick;
    public BoxCollider2D eventCollider;
    // Start is called before the first frame update
    //private new void Start()

    protected new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //イベント中は実行しない
        if (ContentManager.instance.isActioning())
        {
            return;
        }
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
    }

    private new void FixedUpdate()
    {
        base.MoveAnime();

        switch (charaDirection)
        {
            case DIRECTION.up:
                eventCollider.offset = new Vector2(0, 0.05f);
                eventCollider.size = new Vector2(0.2f, 0.05f);
                break;
            case DIRECTION.down:
                eventCollider.offset = new Vector2(0, -0.25f);
                eventCollider.size = new Vector2(0.2f, 0.05f);
                break;
            case DIRECTION.right:
                eventCollider.offset = new Vector2(0.12f, -0.1f);
                eventCollider.size = new Vector2(0.06f, 0.03f);
                break;
            case DIRECTION.left:
                eventCollider.offset = new Vector2(-0.12f, -0.1f);
                eventCollider.size = new Vector2(0.06f, 0.03f);
                break;
        }
        Clamp();
    }


    protected new void Clamp()
    {
        // 画面左下のワールド座標をビューポートから取得
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        // 画面右上のワールド座標をビューポートから取得
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, min.x + chipSize.x, max.x - chipSize.x);
        pos.y = Mathf.Clamp(pos.y, min.y + chipSize.y, max.y - chipSize.y);
        transform.position = pos;
    }
}
