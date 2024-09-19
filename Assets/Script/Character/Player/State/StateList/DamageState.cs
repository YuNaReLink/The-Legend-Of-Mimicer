
/// <summary>
/// �_���[�W�̏�Ԃ���������N���X
/// </summary>
public class DamageState : InterfaceState
{
    private PlayerController controller = null;
    public DamageState(PlayerController _controller)
    {
        controller = _controller;
    }

    /// <summary>
    /// �_���[�W���ɔ��������Ԃƃ��[�V������K�p����
    /// </summary>
    public void DoUpdate()
    {
        if (controller.DamageTag == CharacterTagList.DamageTag.Null) { return; }
        switch (controller.DamageTag)
        {
            case CharacterTagList.DamageTag.Fall:
                controller.GetMotion().ForcedChangeMotion(CharacterTagList.StateTag.Damage);
                controller.GetCommands().GetFallDistanceCheck().SetFallDamage(false);
                break;
            case CharacterTagList.DamageTag.NormalAttack:
                controller.GetMotion().ForcedChangeMotion(CharacterTagList.StateTag.Damage);
                break;
        }
    }
}
