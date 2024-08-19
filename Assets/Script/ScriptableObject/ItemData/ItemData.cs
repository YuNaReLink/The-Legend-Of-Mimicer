using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create ItemData")]
public class ItemData : ScriptableObject
{
    public enum Itemtype
    {
        Sowrd,
        Shield,
        CrossBow
    }
    //アイテムの名前
    [SerializeField]
    private string itemName;
    //アイテムのタイプ
    [SerializeField]
    private Itemtype itemType;
    //アイテムのアイコン
    [SerializeField]
    private Sprite itemIcon;
    //アイテムの説明
    [SerializeField]
    private string itemExplanation;
    //アイテムの持てる最大数
    [SerializeField]
    private int itemLimit;

    public string ItemName {  get { return itemName; } }
    public Itemtype ItemType { get { return itemType; } }
    public Sprite ItemIcon { get { return itemIcon; } }
    public string ItemExplanation { get { return itemExplanation; } }
    public int ItemLimit { get { return itemLimit; } }
}
