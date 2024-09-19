
/// <summary>
/// ダメージの状態を処理するクラス
/// </summary>
public class DamageState : InterfaceState
{
    private PlayerController controller = null;
    public DamageState(PlayerController _controller)
    {
        controller = _controller;
    }

    /// <summary>
    /// ダメージ時に発生する状態とモーションを適用する
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
