using UnityEngine;

/// <summary>
/// 剣の斬撃エフェクトを管理するクラス
/// </summary>
public class SwordEffectController : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer           trailRenderer = null;

    public void StopTrail()
    {
        if (trailRenderer == null) { return; }
        if (!trailRenderer.emitting) { return; }
        trailRenderer.emitting = false;
    }

    public void PlayTrail()
    {
        if (trailRenderer == null) { return; }
        if (trailRenderer.emitting) { return; }
        trailRenderer.emitting = true;
    }

}
