using UnityEngine;
/// <summary>
/// アイテムの基底クラス
/// アイテムに共通する処理を行う
/// </summary>
public class Consumables : MonoBehaviour
{
    protected PlayerController          player = null;
    public enum ConsumablesTag
    {
        Null = -1,
        Heart,
        Arrow,
        DataEnd
    }
    public virtual ConsumablesTag       GetConsumablesTag() { return ConsumablesTag.Null; }

    protected Rigidbody rb = null;
    protected BoxCollider boxCollider = null;

    protected bool getFlag = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponentInChildren<BoxCollider>();
    }

    protected virtual void GetCommand(PlayerController _player,int count)
    {
        _player.GetArrowSound(count);
    }

    protected virtual float GetItemViewPositionY()
    {
        return 3f;
    }

    protected void Update()
    {
        if (player == null) { return; }
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 3f, player.transform.position.z);
        transform.Rotate(0,1,0);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (rb != null)
        {
            Destroy(rb);
        }
        if(boxCollider != null)
        {
            Destroy(boxCollider);
        }
        getFlag = true;
        Destroy(gameObject,1f);
    }
}
