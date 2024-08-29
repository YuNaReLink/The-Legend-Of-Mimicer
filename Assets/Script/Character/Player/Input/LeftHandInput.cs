
/// <summary>
/// ����Ɏ�����(��)�̏������s���N���X
/// </summary>
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
        if (controller.LeftAction == null)
        {
            if (controller.GetKeyInput().GuardHoldButton)
            {
                //���̃N���X�𐶐�����
                controller.LeftAction = new ShieldGuridAction(controller);
            }
            else
            {
                controller.LeftAction = null;
            }
        }
        if (controller.LeftAction == null) { return; }
        //���̓���
        controller.LeftAction.Input();
    }
}
