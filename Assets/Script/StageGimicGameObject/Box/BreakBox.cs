using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �󂷂��Ƃ��ł��锠�̏������s���N���X
/// </summary>
public class BreakBox : MonoBehaviour
{
    private float               breakSpeed = 1.0f;
    private MeshRenderer        meshRenderer = null;
    private BoxCollider         boxCollider = null;
    private SoundController     soundController = null;
    private EffectController    effectController = null;
    private List<GameObject>    spriteObjectList = new List<GameObject>();

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if(meshRenderer == null)
        {
            Debug.LogWarning("meshRenderer���A�^�b�`����Ă��܂���");
        }
        boxCollider = GetComponent<BoxCollider>();
        if(boxCollider == null)
        {
            Debug.LogWarning("boxRenderer���A�^�b�`����Ă��܂���");
        }
        soundController = GetComponent<SoundController>();
        if(soundController == null)
        {
            Debug.LogWarning("soundController���A�^�b�`����Ă��܂���");
        }
        effectController = GetComponent<EffectController>();
        if (effectController == null)
        {
            Debug.Log("effectController���A�^�b�`����Ă��܂���");
        }
        GameObject g = null;
        for(int i = 0;i < transform.childCount; i++)
        {
            g = transform.GetChild(i).gameObject;
            spriteObjectList.Add(g);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Attack") { return; }
        if(soundController != null)
        {
            soundController.PlaySESound((int)SoundTagList.BreakSoundTag.Break);
        }
        if(meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
        if(boxCollider != null)
        {
            boxCollider.enabled = false;
        }
        for(int i = 0;i < spriteObjectList.Count; i++)
        {
            spriteObjectList[i].SetActive(false);
        }
        effectController.CreateVFX((int)EffectTagList.BreakObjectTag.Break, transform.position, 2f, Quaternion.identity);
        Destroy(gameObject, breakSpeed);
    }
}
