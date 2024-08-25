using UnityEngine;

public class CrossBowTool : BaseToolCommand
{
    private PlayerController controller = null;

    private CrossBowShoot shoot = null;

    public CrossBowTool(PlayerController _controller,CrossBowShoot _shot)
    {
        controller = _controller;
        shoot = _shot;
    }

    public ToolTag GetToolTag()
    {
        return ToolTag.CrossBow;
    }

    public void Input()
    {
        if(controller.GetCameraController() == null) { return; }
        if (!controller.GetCameraController().IsFPSMode()) { return; }
        if (!controller.GetKeyInput().ToolButton) { return; }
        if (controller.GetToolController().GetQuiver().Count <= 0) { return; }
        //–î‚ð”­ŽË
        if (shoot.ArrowFire())
        {
            controller.GetToolController().GetQuiver().Count--;
            //–î”­ŽËŒø‰Ê‰¹
            controller.GetSoundController().PlaySESound((int)PlayerSoundController.PlayerSoundTag.Shot);
        }
    }

    public void Execute(){}
}
