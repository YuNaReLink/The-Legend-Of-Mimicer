
/// <summary>
/// 左手に持つ道具(盾)の処理を行うクラス
/// </summary>
public class LeftHandState : InterfaceState
{
    private PlayerController controller;

    public LeftHandState(PlayerController _controller)
    {
        controller = _controller;
    }

    public void DoUpdate()
    {
        ShieldInput();
    }

    private void ShieldInput()
    {
        if (controller.GetToolController().CheckNullToolObject(controller.GetToolController().GetInventoryData().ToolItemList[(int)ToolInventoryController.ToolObjectTag.Shild]))
        {
            return;
        }
        if (controller.LeftAction == null)
        {
            if (controller.GetKeyInput().GuardHoldButton)
            {
                //盾のクラスを生成する
                controller.LeftAction = new ShieldGuridCommand(controller);
            }
            else
            {
                controller.LeftAction = null;
            }
        }
        if (controller.LeftAction == null) { return; }
        //盾の入力
        controller.LeftAction.Input();
    }
}
