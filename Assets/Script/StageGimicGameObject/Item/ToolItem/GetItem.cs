using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// アイテムを取得するクラス
/// </summary>
public class GetItem : MonoBehaviour
{
    [SerializeField]
    protected GameObject item = null;

    [SerializeField]
    protected ToolInventoryController.ToolObjectTag toolTag = ToolInventoryController.ToolObjectTag.Null;

    protected TriggerCheck triggerCheck = null;

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
        if (!InputManager.PushFKey()) { return; }
        Get();
    }

    public virtual void Get()
    {
        ToolInventoryController tool = triggerCheck.GetController().GetToolController();
        tool.GetToolSetting(toolTag, item);
        Destroy(gameObject);
    }
}
