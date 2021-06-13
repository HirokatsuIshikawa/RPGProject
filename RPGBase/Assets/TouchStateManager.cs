using UnityEngine;

/**
 * 状態管理
 */
/**
 * タッチ管理クラス
 */
public class TouchState
{
    public bool _touch_flag;      // タッチ有無
    public Vector2 _touch_position;   // タッチ座標
    public TouchPhase _touch_phase;   // タッチ状態

    /**
     * コンストラクタ
     *
     * @param bool flag タッチ有無
     * @param Vector2 position タッチ座標(引数の省略が行えるようにNull許容型に)
     * @param Touchphase phase タッチ状態
     * @access public
     */
    public TouchState(bool flag = false, Vector2? position = null, TouchPhase phase = TouchPhase.Began)
    {
        this._touch_flag = flag;
        if (position == null)
        {
            this._touch_position = new Vector2(0, 0);
        }
        else
        {
            this._touch_position = (Vector2)position;
        }
        this._touch_phase = phase;
    }

    /**
     * 更新
     *
     * @access public
     */
    public void update()
    {
        this._touch_flag = false;

        // エディタ
        if (Application.isEditor)
        {
            // 押した瞬間
            if (Input.GetMouseButtonDown(0))
            {
                this._touch_flag = true;
                this._touch_phase = TouchPhase.Began;
                Debug.Log("押した瞬間");
            }

            // 離した瞬間
            else if (Input.GetMouseButtonUp(0))
            {
                this._touch_flag = true;
                this._touch_phase = TouchPhase.Ended;
                Debug.Log("離した瞬間");
            }

            // 押しっぱなし
            else if (Input.GetMouseButton(0))
            {
                this._touch_flag = true;
                this._touch_phase = TouchPhase.Moved;
                Debug.Log("押しっぱなし");
            }

            // 座標取得
            if (this._touch_flag) this._touch_position = Input.mousePosition;

            // 端末
        }
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                this._touch_position = touch.position;
                this._touch_phase = touch.phase;
                this._touch_flag = true;
            }
        }
    }

    /**
     * タッチ状態取得
     *
     * @access public
     */
    public TouchState getTouch()
    {
        return new TouchState(this._touch_flag, this._touch_position, this._touch_phase);
    }
}
