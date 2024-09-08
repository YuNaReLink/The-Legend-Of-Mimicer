using UnityEngine;

public class SpiderMotionController : MotionController
{
    private SpiderController controller = null;
    public SpiderMotionController(SpiderController _controller)
    {
        controller = _controller;
    }
    public override void ChangeMotion(CharacterTagList.StateTag tag)
    {
        if (tag == controller.CharacterStatus.CurrentState) { return; }
        controller.CharacterStatus.PastState = controller.CharacterStatus.CurrentState;
        controller.CharacterStatus.CurrentState = tag;
        Animator anim = controller.GetAnimator();
        anim.SetInteger(stateName, (int)tag);
    }

    public override void EndMotionNameCheck()
    {
        Animator anim = controller.GetAnimator();
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime <= 1.0f) { return; }
        SpiderMotionNameList motionName = new SpiderMotionNameList();

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
            case "attack":
                controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Idle);
                controller.GetNavMeshController().PositionReset();
                controller.CharacterRB.velocity = Vector3.zero;
                controller.CharacterStatus.CurrentState = CharacterTagList.StateTag.Null;
                break;
        }
    }
}
