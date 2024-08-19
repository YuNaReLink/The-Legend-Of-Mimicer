using System.Collections.Generic;
using UnityEngine;

public class InventoryData : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> toolItemList = new List<GameObject>();
    public List<GameObject> ToolItemList { get { return toolItemList; } set { toolItemList = value; } }
}
