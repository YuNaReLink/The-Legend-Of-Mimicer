using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGimic : MonoBehaviour
{
    [SerializeField]
    private ColorChange colorChange = null;
    [SerializeField]
    private bool switchFlag = false;
    public bool IsSwitchFlag() {  return switchFlag; }
    // Start is called before the first frame update
    void Start()
    {
        colorChange = GetComponentInChildren<ColorChange>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (switchFlag) { return; }
        if(other.tag != "Attack") { return; }
        switchFlag = true;
        colorChange.ChangeMaterial(ColorChange.MaterialTag.Two);
    }
}
