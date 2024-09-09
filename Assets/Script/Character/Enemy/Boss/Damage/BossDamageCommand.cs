
/// <summary>
/// EnemyDamageCommand���p������Boss�̃_���[�W�������s��
/// �N���X
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
        //���݃t���O��false�Ȃ瑁�����^�[��
        if (!bossController.StunFlag) { return; }
        //�_���[�W��Ԃ�ݒ�
        bossController.GetMotion().ChangeMotion(CharacterTagList.StateTag.Damage);
        //���݂��ێ����邽�߂Ƀ^�C�}�[���X�^�[�g
        bossController.GetTimer().GetTimerStun().StartTimer(stunTimerCount);
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
        bossController.GetRendererEffect().SetChangeFlag();
    }

    private void StunCommand()
    {
        //���݃t���O��false�Ȃ瑁�����^�[��
        if (!bossController.StunFlag) { return; }
        //���݃��[�V�������J�n
        bossController.GetBossSoundController().PlaySESound((int)BossSoundController.BossSoundTag.WeakPointsDamage);
        //���݃t���O��L���ɂ���
        bossController.SetStunFlag(false);
    }
}
