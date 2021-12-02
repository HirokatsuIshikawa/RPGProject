#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharaAnime))]//拡張するクラスを指定
public class CharaAnimeEditor : Editor
{

    [NonSerialized]
    private Texture2D spriteTex;

    [NonSerialized]
    protected string spriteName = "";
    /// <summary>
    /// InspectorのGUIを更新
    /// </summary>
    /// 

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        
        spriteTex = (Texture2D)EditorGUILayout.ObjectField("master", spriteTex, typeof(Texture2D), false, null);
        GUILayout.Label("CharaSprietName");
        spriteName = GUILayout.TextField(spriteName);
        //ボタンを表示
        if (GUILayout.Button("CharaSpriteSet"))
        {
            //対象データを取得
            CharaAnime data = (CharaAnime)target;
            //レンダラーを取得
            if(data.render == null)
            {
                data.render = data.GetComponent<SpriteRenderer>();
            }
            
            //コライダーを取得
            if(data.colider == null)
            {
                data.colider = data.GetComponent<BoxCollider2D>();
                data.colider.offset = new Vector2(0, -0.11f);
                data.colider.size = new Vector2(data.chipSize.x - 0.16f, data.chipSize.y - 0.26f);
            }

            //スプライトリストを取得
            Sprite[] sprites = Resources.LoadAll<Sprite>(spriteTex.name);
            //スプライトレンダーのスプライト変更
            data.gameObject.GetComponent<SpriteRenderer>().sprite = getSprite(sprites, spriteName, 1);
            //上
            data.upSprite.Clear();
            data.upSprite.Add(getSprite(sprites, spriteName, 4));
            data.upSprite.Add(getSprite(sprites, spriteName, 5));
            data.upSprite.Add(getSprite(sprites, spriteName, 4));
            data.upSprite.Add(getSprite(sprites, spriteName, 6));
            //下
            data.downSprite.Clear();
            data.downSprite.Add(getSprite(sprites, spriteName, 1));
            data.downSprite.Add(getSprite(sprites, spriteName, 2));
            data.downSprite.Add(getSprite(sprites, spriteName, 1));
            data.downSprite.Add(getSprite(sprites, spriteName, 3));
            //右
            data.rightSprite.Clear();
            data.rightSprite.Add(getSprite(sprites, spriteName, 7));
            data.rightSprite.Add(getSprite(sprites, spriteName, 8));
            data.rightSprite.Add(getSprite(sprites, spriteName, 7));
            data.rightSprite.Add(getSprite(sprites, spriteName, 9));
            //下
            data.leftSprite.Clear();
            data.leftSprite.Add(getSprite(sprites, spriteName, 10));
            data.leftSprite.Add(getSprite(sprites, spriteName, 11));
            data.leftSprite.Add(getSprite(sprites, spriteName, 10));
            data.leftSprite.Add(getSprite(sprites, spriteName, 12));
            //アセットのセーブ
            Undo.RecordObject(data, "Set chara sprite");
            EditorUtility.SetDirty(data);
        }
    }

    //対象番号スプライト取得
    protected Sprite getSprite(Sprite[] sprites, string spriteName, int num)
    {
        string valueStr = string.Format("{0:D3}", num);
        return System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(spriteName + "_" + valueStr));
    }
}
#endif