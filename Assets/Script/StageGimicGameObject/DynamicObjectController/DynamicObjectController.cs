using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectController : MonoBehaviour
{
    private enum DynamicObjectNumber
    {
        Enemys,
        Item
    }
    [SerializeField]
    private List<GameObject> dynamicObjectList = new List<GameObject>();
    [SerializeField]
    private List<Vector3> itemPositionList = new List<Vector3>();

    [SerializeField]
    private GameObject urnsCopyObject = null;
    
    private void Start()
    {
        //子オブジェクトをリストに設定
        int childCount = transform.childCount;
        if(childCount != 0)
        {
            GameObject g = null;
            for(int i = 0; i < childCount; i++)
            {
                g = transform.GetChild(i).gameObject;
                dynamicObjectList.Add(g);
            }
        }
        //アイテムオブジェクトの子オブジェクトの位置を保存
        childCount = dynamicObjectList[(int)DynamicObjectNumber.Item].transform.childCount;
        if (childCount != 0)
        {
            Vector3 v = Vector3.zero;
            for (int i = 0; i < childCount; i++)
            {
                v = dynamicObjectList[(int)DynamicObjectNumber.Item].transform.GetChild(i).position;
                itemPositionList.Add(v);
            }
        }
        CreateItem();
    }

    private void ActiveEnemys(bool active)
    {
        if (dynamicObjectList[(int)DynamicObjectNumber.Enemys].activeSelf == active) { return; }
        dynamicObjectList[(int)DynamicObjectNumber.Enemys].SetActive(active);
    }

    private void CreateItem()
    {
        if(dynamicObjectList[(int)DynamicObjectNumber.Item].transform.childCount > 0) { return; }
        GameObject g = null;
        for(int i = 0;i < itemPositionList.Count; i++)
        {
            g = Instantiate(urnsCopyObject, itemPositionList[i],Quaternion.identity);
            g.transform.SetParent(dynamicObjectList[(int)DynamicObjectNumber.Item].transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") { return; }
        //ActiveEnemys(true);
        CreateItem();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") { return; }
        //ActiveEnemys(false);
    }
}
