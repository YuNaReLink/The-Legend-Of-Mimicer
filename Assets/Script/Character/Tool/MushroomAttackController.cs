using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAttackController : BaseAttackController
{
    public override ToolTag GetToolTag(){return ToolTag.Other;}


    private void Start()
    {
        collider.enabled = false;
    }

    private void Update()
    {
        SetTrigger();
    }

    private bool CheckAttackState()
    {
        Animator anim = controller.GetAnimator();
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (animInfo.IsName("Mushroom_Attack01Smile"))
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
