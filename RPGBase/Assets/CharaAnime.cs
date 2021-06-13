using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAnime : MonoBehaviour
{

    public enum DIRECTION
    {
        up,
        down,
        right,
        left
    }

    public FloatingJoystick joystick;
    public SpriteRenderer render;
    public Rigidbody2D rigidBody;
    public float SPEED = 1.0f;
    public float MaxSpriteTime = 0.3f;

    public List<Sprite> upSprite;
    public List<Sprite> downSprite;
    public List<Sprite> rightSprite;
    public List<Sprite> leftSprite;

    private List<Sprite> nowSprite;
    private int spriteNum;
    private float spriteTime;
    private int nowSpriteNum;
    public Vector2 inputAxis;

    private bool changeSprite = false;
    private DIRECTION charaDirection = DIRECTION.down;

    // Start is called before the first frame update
    void Start()
    {
        nowSprite = downSprite;
        charaDirection = DIRECTION.down;
    }


    private void Update()
    {
        inputAxis.x = Input.GetAxis("Horizontal");
        inputAxis.y = Input.GetAxis("Vertical");

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
        nowSpriteNum = spriteNum;
        spriteTime += Time.deltaTime;

        if (spriteTime >= MaxSpriteTime)
        {
            spriteNum++;
            if (spriteNum >= nowSprite.Count)
            {
                spriteNum = 0;
            }
            spriteTime = 0;
        }

        changeSprite = false;


        if (inputAxis.x != 0 || inputAxis.y != 0)
        {
            if (Mathf.Abs(inputAxis.y) >= Mathf.Abs(inputAxis.x))
            {

                if (inputAxis.y >= 0 && charaDirection != DIRECTION.up)
                {
                    nowSprite = upSprite;
                    changeSprite = true;
                    charaDirection = DIRECTION.up;
                    spriteNum = 1;
                    spriteTime = 0;
                }
                else if (inputAxis.y < 0 && charaDirection != DIRECTION.down)
                {
                    nowSprite = downSprite;
                    changeSprite = true;
                    charaDirection = DIRECTION.down;
                    spriteNum = 1;
                    spriteTime = 0;
                }
            }
            else if (Mathf.Abs(inputAxis.x) >= Mathf.Abs(inputAxis.y))
            {
                if (inputAxis.x >= 0 && charaDirection != DIRECTION.right)
                {
                    nowSprite = rightSprite;
                    changeSprite = true;
                    charaDirection = DIRECTION.right;
                    spriteNum = 1;
                    spriteTime = 0;
                }
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
            changeSprite = true;
            spriteNum = 0;
            spriteTime = 0;
            rigidBody.velocity = Vector2.zero;
        }
        if (nowSpriteNum != spriteNum || changeSprite)
        {
            render.sprite = nowSprite[spriteNum];
        }
    }
}
