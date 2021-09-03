using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    //���b�Z�[�W�E�B���h�E
    public MessageWindow messageWindow;
    //���b�Z�[�W���X�g
    public List<string> messageList = null;
    //���b�Z�[�W�X�s�[�h
    public float messageSpeed = 0.03f;
    //��b���
    private bool _talkCompFlg = false;
    private int _messageListIndex = 0;

    public bool talkingFlg { get; }
    public bool talkCompFlg { get; }
    public int messageListIndex { get; }

    // Update is called once per frame
    public void talkUpdate()
    {
        //��b�I��
        if (_talkCompFlg == true)
        {
            if (TouchState.instance.getTouchEnded())
            {
                nextMessage();
            }
        }
    }
    
    //��b�J�n
    public void messageStart()
    {
        messageWindow.text.text = string.Empty;
        messageWindow.gameObject.SetActive(true);
        _talkCompFlg = false;

        // SetValue()�𖈃t���[���Ăяo���āA�P�b�ԂɂP����O�܂ł̒l�̒��Ԓl��n��        
        Hashtable hash = new Hashtable(){
            { "from", 0},
            { "to", messageList[_messageListIndex].Length},
            { "time", messageList[_messageListIndex].Length * messageSpeed},
            { "onupdate", "SetMessage"},
            { "oncomplete", "compMessage"}
        };
        iTween.ValueTo(gameObject, hash);
    }

    //iTween�p���b�Z�[�W�Z�b�g
    private void SetMessage(int num)
    {
        messageWindow.text.text = messageList[_messageListIndex].Substring(0, num);
    }

    //���b�Z�[�W�ǂݏI���
    private void compMessage()
    {
        _talkCompFlg = true;
    }

    //���̃��b�Z�[�W��
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

    //���b�Z�[�W�I��
    private void messageEnd()
    {
        //��ԏ�����
        messageWindow.text.text = string.Empty;
        messageWindow.gameObject.SetActive(false);
        _talkCompFlg = false;
        messageList = null;
        _messageListIndex = 0;
        //�C�x���g�I��
        ContentManager.instance.eventEnd();
    }

}