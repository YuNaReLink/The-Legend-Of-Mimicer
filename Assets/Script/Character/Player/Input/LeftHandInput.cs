using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandInput
{
    private PlayerController controller;

    public LeftHandInput(PlayerController _controller)
    {
        controller = _controller;
    }

    public void Execute()
    {
        ShieldInput();
    }

    private void ShieldInput()
    {
        if (controller.GetToolController().CheckNullToolObject(controller.GetToolController().GetInventoryData().ToolItemList[(int)ToolInventoryController.ToolObjectTag.Shild]))
        {
            return;
        }
        if (controller.LeftCommand == null)
        {
            if (controller.GetKeyInput().GuardHoldButton)
            {
                //èÇÇÃÉNÉâÉXÇê∂ê¨Ç∑ÇÈ
                controller.LeftCommand = new ShieldGuridCommand(controller);
            }
            else
            {
                controller.LeftCommand = null;
            }
        }
        if (controller.LeftCommand == null) { return; }
        //èÇÇÃì¸óÕ
        controller.LeftCommand.Input();
    }
}
