using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetChestItem : GetItem
{

    public void SetController(PlayerController _controller)
    {
        controller = _controller;
    }

    public override void Get()
    {
        PlayerToolController tool = controller.GetToolController();
        tool.GetToolSetting(toolTag, item);
    }
}
