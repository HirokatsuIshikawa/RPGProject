using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ItemData")]//  CreateからCreateShelterというメニューを表示し、Shelterを作成する
public class ItemData : ScriptableObject
{
    [SerializeField]
    public Sprite icon;//　避難場所のアイコン

    [SerializeField]
    public string Name;//　避難場所の名前
    [SerializeField]
    public int value;

    public Sprite GetIcon()//  アイコンを入力したら、
    {
        return icon;//  iconに返す
    }
}