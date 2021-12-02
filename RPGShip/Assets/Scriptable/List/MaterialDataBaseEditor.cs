#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MaterialDataBase))]//拡張するクラスを指定
public class MaterialDataBaseEditor : Editor
{
    private const string PATH = "Assets/DataBase/material/";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("DataSet"))
        {
            //csvを読み込む
            var path = EditorUtility.OpenFilePanel("マテリアルCSV", "", "csv");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            //読み込みデータセット
            StreamReader reader = new StreamReader(path);
            //ターゲット設定
            MaterialDataBase dataBase = (MaterialDataBase)target;
            //リスト初期化
            dataBase.GetItemLists().Clear();
            setPos(reader, dataBase);
        }
    }

    //タイミングをずらしての位置セット
    private void setPos(StreamReader reader, MaterialDataBase dataBase)
    {
        //スプライトリストを取得
        Sprite[] sprites = Resources.LoadAll<Sprite>("");

        //文字列が読み込めなくなるまで繰り返し
        for (int i = 0; i < 256; i++)
        {
            string text = reader.ReadLine();
            //読み込めなくなったら抜ける
            if (text == null)
            {
                break;
            }
            //コメントアウトは飛ばす
            if (text.IndexOf("//") == 0)
            {
                continue;
            }
            //カンマ区切りで分割
            string[] csvData = text.Split(',');
            //データ作成
            MaterialData data = CreateInstance<MaterialData>();
            //データ設定
            data.id = int.Parse(csvData[1]);
            data.itemName = csvData[2];
            data.rarity = int.Parse(csvData[3]);
            //データ登録
            AssetDatabase.CreateAsset(data, PATH + data.id + "_" + data.itemName + ".asset");
            //リスト登録
            dataBase.GetItemLists().Add(data);
            EditorUtility.SetDirty(data);
        }
        //アセットのセーブ
        AssetDatabase.SaveAssets();        
        EditorUtility.SetDirty(dataBase);
    }
}

#endif