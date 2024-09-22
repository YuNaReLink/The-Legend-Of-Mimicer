
/// <summary>
/// クロスボウの制御を管理するクラス
/// 主に矢の発射
/// </summary>
public class CrossBowTool : InterfaceBaseToolCommand
{
    private PlayerController    controller = null;

    private CrossBowShoot       shoot = null;

    public CrossBowTool(PlayerController _controller,CrossBowShoot _shot)
    {
        controller = _controller;
        shoot = _shot;
    }


    public void Input()
    {
        if(controller.GetCameraController() == null) { return; }
        if (!controller.GetCameraController().IsFPSMode()) { return; }
        if (!controller.GetKeyInput().ToolButton) { return; }
        if (controller.GetToolController().GetQuiver().Count <= 0) { return; }
        //矢を発射
        if (shoot.ArrowFire())
        {
            controller.GetToolController().GetQuiver().Count--;
            //矢発射効果音
            controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Shot);
        }
    }

    public void Execute(){}
}
