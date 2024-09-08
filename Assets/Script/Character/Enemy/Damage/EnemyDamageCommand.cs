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
        //ダメージフラグがfalseなら早期リターン
        if (!damageFlag) { return; }
        //当たった時に代入したattackerからToolControllerを取得
        ToolController tool = attacker.GetComponent<ToolController>();
        //toolがnullなら
        if(tool == null) { return; }
        //tool内にあるデータからHPを減らす
        WeaponStateData data = tool.GetStatusData();
        controller.CharacterStatus.HP -= data.BaseDamagePower;
        controller.GetKnockBackCommand().KnockBackFlag = true;
        controller.GetKnockBackCommand().Attacker = attacker;
        controller.GetTimer().GetTimerAttackCoolDown().StartTimer(2f);
        damageFlag = false;
        attacker = null;
        DeathCommand();
    }

    protected virtual void DeathCommand()
    {
        //HPが0以降なら
        if (controller.CharacterStatus.HP > 0) { return; }
        controller.GetKnockBackCommand().KnockBackFlag = false;
        controller.GetKnockBackCommand().Attacker = null;
        HitStopManager.instance.StartHitStop(0.5f);
        controller.Death();
    }

}
