
public class ModeChangeState : InterfaceState
{
    private PlayerController controller = null;

    public ModeChangeState(PlayerController _controller)
    {
        controller = _controller;
    }

    public void DoUpdate()
    {
        switch (controller.CurrentState)
        {
            case CharacterTagList.StateTag.Idle:
            case CharacterTagList.StateTag.ChangeMode:
                break;
            default:
                return;
        }
        PlayerInput input = controller.GetKeyInput();
        if (!controller.Landing) { return; }
        if (!input.ChangeButton) { return; }
        if (controller.GetToolController().CheckNullToolObject(controller.GetToolController().GetInventoryData().ToolItemList[(int)ToolInventoryController.ToolObjectTag.Sword])) { return; }
        if (controller.GetToolController().CurrentToolTag == ToolInventoryController.ToolObjectTag.CrossBow) { return; }
        controller.BattleMode = !controller.BattleMode;
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.ChangeMode);
    }
}
