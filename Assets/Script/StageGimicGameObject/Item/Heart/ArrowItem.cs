using UnityEngine;

public class ArrowItem : Consumables
{
    [SerializeField]
    private int count = 5;
    public override ConsumablesTag GetConsumablesTag() { return ConsumablesTag.Arrow; }

    protected override float GetItemPositionY()
    {
        return 4f;
    }
    protected override void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<PlayerController>();
        if(player == null) { return; }
        var quiver = player.SelfObject().GetComponentInChildren<Quiver>();
        if(quiver != null)
        {
            quiver.AddArrow(count);
            GetCommand(player,count);
        }
        base.OnTriggerEnter(other);
    }
}
