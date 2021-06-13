using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "ItemDataBase")]
public class ItemDataBase : ScriptableObject
{

    static public ItemDataBase instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    [SerializeField]
    private List<ItemData> ItemList = new List<ItemData>();

    public List<ItemData> GetShelterLists()
    {
        return ItemList;
    }
}