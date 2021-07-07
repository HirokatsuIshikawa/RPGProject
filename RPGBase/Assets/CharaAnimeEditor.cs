

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharaAnime))]//拡張するクラスを指定
public class CharaAnimeEditor : Editor
{

    /// <summary>
    /// InspectorのGUIを更新
    /// </summary>
    /// 
    
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();


        //ボタンを表示
        if (GUILayout.Button("CharaSpriteSet"))
        {
        }
    }
    

}
#endif