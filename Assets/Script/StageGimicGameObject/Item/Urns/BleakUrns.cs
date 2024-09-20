using UnityEngine;

/// <summary>
/// 壺を破壊する処理を書いてるクラス
/// </summary>
public class BleakUrns : MonoBehaviour
{
    /// <summary>
    /// Destroyするまでのカウントを設定する変数
    /// </summary>
    [SerializeField]
    private float               breakCount = 0.5f;
    /// <summary>
    /// 壺が壊れた時にアイテムを出現させる処理を行うクラス
    /// </summary>
    private AppearRandomItem    appearItem = null;
    /// <summary>
    /// 壺のRenderer
    /// </summary>
    private MeshRenderer        meshRenderer = null;
    /// <summary>
    /// 壺のCollider
    /// </summary>
    private MeshCollider        meshCollider = null;
    /// <summary>
    /// 壺の効果音を管理するクラス
    /// </summary>
    private SoundController     soundController = null;
    /// <summary>
    /// 壺のエフェクトを管理するクラス
    /// </summary>
    private EffectController    effectController = null;


    private void Awake()
    {
        appearItem = GetComponent<AppearRandomItem>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        soundController = GetComponent<SoundController>();
        effectController = GetComponent<EffectController>();

        if(appearItem == null)
        {
            Debug.LogError("AppearRandomItemがアタッチされていません");
        }
        if(meshRenderer == null)
        {
            Debug.LogError("MeshRendererがアタッチされていない");
        }
        if(meshCollider == null)
        {
            Debug.LogError("MeshColliderがアタッチされていない");
        }
        if(soundController != null)
        {
            soundController.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("soundControllerがアタッチされていません");
        }
        if(effectController == null)
        {
            Debug.LogError("effectControllerがアタッチされていません");
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
        effectController.CreateVFX((int)EffectTagList.BreakObjectTag.Break, transform.position, 1f, Quaternion.identity);
        Destroy(gameObject,breakCount);
    }
}
