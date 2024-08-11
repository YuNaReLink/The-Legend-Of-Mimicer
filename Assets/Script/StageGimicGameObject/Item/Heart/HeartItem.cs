using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartItem : Consumables
{
    public override ConsumablesTag GetConsumablesTag(){return ConsumablesTag.Heart;}

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") { return; }
        PlayerController player = other.GetComponent<PlayerController>();
        if(player == null) { return; }
        player.HP++;
        base.OnTriggerEnter(other);
    }
}
