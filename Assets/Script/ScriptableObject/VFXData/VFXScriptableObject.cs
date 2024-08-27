using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create VFXScriptableObject")]
public class VFXScriptableObject : ScriptableObject
{
    [SerializeField]
    private List<GameObject> vfxArray = new List<GameObject>();
    public List<GameObject> GetVFXArray() {  return vfxArray; }
}
