
/// <summary>
/// EnemyDamageCommandを継承したクモのダメージ処理を行うクラス
/// </summary>
public class SpiderDamageCommand : EnemyDamageCommand
{
    private SpiderController spiderController = null;
    public SpiderDamageCommand(SpiderController _controller) : base(_controller)
    {
        controller = _controller;
        spiderController = _controller;
    }

    public override void Execute()
    {
        if (!damageFlag) { return; }
        ToolController tool = attacker.GetComponent<ToolController>();
        if (tool == null) { return; }
        base.Execute();
        spiderController.GetSpiderSoundController().PlaySESound((int)SpiderSoundController.SpiderSoundTag.Damage);
    }
}
