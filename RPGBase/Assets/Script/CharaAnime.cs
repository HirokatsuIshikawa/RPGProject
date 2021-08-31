using System.Collections.Generic;
using UnityEngine;

public class CharaAnime : MonoBehaviour
{
    //方向
    public enum DIRECTION
    {
        up,
        down,
        right,
        left
    }

    //ジョイスティック
    public FloatingJoystick joystick;
    //スプライトレンダー
    public SpriteRenderer render;
    //物理
    public Rigidbody2D rigidBody;
    //スピード
    public float SPEED = 1.0f;
    //animation切り替え時間
    public float MaxSpriteTime = 0.3f;

    //方向スプライト
    public List<Sprite> upSprite;
    public List<Sprite> downSprite;
    public List<Sprite> rightSprite;
    public List<Sprite> leftSprite;

    //現在のスプライト
    private List<Sprite> nowSprite;
    //スプライト番号
    private int spriteNum;
    //スプライト表示時間
    private float spriteTime;
    //現在のスプライト番号
    private int nowSpriteNum;
    //方向
    public Vector2 inputAxis;
    //キャラの向き
    private DIRECTION charaDirection = DIRECTION.down;

    // Start is called before the first frame update
    void Start()
    {
        //初期では下向き
        nowSprite = downSprite;
        charaDirection = DIRECTION.down;
    }

    private void Update()
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
    }

    // Update is calle
    private void FixedUpdate()
    {
        //現在のスプライト番号を取得
        nowSpriteNum = spriteNum;
        //スプライト時間を加算
        spriteTime += Time.deltaTime;
        //最大スプライト時間を超えたら
        if (spriteTime >= MaxSpriteTime)
        {
            //スプライト番号加算
            spriteNum++;
            //スプライト番号が限界を超えたら元に戻す
            if (spriteNum >= nowSprite.Count)
            {
                spriteNum = 0;
            }
            //スプライト時間を０にする
            spriteTime = 0;
        }
        //スプライト変更フラグ
        bool changeSprite = false;
        //縦横どちらかを入力している場合
        if (inputAxis.x != 0 || inputAxis.y != 0)
        {
            //縦要素が強ければ
            if (Mathf.Abs(inputAxis.y) >= Mathf.Abs(inputAxis.x))
            {
                //プラスなら上を向く
                if (inputAxis.y >= 0 && charaDirection != DIRECTION.up)
                {
                    nowSprite = upSprite;
                    changeSprite = true;
                    charaDirection = DIRECTION.up;
                    spriteNum = 1;
                    spriteTime = 0;
                }
                //マイナスなら下を向く
                else if (inputAxis.y < 0 && charaDirection != DIRECTION.down)
                {
                    nowSprite = downSprite;
                    changeSprite = true;
                    charaDirection = DIRECTION.down;
                    spriteNum = 1;
                    spriteTime = 0;
                }
            }
            //横要素が強ければ
            else if (Mathf.Abs(inputAxis.x) >= Mathf.Abs(inputAxis.y))
            {
                //プラスなら右を向く
                if (inputAxis.x >= 0 && charaDirection != DIRECTION.right)
                {
                    nowSprite = rightSprite;
                    changeSprite = true;
                    charaDirection = DIRECTION.right;
                    spriteNum = 1;
                    spriteTime = 0;
                }
                //マイナスなら左を向く
                else if (inputAxis.x < 0 && charaDirection != DIRECTION.left)
                {
                    nowSprite = leftSprite;
                    changeSprite = true;
                    charaDirection = DIRECTION.left;
                    spriteNum = 1;
                    spriteTime = 0;
                }
            }
            rigidBody.velocity = inputAxis.normalized * SPEED;
        }
        else
        {
            //入力してなければ動かさない
            changeSprite = true;
            spriteNum = 0;
            spriteTime = 0;
            rigidBody.velocity = Vector2.zero;
        }
        //スプライト番号が違うか向きを変えていれば表示スプライトを変更
        if (nowSpriteNum != spriteNum || changeSprite)
        {
            render.sprite = nowSprite[spriteNum];
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(gameObject.name + " : " + "Collision_" + collision.gameObject.name);
    }
}
