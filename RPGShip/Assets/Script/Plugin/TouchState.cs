using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * 状態管理
 */
/**
 * タッチ管理クラス
 */
public class TouchState : MonoBehaviour
{
    static public TouchState instance;
    public FloatingJoystick joyStick;
    [SerializeField]
    private bool _touchFlag;      // タッチ有無
    [SerializeField]
    private Vector2 _touchPosition;   // タッチ座標
    [SerializeField]
    private TouchPhase _touchPhase;   // タッチ状態

    public bool touchFlag { get => _touchFlag; }
    public Vector2 touchPosition { get => _touchPosition; }
    public TouchPhase touchPhase { get => _touchPhase; }
    public float touchTimer = 0.0f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    /**
     * 更新
     *
     * @access public
     */
    private void Update()
    {
        this._touchFlag = false;

        // エディタ
        if (Application.isEditor)
        {
            //ジョイスティック以外のUI上ではない場合
            //if (EventSystem.current.IsPointerOverGameObject() == false || joyStick.active)
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                // 押した瞬間
                if (Input.GetMouseButtonDown(0))
                {
                    touchTimer = 0.0f;
                    this._touchFlag = true;
                    this._touchPhase = TouchPhase.Began;
                    Debug.Log("押した瞬間");
                }

                // 離した瞬間
                else if (Input.GetMouseButtonUp(0))
                {
                    this._touchFlag = true;
                    this._touchPhase = TouchPhase.Ended;
                    Debug.Log("離した瞬間");
                }

                // 押しっぱなし
                else if (Input.GetMouseButton(0))
                {
                    this._touchFlag = true;
                    this._touchPhase = TouchPhase.Moved;
                    //Debug.Log("押しっぱなし");
                }

                // 座標取得
                if (this._touchFlag)
                {
                    this._touchPosition = Input.mousePosition;
                }
            }

        }
        else
        {
            // 端末
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId) == false)
                {
                    this._touchPosition = touch.position;
                    this._touchPhase = touch.phase;
                    this._touchFlag = true;
                    if (touchPhase == TouchPhase.Began)
                    {
                        touchTimer = 0.0f;
                    }
                }
            }
        }

        if (_touchFlag == true)
        {
            touchTimer += Time.deltaTime;
        }
        else
        {
            touchTimer = 0.0f;
            if (joyStick != null)
            {
                joyStick.active = false;
            }
        }
    }

    /**
     * タッチ状態取得
     *
     * @access public
     */
    public Vector2 getTouchposition()
    {
        if (_touchFlag == true)
        {
            return this._touchPosition;
        }
        else
        {
            return Vector2.zero;
        }
    }

    //タッチ開始
    public bool getTouchBegan()
    {
        return _touchFlag == true && _touchPhase == TouchPhase.Began;
    }
    //タッチ終了
    public bool getTouchEnded()
    {
        return _touchFlag == true && _touchPhase == TouchPhase.Ended;
    }
    //即座にタッチ終了
    public bool getTouchEndedSoon()
    {
        return _touchFlag == true && _touchPhase == TouchPhase.Ended && touchTimer <= 0.3f;
    }

    public static bool IsUGUIHit(Vector2 _scrPos)
    { // Input.mousePosition
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = _scrPos;
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, result);

        if (result.Count == 0)
        {
            return true;
        }
        //ジョイスティックに当たってたら
        else if (result[0].gameObject.CompareTag("JoyStick"))
        {
            return true;
        }

        return !(result.Count > 0);
    }
}
