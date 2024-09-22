using UnityEngine;

public class HeartItem : Consumables
{
    private const int               Count = 1;
    public override ConsumablesTag  GetConsumablesTag(){return ConsumablesTag.Heart;}

    protected override void GetCommand(PlayerController _player, int count)
    {
        _player.RecoveryHelth(count);
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (getFlag) { return; }
        if (other.tag != "Player") { return; }
        player = other.GetComponent<PlayerController>();
        if(player == null) { return; }
        GetCommand(player,Count);
        base.OnTriggerEnter(other);
    }
}
