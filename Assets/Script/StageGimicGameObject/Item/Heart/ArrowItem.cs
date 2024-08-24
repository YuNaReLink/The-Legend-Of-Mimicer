using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowItem : Consumables
{
    [SerializeField]
    private int count = 5;
    public override ConsumablesTag GetConsumablesTag() { return ConsumablesTag.Arrow; }

    protected override void OnTriggerEnter(Collider other)
    {
        var quiver = other.GetComponentInChildren<Quiver>();
        if(quiver == null) { return; }
        quiver.AddArrow(count);
        base.OnTriggerEnter(other);
    }
}
