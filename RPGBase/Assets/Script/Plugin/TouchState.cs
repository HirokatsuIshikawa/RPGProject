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
    [SerializeField]
    private bool _touchFlag;      // タッチ有無
    [SerializeField]
    private Vector2 _touchPosition;   // タッチ座標
    [SerializeField]
    private TouchPhase _touchPhase;   // タッチ状態

    public bool touchFlag { get => _touchFlag; }
    public Vector2 touchPosition { get => _touchPosition; }
    public TouchPhase touchPhase { get => _touchPhase; }

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
            //UI上ではない場合
            //if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                // 押した瞬間
                if (Input.GetMouseButtonDown(0))
                {
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
                    Debug.Log("押しっぱなし");
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
                }
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
}
