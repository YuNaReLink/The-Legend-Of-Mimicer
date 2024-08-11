using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumables : MonoBehaviour
{
    public enum ConsumablesTag
    {
        Null = -1,
        Heart,
        DataEnd
    }
    public virtual ConsumablesTag GetConsumablesTag() { return ConsumablesTag.Null; }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
