

public class FallState : InterfaceState
{
    private PlayerController controller = null;
    public FallState(PlayerController _controller)
    {
        controller = _controller;
    }

    public void DoUpdate()
    {
        bool noUpdateState =    controller.CharacterStatus.CurrentState == CharacterTagList.StateTag.Jump ||
                                controller.CharacterStatus.CurrentState == CharacterTagList.StateTag.JumpAttack ||
                                controller.CharacterStatus.CurrentState == CharacterTagList.StateTag.Rolling ||
                                controller.CharacterStatus.CurrentState == CharacterTagList.StateTag.WallJump ||
                                controller.CharacterStatus.CurrentState == CharacterTagList.StateTag.Grab ||
                                controller.CharacterStatus.CurrentState == CharacterTagList.StateTag.Null;
        if (noUpdateState) { return; }
        if(controller.GetCameraController().IsFPSMode()){ return; }
        if (controller.GetObstacleCheck().IsGrabFlag()) { return; }
        if (controller.GetObstacleCheck().IsClimbFlag()) { return; }
        if (controller.GetObstacleCheck().IsWallJumpFlag()) { return; }
        if (!controller.CharacterStatus.Landing)
        {
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Fall);
        }
    }
}
