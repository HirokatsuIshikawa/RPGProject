using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//画面全体効果
public class ScreenManager : MonoBehaviour
{
    public Image darkField;

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
        iTween.ValueTo(gameObject, hash);
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
        iTween.ValueTo(gameObject, hash);
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
        iTween.ValueTo(gameObject, hash);
    }

    //暗幕セット
    private void SetValue(float alpha)
    {
        // iTweenで呼ばれたら、受け取った値をImageのアルファ値にセット
        darkField.color = new Color(0, 0, 0, alpha);
    }

    //色暗幕
    private void SetValue(Color color)
    {
        // iTweenで呼ばれたら、受け取った値をImageのアルファ値にセット
        darkField.color = color;
    }

}
