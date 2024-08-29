using UnityEngine;

public class EnemyMotion : MotionController
{
    protected EnemyController controller = null;

    public EnemyMotion(EnemyController _controller)
    {
        controller = _controller;
    }

    public overrideÅ@void ChangeMotion(CharacterTagList.StateTag tag)
    {
        if (tag == controller.CurrentState) { return; }
        controller.PastState = controller.CurrentState;
        controller.CurrentState = tag;
        Animator anim = controller.GetAnimator();
        anim.SetInteger(stateName, (int)tag);
    }
}
