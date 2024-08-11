using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBox : MonoBehaviour
{
    [SerializeField]
    private bool breakFlag = false;


    private void Update()
    {
        if (breakFlag)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Attack") { return; }
        breakFlag = true;
    }
}
