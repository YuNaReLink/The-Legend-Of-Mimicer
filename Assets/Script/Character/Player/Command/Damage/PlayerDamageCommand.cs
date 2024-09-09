using UnityEngine;

/// <summary>
/// プレイヤーのダメージ処理を行うクラス
/// </summary>
public class PlayerDamageCommand : InterfaceBaseCommand
{
    private PlayerController        controller = null;
    public PlayerDamageCommand(PlayerController _controller)
    {
        controller = _controller;
    }

    private GameObject              attacker = null;
    public GameObject               Attacker { get { return attacker; }set { attacker = value; } }

    /// <summary>
    /// 体力を減らす&プレイヤーオブジェクトを動かす処理をする
    /// </summary>
    public void Execute()
    {
        if (controller.DamageTag == CharacterTagList.DamageTag.Null) { return; }
        controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Damage);
        controller.GetRendererEffect().SetChangeFlag();
        Damage();
    }

    private void Damage()
    {
        switch (controller.DamageTag)
        {
            case CharacterTagList.DamageTag.Fall:
                controller.CharacterStatus.HP--;
                controller.DamageTag = CharacterTagList.DamageTag.Null;
                break;
            case CharacterTagList.DamageTag.NormalAttack:
            case CharacterTagList.DamageTag.GreatAttack:
                if (attacker == null) { return; }
                BaseAttackController tool = attacker.GetComponent<BaseAttackController>();
                if (tool == null) { return; }
                controller.CharacterStatus.HP-= tool.AttackPower;
                if (EnabledknockBackCheck())
                {
                    controller.GetKnockBackCommand().SetKnockBackFlag(true);
                    controller.GetKnockBackCommand().SetAttacker(attacker);
                }
                controller.GetEffectController().CreateVFX((int)EffectTagList.CharacterEffectTag.Damage,attacker.transform.position,1f,Quaternion.identity);
                attacker = null;
                controller.DamageTag = CharacterTagList.DamageTag.Null;
                break;
        }
        if(controller.CharacterStatus.HP > 0) { return; }
        controller.Death();
    }

    private bool EnabledknockBackCheck()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Grab:
            case CharacterTagList.StateTag.WallJump:
            case CharacterTagList.StateTag.ClimbWall:
                return false;
        }
        return true;
    }
}
