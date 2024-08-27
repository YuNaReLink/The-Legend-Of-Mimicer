using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create ObjectList")]
public class ObjectList : ScriptableObject
{
    [SerializeField]
    private List<GameObject> objects = new List<GameObject>();

    public List<GameObject> Objects {  get { return objects; } }
}
