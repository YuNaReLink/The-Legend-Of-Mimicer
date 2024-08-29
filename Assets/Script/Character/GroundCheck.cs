using UnityEngine;

public class GroundCheck : MonoBehaviour
{
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
    /// DrawLineでGroundCheckを確認
    /// </summary>
    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position, transform.position + groundCheckOffsetY * Vector3.up,Color.black);
    }
    /// <summary>
    /// GroundCheck処理
    /// SphereCastを使用してtrue、falseの処理を行う
    /// </summary>
    /// <returns></returns>
    public bool CheckGroundStatus()
    {
        return Physics.SphereCast(transform.position + groundCheckOffsetY * Vector3.up,
            groundCheckRadius, Vector3.down, out hit, groundCheckDistance, groundLayers,
            QueryTriggerInteraction.Ignore);
    }
}
