using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    [SerializeField]
    private VFXScriptableObject vfxObjects = null;

    public void CreateVFX(VFXScriptableObject.VFXTag vfxNumber,Vector3 position,float scale,Quaternion quaternion)
    {
        GameObject effect = Instantiate(vfxObjects.GetVFXArray()[(int)vfxNumber], position, quaternion);
        effect.transform.localScale *= scale;
    }
}
