using UnityEngine;

public class EnemyDamageCommand : InterfaceBaseCommand
{
    protected EnemyController   controller = null;
    public EnemyDamageCommand(EnemyController _controller)
    {
        controller = _controller;
    }
    protected GameObject        attacker = null;
    public GameObject           Attacker { get { return attacker; } set { attacker = value; } }

    protected bool              damageFlag = false;
    public bool                 DamageFlag {  get { return damageFlag; } set { damageFlag = value; } }

    public virtual void Execute()
    {
        //�_���[�W�t���O��false�Ȃ瑁�����^�[��
        if (!damageFlag) { return; }
        //�����������ɑ������attacker����ToolController���擾
        ToolController tool = attacker.GetComponent<ToolController>();
        //tool��null�Ȃ�
        if(tool == null) { return; }
        //tool���ɂ���f�[�^����HP�����炷
        WeaponStateData data = tool.GetStatusData();
        controller.HP -= data.BaseDamagePower;
        controller.GetKnockBackCommand().KnockBackFlag = true;
        controller.GetKnockBackCommand().Attacker = attacker;
        damageFlag = false;
        attacker = null;
        DeathCommand();
    }

    protected virtual void DeathCommand()
    {
        //HP��0�ȍ~�Ȃ�
        if (controller.HP > 0) { return; }
        HitStopManager.instance.StartHitStop(0.5f);
        controller.Death();
    }

}
