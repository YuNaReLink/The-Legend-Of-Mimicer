

public class MoveState : InterfaceState
{
    private PlayerController controller = null;
    private PlayerInput playerInput = null;
    public MoveState(PlayerController _controller)
    {
        controller = _controller;
        playerInput = controller.GetKeyInput();
    }
    public void DoUpdate()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.GetUp:
            case CharacterTagList.StateTag.Grab:
            case CharacterTagList.StateTag.WallJump:
            case CharacterTagList.StateTag.ClimbWall:
            case CharacterTagList.StateTag.Rolling:
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.JumpAttack:
            case CharacterTagList.StateTag.ReadySpinAttack:
            case CharacterTagList.StateTag.SpinAttack:
            case CharacterTagList.StateTag.Damage:
                return;
            case CharacterTagList.StateTag.Push:
                if (controller.PushTag != CharacterTagList.PushTag.Null)
                {
                    return;
                }
                break;
        }
        if (!controller.CharacterStatus.Landing){return;}
        if (playerInput.Horizontal == 0&&
            playerInput.Vertical   == 0) { return; }
        if (!playerInput.GuardHoldButton)
        {
            playerInput.CurrentDirection = CharacterTagList.DirectionTag.Null;
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Run);
        }
        else
        {
            DirectionRunInput();
        }
    }

    private void DirectionRunInput()
    {
        if (playerInput.Horizontal > 0)
        {
            playerInput.CurrentDirection = CharacterTagList.DirectionTag.Right;
        }
        if (playerInput.Horizontal < 0)
        {
            playerInput.CurrentDirection = CharacterTagList.DirectionTag.Left;
        }
        if (playerInput.Vertical > 0)
        {
            playerInput.CurrentDirection = CharacterTagList.DirectionTag.Up;
        }
        if (playerInput.Vertical < 0)
        {
            playerInput.CurrentDirection = CharacterTagList.DirectionTag.Down;
        }
        controller.CharacterStatus.MoveInput = true;
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Run);
    }
}
