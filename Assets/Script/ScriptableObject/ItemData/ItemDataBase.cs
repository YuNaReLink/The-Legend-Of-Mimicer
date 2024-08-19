using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create ItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField]
    private List<ItemData> itemList = new List<ItemData>();

    //ƒAƒCƒeƒ€‚ð•Ô‚·
    public List<ItemData> GetItemList()
    {
        return itemList;
    }
}
