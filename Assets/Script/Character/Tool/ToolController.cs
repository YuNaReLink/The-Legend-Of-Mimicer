using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道具自体が道具を持っているキャラクターの情報を元に処理を行うベースクラス
/// </summary>
public class ToolController : BaseAttackController
{
    [SerializeField]
    protected ItemData itemData = null;
    public ItemData GetItemData() { return itemData; }
}
