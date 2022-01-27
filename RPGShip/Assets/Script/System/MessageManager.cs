﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MessageManager
{
    //メッセージウィンドウ
    public MessageWindow messageWindow;
    //メッセージリスト
    [SerializeField] private List<string> messageList = null;
    //メッセージスピード
    public float messageSpeed = 0.03f;
    //会話状態
    private bool _talkCompFlg = false;
    private int _messageListIndex = 0;

    public bool talkingFlg { get; }
    public bool talkCompFlg { get; }
    public int messageListIndex { get; }

    public MessageManager(MessageWindow window)
    {
        messageWindow = window;
    }


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

    //リスト読み込み
    public void loadTestAsset(string assetName)
    {
        TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load(assetName, typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる
        //Splitで一行づつを代入した1次配列を作成
        messageList = new List<string>(TextLines.Split('\n')); //        
    }

    //リストセット
    public void setMessageList(string[] msg)
    {
        messageList = new List<string>(msg);
    }

    public void setMessageList(List<string> msg)
    {
        foreach (var mem in msg) messageList.Add(mem);
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
        iTween.ValueTo(ContentManager.instance.gameObject, hash);
    }

    //現在の文字列の指定カウント数までを表示
    public void setMessageNum(int num)
    {
        messageWindow.text.text = messageList[_messageListIndex].Substring(0, num);
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

    //メッセージ読み込み完了・一時停止
    public void compMessage()
    {
        _talkCompFlg = true;
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
