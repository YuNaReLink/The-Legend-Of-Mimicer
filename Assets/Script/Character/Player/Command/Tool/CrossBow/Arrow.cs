using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
