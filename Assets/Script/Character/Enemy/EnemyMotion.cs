using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotion : MotionController
{
    protected EnemyController controller = null;

    public EnemyMotion(EnemyController _controller)
    {
        controller = _controller;
    }

    public overrideÅ@void ChangeMotion(CharacterTag.StateTag tag)
    {
        if (tag == controller.CurrentState) { return; }
        controller.PastState = controller.CurrentState;
        controller.CurrentState = tag;
        Animator anim = controller.GetAnimator();
        anim.SetInteger(statename, (int)tag);
    }
}
