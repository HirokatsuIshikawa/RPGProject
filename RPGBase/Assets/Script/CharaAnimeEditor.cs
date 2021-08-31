#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharaAnime))]//拡張するクラスを指定
public class CharaAnimeEditor : Editor
{

    string spriteName = "";
    /// <summary>
    /// InspectorのGUIを更新
    /// </summary>
    /// 

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Label("CharaSprietName");
        spriteName = GUILayout.TextField(spriteName);
        //ボタンを表示
        if (GUILayout.Button("CharaSpriteSet"))
        {
            //対象データを取得
            CharaAnime data = (CharaAnime)target;
            //スプライトリストを取得
            Sprite[] sprites = Resources.LoadAll<Sprite>("charaList");
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
    private Sprite getSprite(Sprite[] sprites, string spriteName, int num)
    {
        string valueStr = string.Format("{0:D3}", num);
        return System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(spriteName + "_" + valueStr));
    }
}
#endif