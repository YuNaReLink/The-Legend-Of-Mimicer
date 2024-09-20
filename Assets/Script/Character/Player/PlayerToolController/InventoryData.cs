using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーが持つ道具をまとめて保持するクラス
/// </summary>
public class InventoryData : MonoBehaviour
{
    [SerializeField]
    private List<GameObject>    toolItemList = new List<GameObject>();
    public List<GameObject>     ToolItemList { get { return toolItemList; } set { toolItemList = value; } }
}
