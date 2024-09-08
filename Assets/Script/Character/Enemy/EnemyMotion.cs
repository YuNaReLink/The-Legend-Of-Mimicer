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
        if (tag == controller.CharacterStatus.CurrentState) { return; }
        controller.CharacterStatus.PastState = controller.CharacterStatus.CurrentState;
        controller.CharacterStatus.CurrentState = tag;
        Animator anim = controller.GetAnimator();
        anim.SetInteger(stateName, (int)tag);
    }
}
