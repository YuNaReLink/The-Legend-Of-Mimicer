
/// <summary>
/// 宝箱からアイテムを取得する処理を行うクラス
/// 内容は直接アイテムを取得GetItemと同じなのでGetItemを継承
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
