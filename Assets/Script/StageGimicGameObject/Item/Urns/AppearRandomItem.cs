using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearRandomItem : MonoBehaviour
{
    //�o��������A�C�e���̃��X�g
    [SerializeField]
    private ObjectList list = null;
    //�A�C�e���̔ԍ�
    public enum ItemNumber
    {
        Null = -1,
        Heart,
        Arrow,
        DataEnd
    }
    [SerializeField]
    private ItemNumber number = ItemNumber.Null;
    //�o��������m���̔䗦
    public enum RandomRatio
    {
        Null = -1,
        Low,
        Middle,
        High,
        DataEnd
    }
    //�m���̔䗦�̐��l
    [SerializeField]
    private int[] ratios = new int[]
    {
        100,
        500,
        1000,
    };
    //�䗦�ݒ�
    [SerializeField]
    private RandomRatio ratioNum = RandomRatio.Low;

    [SerializeField]
    private float lancherPowerY = 25f;
    [SerializeField]
    private float lancherPowerX = 5f;

    public void Execute(Collider other)
    {
        int ratio = Random.Range(0, ratios[(int)ratioNum]);
        if (ratio < ratios[(int)ratioNum] / 1.5f)
        {
            GameObject item = Instantiate(list.Objects[(int)number], transform.position, Quaternion.identity);
            Lancher(item,other);
        }
    }

    private void Lancher(GameObject item,Collider other)
    {
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb == null) { return; }
        Vector3 dis = transform.position - other.transform.position;
        rb.AddForce(dis * lancherPowerX, ForceMode.Impulse);
        rb.AddForce(item.transform.up * lancherPowerY, ForceMode.Impulse);
    }

}
