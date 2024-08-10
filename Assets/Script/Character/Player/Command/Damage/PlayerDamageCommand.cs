using CharacterTag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCommand : InterfaceBaseCommand
{
    private PlayerController controller = null;
    public PlayerDamageCommand(PlayerController _controller)
    {
        controller = _controller;
    }

    private GameObject attacker = null;
    public GameObject Attacker { get { return attacker; }set { attacker = value; } }
    /// <summary>
    /// ダメージ時に発生する状態とモーションを適用する
    /// </summary>
    public void Input()
    {
        if(controller.DamageTag == DamageTag.Null) { return; }
        switch (controller.DamageTag)
        {
            case DamageTag.Fall:
                controller.GetMotion().ChangeMotion(StateTag.Damage);
                controller.GetFallDistanceCheck().FallDamage = false;
                break;
            case DamageTag.NormalAttack:
                controller.GetMotion().ChangeMotion(StateTag.Damage);
                break;
            case DamageTag.GreatAttack:
                break;
        }
    }

    /// <summary>
    /// 体力を減らす&プレイヤーオブジェクトを動かす処理をする
    /// </summary>
    public void Execute()
    {
        if (controller.DamageTag == DamageTag.Null) { return; }
        Damage();
    }

    private void Damage()
    {
        switch (controller.DamageTag)
        {
            case DamageTag.Fall:
                controller.HP--;
                controller.DamageTag = DamageTag.Null;
                break;
            case DamageTag.NormalAttack:
            case DamageTag.GreatAttack:
                if (attacker == null) { return; }
                ToolController tool = attacker.GetComponent<ToolController>();
                if (tool == null) { return; }
                WeaponStateData data = tool.GetStatusData();
                if(data == null) { return; }
                controller.HP-= data.BaseDamagePower;
                controller.Knockback(attacker.transform.position,data.KnockBackPower);
                attacker = null;
                controller.DamageTag = DamageTag.Null;
                break;
        }
        if(controller.HP > 0) { return; }
        controller.Death();
    }
}
