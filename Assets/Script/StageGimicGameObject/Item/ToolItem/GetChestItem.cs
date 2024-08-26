using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetChestItem : GetItem
{
    public override void Get()
    {
        ToolInventoryController tool = triggerCheck.GetController().GetToolController();
        tool.GetToolSetting(toolTag, item);
    }
}
