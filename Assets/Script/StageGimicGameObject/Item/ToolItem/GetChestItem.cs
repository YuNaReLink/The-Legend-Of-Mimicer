
/// <summary>
/// �󔠂���A�C�e�����擾���鏈�����s���N���X
/// ���e�͒��ڃA�C�e�����擾GetItem�Ɠ����Ȃ̂�GetItem���p��
/// </summary>
public class GetChestItem : GetItem
{
    public override void Get()
    {
        ToolInventoryController tool = triggerCheck.GetController().GetToolController();
        tool.GetToolSetting(toolTag, item);
        SetItemData();
    }
}
