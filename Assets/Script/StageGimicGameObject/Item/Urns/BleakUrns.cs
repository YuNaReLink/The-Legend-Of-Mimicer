using UnityEngine;

public class BleakUrns : MonoBehaviour
{
    private AppearRandomItem appearItem = null;
    private MeshRenderer meshRenderer = null;
    private MeshCollider meshCollider = null;
    private BreakSoundController soundController = null;
    private VFXController effectController = null;

    [SerializeField]
    private float breakSpeed = 0.5f;

    private void Awake()
    {
        appearItem = GetComponent<AppearRandomItem>();
        if(appearItem == null)
        {
            Debug.LogWarning("AppearRandomItem���A�^�b�`����Ă��܂���");
        }
        meshRenderer = GetComponent<MeshRenderer>();
        if(meshRenderer == null)
        {
            Debug.LogWarning("MeshRenderer���A�^�b�`����Ă��Ȃ�");
        }
        meshCollider = GetComponent<MeshCollider>();
        if(meshCollider == null)
        {
            Debug.LogWarning("MeshCollider���A�^�b�`����Ă��Ȃ�");
        }
        soundController = GetComponent<BreakSoundController>();
        if(soundController == null)
        {
            Debug.LogWarning("soundController���A�^�b�`����Ă��܂���");
        }
        effectController = GetComponent<VFXController>();
        if(effectController == null)
        {
            Debug.Log("effectController���A�^�b�`����Ă��܂���");
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
        effectController.CreateVFX((int)EffectTagList.CharacterEffectTag.Damage, transform.position, 1f, Quaternion.identity);
        Destroy(gameObject,breakSpeed);
    }
}
