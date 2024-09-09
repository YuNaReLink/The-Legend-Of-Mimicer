using UnityEngine;

/// <summary>
/// ���̎a���G�t�F�N�g���Ǘ�����N���X
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
