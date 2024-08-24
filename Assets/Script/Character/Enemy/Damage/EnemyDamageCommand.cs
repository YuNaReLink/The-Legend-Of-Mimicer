using UnityEngine;

public class EnemyDamageCommand : InterfaceBaseCommand
{
    private EnemyController controller = null;
    public EnemyDamageCommand(EnemyController _controller)
    {
        controller = _controller;
    }
    private GameObject attacker = null;
    public GameObject Attacker { get { return attacker; } set { attacker = value; } }

    private bool damageFlag = false;
    public bool DamageFlag {  get { return damageFlag; } set { damageFlag = value; } }

    public void Input()
    {

    }

    public void Execute()
    {
        if (!damageFlag) { return; }
        ToolController tool = attacker.GetComponent<ToolController>();
        if(tool == null) { return; }
        WeaponStateData data = tool.GetStatusData();
        controller.HP -= data.BaseDamagePower;
        controller.Knockback(attacker.transform.position, data.KnockBackPower);
        damageFlag = false;
        attacker = null;
        if(controller.HP > 0) { return; }
        HitStopManager.instance.StartHitStop(0.1f);
        controller.Death();
    }

}
