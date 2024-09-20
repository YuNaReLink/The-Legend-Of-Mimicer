using UnityEngine;

/// <summary>
/// クモの攻撃用のコライダーの処理を行うクラス
/// </summary>
public class SpiderAttackController : BaseAttackController
{
    public override ToolTag GetToolTag(){ return ToolTag.Other;}

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
        Animator anim = controller.GetAnimator();
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (animInfo.IsName("attack"))
        {
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
