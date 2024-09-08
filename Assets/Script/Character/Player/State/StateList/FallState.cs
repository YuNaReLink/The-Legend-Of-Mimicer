

public class FallState : InterfaceState
{
    private PlayerController controller = null;
    public FallState(PlayerController _controller)
    {
        controller = _controller;
    }

    public void DoUpdate()
    {
        if(controller.GetCameraController().IsFPSMode()){ return; }
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Jump:
            case CharacterTagList.StateTag.JumpAttack:
            case CharacterTagList.StateTag.Rolling:
            case CharacterTagList.StateTag.WallJump:
            case CharacterTagList.StateTag.Grab:
            case CharacterTagList.StateTag.Null:
                return;
        }
        if (controller.GetObstacleCheck().IsGrabFlag()) { return; }
        if (controller.GetObstacleCheck().IsClimbFlag()) { return; }
        if (controller.GetObstacleCheck().IsWallJumpFlag()) { return; }
        if (!controller.CharacterStatus.Landing)
        {
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Fall);
        }
    }
}
