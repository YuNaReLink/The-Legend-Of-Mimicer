using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 壊すことができる箱の処理を行うクラス
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
            Debug.LogWarning("meshRendererがアタッチされていません");
        }
        boxCollider = GetComponent<BoxCollider>();
        if(boxCollider == null)
        {
            Debug.LogWarning("boxRendererがアタッチされていません");
        }
        soundController = GetComponent<SoundController>();
        if(soundController == null)
        {
            Debug.LogWarning("soundControllerがアタッチされていません");
        }
        effectController = GetComponent<EffectController>();
        if (effectController == null)
        {
            Debug.Log("effectControllerがアタッチされていません");
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
