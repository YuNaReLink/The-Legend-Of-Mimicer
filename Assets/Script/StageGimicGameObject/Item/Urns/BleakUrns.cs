using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleakUrns : MonoBehaviour
{

    [SerializeField]
    private AppearRandomItem appearItem = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Attack") { return; }
        appearItem.Execute(other);
        Destroy(gameObject);
    }
}
