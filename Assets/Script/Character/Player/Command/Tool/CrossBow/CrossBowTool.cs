using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBowTool : BaseToolCommand
{
    private PlayerController controller = null;

    private CrossBowShot shot = null;

    public CrossBowTool(PlayerController _controller,CrossBowShot _shot)
    {
        controller = _controller;
        shot = _shot;
    }

    public ToolTag GetToolTag()
    {
        return ToolTag.CrossBow;
    }

    public void Input()
    {
        if(controller.GetCameraScript() == null) { return; }
        if (!controller.GetCameraScript().IsFPSMode()) { return; }
        if (!controller.GetKeyInput().EKey) { return; }
        //–î‚ð”­ŽË
        shot.ArrowFire();
    }

    public void Execute()
    {

    }
}
