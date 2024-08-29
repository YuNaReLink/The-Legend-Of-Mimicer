using UnityEngine;

public class PlayerDamageCommand : InterfaceBaseCommand, InterfaceBaseInput
{
    private PlayerController        controller = null;
    public PlayerDamageCommand(PlayerController _controller)
    {
        controller = _controller;
    }

    private GameObject              attacker = null;
    public GameObject               Attacker { get { return attacker; }set { attacker = value; } }
    /// <summary>
    /// �_���[�W���ɔ��������Ԃƃ��[�V������K�p����
    /// </summary>
    public void Input()
    {
        if(controller.DamageTag == CharacterTagList.DamageTag.Null) { return; }
        switch (controller.DamageTag)
        {
            case CharacterTagList.DamageTag.Fall:
                controller.GetMotion().ForcedChangeMotion(CharacterTagList.StateTag.Damage);
                controller.GetFallDistanceCheck().FallDamage = false;
                break;
            case CharacterTagList.DamageTag.NormalAttack:
                controller.GetMotion().ForcedChangeMotion(CharacterTagList.StateTag.Damage);
                break;
            case CharacterTagList.DamageTag.GreatAttack:
                break;
        }
    }

    /// <summary>
    /// �̗͂����炷&�v���C���[�I�u�W�F�N�g�𓮂�������������
    /// </summary>
    public void Execute()
    {
        if (controller.DamageTag == CharacterTagList.DamageTag.Null) { return; }
        controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Damage);
        Damage();
    }

    private void Damage()
    {
        switch (controller.DamageTag)
        {
            case CharacterTagList.DamageTag.Fall:
                controller.HP--;
                controller.DamageTag = CharacterTagList.DamageTag.Null;
                break;
            case CharacterTagList.DamageTag.NormalAttack:
            case CharacterTagList.DamageTag.GreatAttack:
                if (attacker == null) { return; }
                ToolController tool = attacker.GetComponent<ToolController>();
                if (tool == null) { return; }
                WeaponStateData data = tool.GetStatusData();
                if(data == null) { return; }
                controller.HP-= data.BaseDamagePower;
                controller.GetKnockBackCommand().KnockBackFlag = true;
                controller.GetKnockBackCommand().Attacker = attacker;
                controller.GetEffectController().CreateVFX((int)EffectTagList.CharacterEffectTag.Damage,attacker.transform.position,1f,Quaternion.identity);
                attacker = null;
                controller.DamageTag = CharacterTagList.DamageTag.Null;
                break;
        }
        if(controller.HP > 0) { return; }
        controller.Death();
    }
}
