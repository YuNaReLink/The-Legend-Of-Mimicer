using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotion
{
    [SerializeField]
    private EnemyController controller = null;

    private string stateName = "State";
    private string guardName = "Guard";
    public EnemyMotion(EnemyController _controller)
    {
        controller = _controller;
    }

    public void ChangeMotion(CharacterTag.StateTag tag)
    {
        if (tag == controller.CurrentState) { return; }
        controller.PastState = controller.CurrentState;
        controller.CurrentState = tag;
        Animator anim = controller.GetAnimator();
        if(anim.speed == 0)
        {
            anim.speed = 1;
        }
        anim.SetInteger(stateName, (int)tag);
    }

    public void ChangeGuardBool(bool f)
    {
        Animator anim = controller.GetAnimator();
        int num = (int)controller.GuardState;
        bool enabled = IntToBool(num);
        anim.SetBool(guardName, enabled);
    }

    public void StopMotion()
    {
        if(controller.CurrentState == CharacterTag.StateTag.Null) { return; }
        Animator anim = controller.GetAnimator();
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        switch (controller.CurrentState)
        {
            case CharacterTag.StateTag.Attack:
                if (animInfo.IsName("stampAttack")&&animInfo.normalizedTime >= 0.7f)
                {
                    if (controller.GetTimer().GetTimerAttackCoolDown().IsEnabled())
                    {
                        anim.speed = 0;
                    }
                    else
                    {
                        anim.speed = 1;
                    }
                }
                break;
            case CharacterTag.StateTag.Damage:
                if (animInfo.IsName("stun") && animInfo.normalizedTime >= 0.8f)
                {
                    if (controller.GetTimer().GetTimerStun().IsEnabled())
                    {
                        anim.speed = 0;
                    }
                    else
                    {
                        anim.speed = 1;
                    }
                }
                break;
        }
    }

    public void EndMotion()
    {
        Animator anim = controller.GetAnimator();
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime < 1.0f) { return; }
        BossMotionName motionName = new BossMotionName();

        foreach (string motion in motionName.GetMotionName())
        {
            if (animInfo.IsName(motion))
            {
                EndMotionCommand(motion);
                return;
            }
        }
    }

    public void EndMotionCommand(string motion)
    {
        Animator anim = controller.GetAnimator();
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        switch (motion)
        {
            case "Idle":
                break;
            case "walk":
                break;
            case "stampAttack":
                controller.CurrentState = CharacterTag.StateTag.Null;
                break;
            case "guard":
                controller.CurrentState = CharacterTag.StateTag.Null;
                break;
            case "stun":
                controller.CurrentState = CharacterTag.StateTag.Null;
                break;
            case "returnUp":
                controller.CurrentState = CharacterTag.StateTag.Null;
                break;
        }
    }


    private bool IntToBool(int number)
    {
        return number != 0;
    }
}
