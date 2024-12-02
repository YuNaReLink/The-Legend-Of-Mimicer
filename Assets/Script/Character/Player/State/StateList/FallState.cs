
/// <summary>
/// —‰º‚Ìó‘Ô‘JˆÚ‚ğs‚¤ƒNƒ‰ƒX
/// </summary>
public class FallState : InterfaceState
{
    private PlayerController controller = null;
    public FallState(PlayerController _controller)
    {
        controller = _controller;
    }

    public void DoUpdate()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Jump:
            case CharacterTagList.StateTag.JumpAttack:
            case CharacterTagList.StateTag.Rolling:
            case CharacterTagList.StateTag.WallJump:
            case CharacterTagList.StateTag.Null:
                return;
            case CharacterTagList.StateTag.Grab:
                if (controller.GetObstacleCheck().GrabCancel)
                {
                    break;
                }
                return;
        }
        if (CameraController.Instance.IsFPSMode()){ return; }
        if (controller.CharacterStatus.Landing) { return; }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Fall);
    }
}
