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
    //スプライトレンダー
    public SpriteRenderer render;

    public BoxCollider2D colider;
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
    protected List<Sprite> nowSprite;
    //スプライト番号
    protected int spriteNum;
    //スプライト表示時間
    protected float spriteTime;
    //現在のスプライト番号
    protected int nowSpriteNum;
    //キャラの向き
    public DIRECTION charaDirection = DIRECTION.down;


    //方向
    public Vector2 inputAxis;
    public Vector2 chipSize = new Vector2(0.32f, 0.48f);

    // Start is called before the first frame update
    protected void Start()
    //void Start()
    {
        //初期では下向き
        nowSprite = downSprite;
        charaDirection = DIRECTION.down;
    }

    void Update()
    {
    }
    
    // Update is calle
    protected void FixedUpdate()
    {
        //イベント中は実行しない
        if (ContentManager.instance.isActioning())
        {
            return;
        }
        moveAnime();
        Clamp();
    }
    
    public void moveAnime() {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.y * 0.0001f);
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

    public void changeDirection()
    {
        switch(charaDirection)
        {
            case DIRECTION.up:
                nowSprite = upSprite;
                break;
            case DIRECTION.down:
                nowSprite = downSprite;
                break;
            case DIRECTION.right:
                nowSprite = rightSprite;
                break;
            case DIRECTION.left:
                nowSprite = leftSprite;
                break;

        }

        render.sprite = nowSprite[0];
    }



    protected void Clamp()
    {
        /*
        // 画面左下のワールド座標をビューポートから取得
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        // 画面右上のワールド座標をビューポートから取得
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, min.x + chipSize.x, max.x - chipSize.x);
        pos.y = Mathf.Clamp(pos.y, min.y + chipSize.y, max.y - chipSize.y);
        transform.position = pos;
        */
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(gameObject.name + " : " + "Collision_" + collision.gameObject.name);
        //rigidBody.velocity = Vector2.zero;
    }
}
