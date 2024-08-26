using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleakUrns : MonoBehaviour
{
    private AppearRandomItem appearItem = null;
    private MeshRenderer meshRenderer = null;
    private MeshCollider meshCollider = null;
    private BreakSoundController soundController = null;

    [SerializeField]
    private float breakSpeed = 0.5f;

    private void Awake()
    {
        appearItem = GetComponent<AppearRandomItem>();
        if(appearItem == null)
        {
            Debug.LogWarning("AppearRandomItemがアタッチされていません");
        }
        meshRenderer = GetComponent<MeshRenderer>();
        if(meshRenderer == null)
        {
            Debug.LogWarning("MeshRendererがアタッチされていない");
        }
        meshCollider = GetComponent<MeshCollider>();
        if(meshCollider == null)
        {
            Debug.LogWarning("MeshColliderがアタッチされていない");
        }
        soundController = GetComponent<BreakSoundController>();
        if(soundController == null)
        {
            Debug.LogWarning("soundControllerがアタッチされていません");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Attack") { return; }
        if(soundController != null)
        {
            soundController.PlaySESound((int)BreakSoundController.BreakSoundTag.Break);
        }
        if(meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
        if(meshCollider != null)
        {
            meshCollider.enabled = false;
        }
        appearItem.Execute(other);
        Destroy(gameObject,breakSpeed);
    }
}
