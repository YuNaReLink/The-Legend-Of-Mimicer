using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VFXScriptableObject : ScriptableObject
{
    public enum VFXTag
    {
        Damage,
        Die,
    }
    [SerializeField]
    private List<GameObject> vfxArray = new List<GameObject>();
    public List<GameObject> GetVFXArray() {  return vfxArray; }
}
