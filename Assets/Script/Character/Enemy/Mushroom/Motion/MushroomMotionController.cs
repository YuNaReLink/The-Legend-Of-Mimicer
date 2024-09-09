using UnityEngine;

public class MushroomMotionNameList
{
    private string[] motionName = new string[]
        {
            "Mushroom_Attack01Smile",
            "Mushroom_GetHitSmile"
        };

    public string[] GetMotionName() { return motionName; }
}
public class MushroomMotionController : MotionController
{
    private MushroomController controller = null;
    public MushroomMotionController(MushroomController _controller)
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
        anim.SetBool(boolname, controller.BattleMode);
    }

    public override void EndMotionNameCheck()
    {
        Animator anim = controller.GetAnimator();
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime <= 1.0f) { return; }
        MushroomMotionNameList motionName = new MushroomMotionNameList();

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
            case "Mushroom_Attack01Smile":
            case "Mushroom_GetHitSmile":
                controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Idle);
                controller.CharacterRB.velocity = Vector3.zero;
                break;
        }
    }
}
