
/// <summary>
/// EnemyDamageCommandを継承したキノコモンスターのダメージ処理を行うクラス
/// </summary>
public class MushroomDamageCommand : EnemyDamageCommand
{
    private MushroomController mushroomController = null;
    public MushroomDamageCommand(MushroomController _controller) : base(_controller)
    {
        controller = _controller;
        mushroomController = _controller;
    }

    public override void Execute()
    {
        if (!damageFlag) { return; }
        ToolController tool = attacker.GetComponent<ToolController>();
        if (tool == null) { return; }
        mushroomController.GetMotion().ChangeMotion(CharacterTagList.StateTag.Damage);
        mushroomController.GetSoundController().PlaySESound((int)SoundTagList.MushroomSoundTag.Damage);
        mushroomController.GetRendererEffect().SetChangeFlag();
        base.Execute();
    }
}
