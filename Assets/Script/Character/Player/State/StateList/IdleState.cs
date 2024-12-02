
public class IdleState : InterfaceState
{
    private PlayerController controller = null;
    public IdleState(PlayerController _controller)
    {
        controller = _controller;
    }
    public void DoUpdate()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.GetUp:
            case CharacterTagList.StateTag.Grab:
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
            case CharacterTagList.StateTag.ChangeMode:
                if (!CameraController.Instance.IsFPSMode())
                {
                    return;
                }
                break;
        }
        PlayerInput input = controller.GetKeyInput();
        bool stopstate = controller.CharacterStatus.CurrentState == CharacterTagList.StateTag.Idle && input.GuardHoldButton;
        if (stopstate) { return; }
        if (!controller.CharacterStatus.Landing) { return; }
        bool horidleNoEnabled = input.Horizontal >= 1 || input.Horizontal <= -1;
        bool verIdleNoEnabled = input.Vertical >= 1 || input.Vertical <= -1;
        if (horidleNoEnabled || verIdleNoEnabled) { return; }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Idle);
    }
}
