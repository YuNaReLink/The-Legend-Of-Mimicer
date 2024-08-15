using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTitleUIConfiguration : MonoBehaviour
{
    [SerializeField]
    protected bool fadeEnd = false;
    public bool IsFadeEnd() { return fadeEnd; }

    [SerializeField]
    protected List<GameObject> uiObjectArray = new List<GameObject>();
    public List<GameObject> GetUIObjectArray() { return uiObjectArray; }

    public virtual void Initilaize()
    {
        int childCount = transform.childCount;
        if(childCount != 0)
        {
            GameObject childObject = null;
            for(int i = 0; i < childCount; i++)
            {
                childObject = transform.GetChild(i).gameObject;
                uiObjectArray.Add(childObject);
            }
        }
    }

    public virtual void ConfigurationUpdate()
    {

    }
}
