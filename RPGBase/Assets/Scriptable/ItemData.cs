using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ItemData/ItemData")]//  CreateからCreateShelterというメニューを表示し、Shelterを作成する
public class ItemData : ScriptableObject
{

    [SerializeField]
    public int id;//　避難場所の名前
    [SerializeField]
    public string itemName;//　避難場所の名前
    [SerializeField]
    public int value;
    [SerializeField]
    public Sprite icon;//　避難場所のアイコン

    public Sprite GetIcon()//  アイコンを入力したら、
    {
        return icon;//  iconに返す
    }
}