using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create ItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField]
    private List<ItemData> itemList = new List<ItemData>();

    //�A�C�e����Ԃ�
    public List<ItemData> GetItemList()
    {
        return itemList;
    }
}
