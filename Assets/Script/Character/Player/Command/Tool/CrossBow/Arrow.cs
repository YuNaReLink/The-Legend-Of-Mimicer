using UnityEngine;

/// <summary>
/// 矢単体の処理
/// 矢が物に当たった時に処理を行う
/// </summary>
public class Arrow : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb = null;

    private void OnCollisionEnter(Collision collision)
    {
        if (rb == null) { return; }
        if (collision.collider.tag != "Obstacle") { return; }
        rb.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (rb == null) { return; }
        if (other.tag != "Obstacle") { return; }
        rb.velocity = Vector3.zero;
    }
}
