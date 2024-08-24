using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearRandomItem : MonoBehaviour
{
    //出現させるアイテムのリスト
    [SerializeField]
    private ObjectList list = null;
    //アイテムの番号
    public enum ItemNumber
    {
        Null = -1,
        Heart,
        Arrow,
        DataEnd
    }
    [SerializeField]
    private ItemNumber number = ItemNumber.Null;
    //出現させる確率の比率
    public enum RandomRatio
    {
        Null = -1,
        Low,
        Middle,
        High,
        DataEnd
    }
    //確率の比率の数値
    [SerializeField]
    private int[] ratios = new int[]
    {
        100,
        500,
        1000,
    };
    //比率設定
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
