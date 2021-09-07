using UnityEngine;

[CreateAssetMenu(fileName = "Material", menuName = "ItemData/MaterialData")]//  CreateからCreateShelterというメニューを表示し、Shelterを作成する
public class MaterialData : ScriptableObject
{

    [SerializeField]
    public int id;//　ID
    [SerializeField]
    public string itemName;//　名前
    [SerializeField]
    public int rarity;  //レアリティ
    [SerializeField]
    public Sprite icon;//　避難場所のアイコン

    public Sprite GetIcon()//  アイコンを入力したら、
    {
        return icon;//  iconに返す
    }
}