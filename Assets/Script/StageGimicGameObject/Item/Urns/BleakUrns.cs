using UnityEngine;

public class BleakUrns : MonoBehaviour
{
    /// <summary>
    /// Destroy����܂ł̃J�E���g��ݒ肷��ϐ�
    /// </summary>
    [SerializeField]
    private float               breakCount = 0.5f;
    /// <summary>
    /// �₪��ꂽ���ɃA�C�e�����o�������鏈�����s���N���X
    /// </summary>
    private AppearRandomItem    appearItem = null;
    /// <summary>
    /// ���Renderer
    /// </summary>
    private MeshRenderer        meshRenderer = null;
    /// <summary>
    /// ���Collider
    /// </summary>
    private MeshCollider        meshCollider = null;
    /// <summary>
    /// ��̌��ʉ����Ǘ�����N���X
    /// </summary>
    private SoundController     soundController = null;
    /// <summary>
    /// ��̃G�t�F�N�g���Ǘ�����N���X
    /// </summary>
    private EffectController    effectController = null;


    private void Awake()
    {
        appearItem = GetComponent<AppearRandomItem>();
        if(appearItem == null)
        {
            Debug.LogError("AppearRandomItem���A�^�b�`����Ă��܂���");
        }
        meshRenderer = GetComponent<MeshRenderer>();
        if(meshRenderer == null)
        {
            Debug.LogError("MeshRenderer���A�^�b�`����Ă��Ȃ�");
        }
        meshCollider = GetComponent<MeshCollider>();
        if(meshCollider == null)
        {
            Debug.LogError("MeshCollider���A�^�b�`����Ă��Ȃ�");
        }
        soundController = GetComponent<SoundController>();
        if(soundController != null)
        {
            soundController.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("soundController���A�^�b�`����Ă��܂���");
        }
        effectController = GetComponent<EffectController>();
        if(effectController == null)
        {
            Debug.LogError("effectController���A�^�b�`����Ă��܂���");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Attack") { return; }
        if(soundController != null)
        {
            soundController.PlaySESound((int)SoundTagList.BreakSoundTag.Break);
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
        Destroy(gameObject,breakCount);
    }
}
