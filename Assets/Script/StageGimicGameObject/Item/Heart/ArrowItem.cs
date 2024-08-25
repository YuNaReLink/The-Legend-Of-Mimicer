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
        var player = other.GetComponent<PlayerController>();
        if(player == null) { return; }
        var quiver = player.gameObject.GetComponentInChildren<Quiver>();
        if(quiver == null) { return; }
        quiver.AddArrow(count);
        GetCommand(player,count);
        base.OnTriggerEnter(other);
    }
}
