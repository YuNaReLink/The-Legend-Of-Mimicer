using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create SpriteObjectData")]
public class SpriteObjectData : ScriptableObject
{
    [SerializeField]
    private List<Sprite> spriteList = new List<Sprite>();
    public List<Sprite> SpriteList {  get { return spriteList; } }
}
