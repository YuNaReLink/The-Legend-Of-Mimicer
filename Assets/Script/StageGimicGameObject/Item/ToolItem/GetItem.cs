using UnityEngine;

/// <summary>
/// アイテムを取得するクラス
/// </summary>
public class GetItem : MonoBehaviour
{
    [SerializeField]
    protected GameObject                                item = null;

    [SerializeField]
    protected ToolInventoryController.ToolObjectTag     toolTag = ToolInventoryController.ToolObjectTag.Null;

    protected TriggerCheck                              triggerCheck = null;

    private void Awake()
    {
        triggerCheck = GetComponentInChildren<TriggerCheck>();
    }

    private void Update()
    {
        PassingAnItem();
    }

    private void PassingAnItem()
    {
        if (!triggerCheck.IsHitPlayer()) { return; }
        if(triggerCheck.GetController() == null) { return; }
        if(item == null) { return; }
        if (!triggerCheck.GetController().GetKeyInput().IsGetButton()) { return; }
        Get();
    }

    public virtual void Get()
    {
        ToolInventoryController tool = triggerCheck.GetController().GetToolController();
        tool.GetToolSetting(toolTag, item);
        SetItemData();
        Destroy(gameObject);
    }

    protected void SetItemData()
    {
        if (item == null) { return; }
        ToolController tool = item.GetComponent<ToolController>();
        GetItemMessage.Instance.SetItemData(tool.GetItemData());
        item = null;
    }
}
