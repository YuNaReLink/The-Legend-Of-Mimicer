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
        if (controller.GetToolController().CheckNullToolObject(controller.GetToolController().Tools[(int)PlayerToolController.ToolObjectTag.Shild]))
        {
            return;
        }
        if (controller.LeftCommand == null)
        {
            if (!controller.GetKeyInput().RightMouseClick && !controller.BattleMode) { return; }
            //èÇÇÃÉNÉâÉXÇê∂ê¨Ç∑ÇÈ
            controller.LeftCommand = new ShieldGuridCommand(controller);
        }
        if (controller.LeftCommand == null) { return; }
        //èÇÇÃì¸óÕ
        controller.LeftCommand.Input();
    }
}
