using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EffectTagList
{
    public enum CharacterEffectTag
    {
        Damage,
        Death
    }

    public enum BreakObjectTag
    {
        Break
    }
}
public class VFXController : MonoBehaviour
{
    [SerializeField]
    private VFXScriptableObject vfxObjects = null;

    public void CreateVFX(int number,Vector3 position,float scale,Quaternion quaternion)
    {
        GameObject effect = Instantiate(vfxObjects.GetVFXArray()[number], position, quaternion);
        effect.transform.localScale *= scale;
    }
}
