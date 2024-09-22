using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

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
    public bool                 DamageFlag {  get { return damageFlag; }}
    public void                 SetDamageFlag(bool flag) {  damageFlag = flag; }
    public virtual void Execute()
    {
        //ダメージフラグがfalseなら早期リターン
        if (!damageFlag) { return; }
        //当たった時に代入したattackerからToolControllerを取得
        ToolController tool = attacker.GetComponent<ToolController>();
        //toolがnullなら
        if(tool == null) { return; }
        controller.CharacterStatus.HP -= tool.AttackPower;
        controller.GetKnockBackCommand().SetKnockBackFlag(true);
        controller.GetKnockBackCommand().SetAttacker(attacker);
        controller.GetTimer().GetTimerAttackCoolDown().StartTimer(2f);
        damageFlag = false;
        attacker = null;
        DeathCommand();
    }

    protected virtual void DeathCommand()
    {
        //HPが0以降なら
        if (controller.CharacterStatus.HP > 0) { return; }
        controller.GetKnockBackCommand().SetKnockBackFlag(false);
        controller.GetKnockBackCommand().SetAttacker(null);
        HitThrowManager.instance.StartHitStop(HitThrowManager.instance.GetHitStopCount);
        controller.Death();
    }

}
