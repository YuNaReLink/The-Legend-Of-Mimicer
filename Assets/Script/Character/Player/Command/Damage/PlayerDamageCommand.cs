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
                controller.GetMotion().ForcedChangeMotion(StateTag.Damage);
                controller.GetFallDistanceCheck().FallDamage = false;
                break;
            case DamageTag.NormalAttack:
                controller.GetMotion().ForcedChangeMotion(StateTag.Damage);
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
        controller.GetSoundController().PlaySESound((int)PlayerSoundController.PlayerSoundTag.Damage);
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
                controller.GetKnockBackCommand().KnockBackFlag = true;
                controller.GetKnockBackCommand().Attacker = attacker;
                controller.GetVFXController().CreateVFX((int)EffectTagList.CharacterEffectTag.Damage,attacker.transform.position,1f,Quaternion.identity);
                attacker = null;
                controller.DamageTag = DamageTag.Null;
                break;
        }
        if(controller.HP > 0) { return; }
        controller.Death();
    }
}
