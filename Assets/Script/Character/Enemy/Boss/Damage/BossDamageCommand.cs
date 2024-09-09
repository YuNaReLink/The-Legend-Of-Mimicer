
/// <summary>
/// EnemyDamageCommandを継承したBossのダメージ処理を行う
/// クラス
/// </summary>
public class BossDamageCommand : EnemyDamageCommand, InterfaceState
{
    private BossController bossController = null;
    public BossDamageCommand(BossController _bossController) : base(_bossController)
    {
        controller = _bossController;
        bossController = _bossController;
    }

    public void DoUpdate()
    {
        StunInput();
    }
    private const float stunTimerCount = 7f;
    private void StunInput()
    {
        //怯みフラグがfalseなら早期リターン
        if (!bossController.StunFlag) { return; }
        //ダメージ状態を設定
        bossController.GetMotion().ChangeMotion(CharacterTagList.StateTag.Damage);
        //怯みを維持するためにタイマーをスタート
        bossController.GetTimer().GetTimerStun().StartTimer(stunTimerCount);
    }

    public override void Execute()
    {
        //怯みの行動処理
        StunCommand();
        //ダメージを受けた時の行動処理
        if (!damageFlag) { return; }
        ToolController tool = attacker.GetComponent<ToolController>();
        if (tool == null) { return; }
        base.Execute();
        bossController.GetBossSoundController().PlaySESound((int)BossSoundController.BossSoundTag.Damage);
        bossController.GetRendererEffect().SetChangeFlag();
    }

    private void StunCommand()
    {
        //怯みフラグがfalseなら早期リターン
        if (!bossController.StunFlag) { return; }
        //怯みモーションを開始
        bossController.GetBossSoundController().PlaySESound((int)BossSoundController.BossSoundTag.WeakPointsDamage);
        //怯みフラグを有効にする
        bossController.SetStunFlag(false);
    }
}
