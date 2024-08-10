using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleakUrns : MonoBehaviour
{

    [SerializeField]
    private ObjectList list = null;

    private void AppearItem()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Attack") { return; }
        AppearItem();
        Destroy(gameObject);
    }
}
