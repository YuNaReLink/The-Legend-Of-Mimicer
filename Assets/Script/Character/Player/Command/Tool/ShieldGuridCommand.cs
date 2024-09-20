

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
        //�����ɂ����Ԃ������瑁�����^�[��
        if (CheckStopState()) { return; }
        //���n���ĂȂ������瑁�����^�[��
        if (!controller.CharacterStatus.Landing) { return; }
        //�K�[�h�{�^���������Ă鎞
        if (controller.GetKeyInput().GuardHoldButton)
        {
            //�������u�Ԃ������ʉ����Đ�
            if (controller.GetKeyInput().GuardPushButton && controller.CharacterStatus.GuardState == CharacterTagList.GuardState.Null)
            {
                controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.ShildPosture);
            }
            //�J���������ڒ���������
            //�K�[�h��Ԃ̃^�O��ύX
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
            //�K�[�h��Ԃ̃^�O��ύX
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
        //��Ԃɂ���č��r�̃��[�V������ύX����(���݂͏��������[�V��������)
        ArmMotionCommand();
    }

    private void ArmMotionCommand()
    {
        //��Ԃɂ���č��r�̃��[�V������ύX����(���݂͏��������[�V��������)
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
