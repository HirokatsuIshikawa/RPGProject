using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    //メッセージウィンドウ
    public MessageWindow messageWindow;
    //メッセージリスト
    public List<string> messageList = null;
    //メッセージスピード
    public float messageSpeed = 0.03f;
    //会話状態
    private bool _talkCompFlg = false;
    private int _messageListIndex = 0;

    public bool talkingFlg { get; }
    public bool talkCompFlg { get; }
    public int messageListIndex { get; }

    // Update is called once per frame
    public void talkUpdate()
    {
        //会話終了
        if (_talkCompFlg == true)
        {
            if (TouchState.instance.getTouchEnded())
            {
                nextMessage();
            }
        }
    }
    
    //会話開始
    public void messageStart()
    {
        messageWindow.text.text = string.Empty;
        messageWindow.gameObject.SetActive(true);
        _talkCompFlg = false;

        // SetValue()を毎フレーム呼び出して、１秒間に１から０までの値の中間値を渡す        
        Hashtable hash = new Hashtable(){
            { "from", 0},
            { "to", messageList[_messageListIndex].Length},
            { "time", messageList[_messageListIndex].Length * messageSpeed},
            { "onupdate", "SetMessage"},
            { "oncomplete", "compMessage"}
        };
        iTween.ValueTo(gameObject, hash);
    }

    //iTween用メッセージセット
    private void SetMessage(int num)
    {
        messageWindow.text.text = messageList[_messageListIndex].Substring(0, num);
    }

    //メッセージ読み終わり
    private void compMessage()
    {
        _talkCompFlg = true;
    }

    //次のメッセージへ
    public void nextMessage()
    {
        _messageListIndex++;
        if (_messageListIndex < messageList.Count)
        {
            messageStart();
        }
        else
        {
            messageEnd();
        }
    }

    //メッセージ終了
    private void messageEnd()
    {
        //状態初期化
        messageWindow.text.text = string.Empty;
        messageWindow.gameObject.SetActive(false);
        _talkCompFlg = false;
        messageList = null;
        _messageListIndex = 0;
        //イベント終了
        ContentManager.instance.eventEnd();
    }

}
