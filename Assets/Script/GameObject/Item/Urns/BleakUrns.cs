using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleakUrns : MonoBehaviour
{

    [SerializeField]
    private ObjectList list = null;
    public enum ItemNumber
    {
        Null = -1,
        Heart,
        DataEnd
    }

    public enum RandomRatio
    {
        Null = -1,
        Low,
        Middle,
        High,
        DataEnd
    }

    [SerializeField]
    private int[] ratios = new int[]
    {
        100,
        500,
        1000,
    };

    [SerializeField]
    private RandomRatio ratioNum = RandomRatio.Low;

    private void AppearItem()
    {
        int ratio = Random.Range(0,ratios[(int)ratioNum]);
        if(ratio < ratios[(int)ratioNum] / 2)
        {
            Instantiate(list.Objects[(int)ItemNumber.Heart],transform.position,Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Attack") { return; }
        AppearItem();
        Destroy(gameObject);
    }
}
