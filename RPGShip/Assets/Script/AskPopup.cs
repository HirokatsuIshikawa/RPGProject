using UnityEngine;
using UnityEngine.UI;

public class AskPopup : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;
    public Text discription;
    public CallBack yesEvent;
    public CallBack noEvent;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init()
    {
        yesEvent = new CallBack();
        noEvent = new CallBack();
    }


    public void SetEvent(CallBack.OnCompleteDelegate setYesEvent, CallBack.OnCompleteDelegate setNoEvent = null)
    {
        ClearEvent();
        yesEvent.CompleteHandler = setYesEvent;
        noEvent.CompleteHandler = setNoEvent;
    }

    public void PushYesButton()
    {
        if(yesEvent.CompleteHandler != null)
        {
            yesEvent.ExeCallBack();
        }
        else
        {
            this.gameObject.SetActive(false);
        }

    }

    public void PushNoButton()
    {
        if (noEvent.CompleteHandler != null)
        {
            noEvent.ExeCallBack();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SetDiscriptionText(string str)
    {
        discription.text = str;
    }

    public void ClearEvent()
    {
        if (yesEvent.CompleteHandler != null)
        {
            yesEvent.CompleteHandler = null;
        }
        if (noEvent.CompleteHandler != null)
        {
            noEvent.CompleteHandler = null;
        }
    }

    private void OnDisable()
    {
        ClearEvent();
    }

}

// 処理クラス
public class CallBack
{
    public delegate void OnCompleteDelegate();
    public OnCompleteDelegate CompleteHandler;

    // 処理実行
    public void ExeCallBack()
    {
        // 処理実行
        Debug.Log("処理実行");

        // コールバック実行
        CompleteHandler?.Invoke();
    }
}