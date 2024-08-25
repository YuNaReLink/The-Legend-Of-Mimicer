using UnityEngine;

public class EnemyDamageCommand : InterfaceBaseCommand
{
    protected EnemyController controller = null;
    public EnemyDamageCommand(EnemyController _controller)
    {
        controller = _controller;
    }
    protected GameObject attacker = null;
    public GameObject Attacker { get { return attacker; } set { attacker = value; } }

    protected bool damageFlag = false;
    public bool DamageFlag {  get { return damageFlag; } set { damageFlag = value; } }

    public virtual void Input()
    {

    }

    public virtual void Execute()
    {
        if (!damageFlag) { return; }
        ToolController tool = attacker.GetComponent<ToolController>();
        if(tool == null) { return; }
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
        if (controller.HP > 0) { return; }
        HitStopManager.instance.StartHitStop(0.1f);
        controller.Death();
    }

}
