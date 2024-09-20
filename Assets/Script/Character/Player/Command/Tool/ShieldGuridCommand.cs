

public class ShieldGuridCommand : InterfaceBaseToolCommand
{
    private PlayerController    controller = null;

    public ShieldGuridCommand(PlayerController _controller)
    {
        controller = _controller;
    }

    private bool CheckStopState()
    {
        CharacterTagList.StateTag state = controller.CharacterStatus.CurrentState;
        switch (state)
        {
            case CharacterTagList.StateTag.Rolling:
            case CharacterTagList.StateTag.Jump:
            case CharacterTagList.StateTag.WallJump:
            case CharacterTagList.StateTag.Grab:
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.JumpAttack:
            case CharacterTagList.StateTag.ReadySpinAttack:
            case CharacterTagList.StateTag.SpinAttack:
                return true;
        }
        return false;
    }

    public void Input()
    {
        //条件にある状態だったら早期リターン
        if (CheckStopState()) { return; }
        //着地してなかったら早期リターン
        if (!controller.CharacterStatus.Landing) { return; }
        //ガードボタンを押してる時
        if (controller.GetKeyInput().GuardHoldButton)
        {
            //押した瞬間だけ効果音を再生
            if (controller.GetKeyInput().GuardPushButton && controller.CharacterStatus.GuardState == CharacterTagList.GuardState.Null)
            {
                controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.ShildPosture);
            }
            //カメラが注目中だったら
            //ガード状態のタグを変更
            if (controller.GetKeyInput().IsCameraLockEnabled())
            {
                controller.CharacterStatus.GuardState = CharacterTagList.GuardState.Normal;
            }
            else
            {
                controller.CharacterStatus.GuardState = CharacterTagList.GuardState.Crouch;
            }
        }
        else
        {
            //ガード状態のタグを変更
            controller.CharacterStatus.GuardState = CharacterTagList.GuardState.Null;
        }
        GuardStateInput();
    }

    private void GuardStateInput()
    {
        if(controller.CharacterStatus.GuardState != CharacterTagList.GuardState.Crouch){return;}
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Gurid);
    }

    public void Execute()
    {
        //状態によって左腕のモーションを変更する(現在は盾を持つモーションだけ)
        ArmMotionCommand();
    }

    private void ArmMotionCommand()
    {
        //状態によって左腕のモーションを変更する(現在は盾を持つモーションだけ)
        if (controller.CharacterStatus.GuardState == CharacterTagList.GuardState.Normal)
        {
            controller.GetMotion().Change(controller.GetClip());
        }
        else
        {
            controller.GetMotion().Change(controller.GetNullClip());
        }
    }
}
