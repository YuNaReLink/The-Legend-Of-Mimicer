using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;
    void Update()
    {
        Vector3 pos = transform.position;
        float sin = Mathf.Sin(Time.time);
        transform.position = new Vector3(sin * speed,pos.y, pos.z);
    }
}
