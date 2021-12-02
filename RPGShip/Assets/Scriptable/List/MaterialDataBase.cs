using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialDataBase", menuName = "ItemList/MaterialDataBase")]
public class MaterialDataBase : ScriptableObject
{
    static public MaterialDataBase instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [SerializeField]
    private List<MaterialData> ItemList = new List<MaterialData>();

    public List<MaterialData> GetItemLists()
    {
        return ItemList;
    }
}