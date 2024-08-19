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
    //�A�C�e���̖��O
    [SerializeField]
    private string itemName;
    //�A�C�e���̃^�C�v
    [SerializeField]
    private Itemtype itemType;
    //�A�C�e���̃A�C�R��
    [SerializeField]
    private Sprite itemIcon;
    //�A�C�e���̐���
    [SerializeField]
    private string itemExplanation;
    //�A�C�e���̎��Ă�ő吔
    [SerializeField]
    private int itemLimit;

    public string ItemName {  get { return itemName; } }
    public Itemtype ItemType { get { return itemType; } }
    public Sprite ItemIcon { get { return itemIcon; } }
    public string ItemExplanation { get { return itemExplanation; } }
    public int ItemLimit { get { return itemLimit; } }
}
