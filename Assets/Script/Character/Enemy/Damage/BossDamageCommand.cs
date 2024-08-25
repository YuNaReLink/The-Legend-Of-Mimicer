using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageCommand : EnemyDamageCommand
{
    private BossController bossController = null;
    public BossDamageCommand(BossController _bossController) : base(_bossController)
    {
        controller = _bossController;
        bossController = _bossController;
    }

    public override void Input()
    {
        StunInput();
    }
    private void StunInput()
    {
        if (!bossController.StunFlag) { return; }
        bossController.GetMotion().ChangeMotion(CharacterTag.StateTag.Damage);
        bossController.GetTimer().GetTimerStun().StartTimer(5f);
    }

    public override void Execute()
    {
        //���݂̍s������
        StunCommand();
        //�_���[�W���󂯂����̍s������
        if (!damageFlag) { return; }
        ToolController tool = attacker.GetComponent<ToolController>();
        if (tool == null) { return; }
        base.Execute();
        bossController.GetBossSoundController().PlaySESound((int)BossSoundController.BossSoundTag.Damage);
    }

    private void StunCommand()
    {
        if (!bossController.StunFlag) { return; }
        bossController.GetBossSoundController().PlaySESound((int)BossSoundController.BossSoundTag.WeakPointsDamage);
        bossController.StunFlag = false;
    }
}
