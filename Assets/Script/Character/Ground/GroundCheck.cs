using UnityEngine;

[System.Serializable]
public class GroundCheck
{
    private Transform       transform;
    public void             SetTransform(Transform _transform) { transform = _transform; }

    [SerializeField]
    private float           groundCheckRadius = 0.4f;
    [SerializeField]
    private float           groundCheckOffsetY = 0.45f;
    [SerializeField]
    private float           groundCheckDistance = 0.2f;
    [SerializeField]
    private LayerMask       groundLayers = 0;


    private RaycastHit      hit;

    /// <summary>
    /// GroundCheck����
    /// SphereCast���g�p����true�Afalse�̏������s��
    /// </summary>
    /// <returns></returns>
    public bool CheckGroundStatus()
    {
        return Physics.SphereCast(transform.position + groundCheckOffsetY * Vector3.up,
            groundCheckRadius, Vector3.down, out hit, groundCheckDistance, groundLayers,
            QueryTriggerInteraction.Ignore);
    }
}