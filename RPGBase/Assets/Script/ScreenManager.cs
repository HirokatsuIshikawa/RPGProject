using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//��ʑS�̌���
public class ScreenManager : MonoBehaviour
{
    public Image darkField;

    public void changeColor(Color color, float time = 1.0f, string callBackStr = null)
    {
        // SetValue()�𖈃t���[���Ăяo���āA�P�b�ԂɂO����P�܂ł̒l�̒��Ԓl��n��
        //iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 1f, "time", 1f, "onupdate", "SetValue", "oncomplete", callBackStr));
        Hashtable hash = new Hashtable(){
            {"from", darkField.color},
            {"to", color},
            {"time", time},
            {"onupdate", "SetValue"}
        };
        if (callBackStr != null)
        {
            hash.Add("oncomplete", callBackStr);
        }
        iTween.ValueTo(gameObject, hash);
    }

    private void FadeIn(float time = 1.0f, string callBackStr = null)
    {
        // SetValue()�𖈃t���[���Ăяo���āA�P�b�ԂɂO����P�܂ł̒l�̒��Ԓl��n��
        //iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 1f, "time", 1f, "onupdate", "SetValue", "oncomplete", callBackStr));
        Hashtable hash = new Hashtable(){
            {"from", 0f},
            {"to", 1f},
            {"time", time},
            {"onupdate", "SetValue"}
        };
        if (callBackStr != null)
        {
            hash.Add("oncomplete", callBackStr);
        }
        iTween.ValueTo(gameObject, hash);
    }

    private void FadeOut(float time = 1.0f, string callBackStr = null)
    {
        // SetValue()�𖈃t���[���Ăяo���āA�P�b�ԂɂP����O�܂ł̒l�̒��Ԓl��n��        
        Hashtable hash = new Hashtable(){
            { "from", 1f},
            { "to", 0f},
            { "time", time},
            { "onupdate", "SetValue"}
        };
        if (callBackStr != null)
        {
            hash.Add("oncomplete", callBackStr);
        }
        iTween.ValueTo(gameObject, hash);
    }

    //�Ö��Z�b�g
    private void SetValue(float alpha)
    {
        // iTween�ŌĂ΂ꂽ��A�󂯎�����l��Image�̃A���t�@�l�ɃZ�b�g
        darkField.color = new Color(0, 0, 0, alpha);
    }

    //�F�Ö�
    private void SetValue(Color color)
    {
        // iTween�ŌĂ΂ꂽ��A�󂯎�����l��Image�̃A���t�@�l�ɃZ�b�g
        darkField.color = color;
    }

}
