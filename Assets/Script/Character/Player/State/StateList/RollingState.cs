
public class RollingState : InterfaceState
{
    private PlayerController controller = null;
    public RollingState(PlayerController _controller)
    {
        controller = _controller;
    }
    public void DoUpdate()
    {
        if (CameraController.Instance.IsFPSMode()) { return; }
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Rolling:
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.JumpAttack:
            case CharacterTagList.StateTag.ReadySpinAttack:
            case CharacterTagList.StateTag.SpinAttack:
            case CharacterTagList.StateTag.GetUp:
            case CharacterTagList.StateTag.Damage:
                return;
            case CharacterTagList.StateTag.Push:
                if(controller.PushTag != CharacterTagList.PushTag.Null)
                {
                    return;
                }
                break;
        }
        if (!controller.CharacterStatus.Landing) { return; }
        if (controller.GetMotion().IsEndRollingMotionNameCheck()) { return; }
        if (!controller.GetKeyInput().ActionButton) { return; }
        if (!controller.GetKeyInput().GuardHoldButton)
        {
            //完全に止まっていたらリターン
            if (controller.GetKeyInput().Vertical == 0 && controller.GetKeyInput().Horizontal == 0) { return; }
            controller.GetKeyInput().CurrentDirection = CharacterTagList.DirectionTag.Up;
            //ローリングの移動を行うための初速度を代入
            controller.GetKeyInput().InitVelocity = controller.CharacterRB.velocity;
            controller.GetKeyInput().RollTimer = 0.0f;
            controller.GetTimer().GetTimerNoAccele().StartTimer(0.4f);
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Rolling);
            //Shiftキーを無効にする
            controller.GetKeyInput().ActionButton = false;
        }
        else
        {
            if (controller.BattleMode && (controller.GetKeyInput().Vertical > 0 || controller.GetKeyInput().Vertical == 0 && controller.GetKeyInput().Horizontal == 0)) { return; }
            if (controller.GetKeyInput().Vertical > 0 || controller.GetKeyInput().Vertical == 0 && controller.GetKeyInput().Horizontal == 0)
            {
                controller.GetKeyInput().CurrentDirection = CharacterTagList.DirectionTag.Up;
            }
            DirectionRollingInput();
        }
    }
    private void DirectionRollingInput()
    {
        float noaccele = 0.4f;
        if (controller.GetKeyInput().Horizontal > 0)
        {
            controller.GetKeyInput().CurrentDirection = CharacterTagList.DirectionTag.Right;
        }
        if (controller.GetKeyInput().Horizontal < 0)
        {
            controller.GetKeyInput().CurrentDirection = CharacterTagList.DirectionTag.Left;
        }
        if (controller.GetKeyInput().Vertical < 0 && controller.GetKeyInput().Horizontal == 0)
        {
            noaccele = 0.5f;
            controller.GetKeyInput().CurrentDirection = CharacterTagList.DirectionTag.Down;
        }
        controller.GetKeyInput().InitVelocity = controller.CharacterRB.velocity;
        controller.GetKeyInput().RollTimer = 0.0f;
        controller.GetTimer().GetTimerNoAccele().StartTimer(noaccele);
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Rolling);
        //Shiftキーを無効にする
        controller.GetKeyInput().ActionButton = false;
    }
}
