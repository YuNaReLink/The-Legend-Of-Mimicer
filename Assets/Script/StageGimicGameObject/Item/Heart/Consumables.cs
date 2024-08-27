using UnityEngine;

public class Consumables : MonoBehaviour
{
    public enum ConsumablesTag
    {
        Null = -1,
        Heart,
        Arrow,
        DataEnd
    }
    public virtual ConsumablesTag GetConsumablesTag() { return ConsumablesTag.Null; }

    protected virtual void GetCommand(PlayerController _player,int count)
    {
        _player.GetArrow(count);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
