
/// <summary>
/// ボスの攻撃の処理(コライダー)を行うクラス
/// </summary>
public class BossFootDamageController : BaseAttackController
{
    public override ToolTag GetToolTag() { return ToolTag.Other; }
    private void Start()
    {
        collider.enabled = false;
    }

    void Update()
    {
        SetTrigger();
    }

    private bool CheckAttackState()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
                return true;
        }
        return false;
    }

    private void SetTrigger()
    {
        if (controller == null) { return; }
        if (collider == null) { return; }
        if (CheckAttackState())
        {
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }
    }
}
