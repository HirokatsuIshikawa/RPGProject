using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//画面全体効果
public class ScreenManager
{
    public Image darkField;

    public ScreenManager(Image field)
    {
        darkField = field;
    }


    public void changeColor(Color color, float time = 1.0f, string callBackStr = null)
    {
        // SetValue()を毎フレーム呼び出して、１秒間に０から１までの値の中間値を渡す
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
        iTween.ValueTo(ContentManager.instance.gameObject, hash);
    }

    private void FadeIn(float time = 1.0f, string callBackStr = null)
    {
        // SetValue()を毎フレーム呼び出して、１秒間に０から１までの値の中間値を渡す
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
        iTween.ValueTo(ContentManager.instance.gameObject, hash);
    }

    private void FadeOut(float time = 1.0f, string callBackStr = null)
    {
        // SetValue()を毎フレーム呼び出して、１秒間に１から０までの値の中間値を渡す        
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
        iTween.ValueTo(ContentManager.instance.gameObject, hash);
    }

}
