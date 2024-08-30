

public class ShieldGuridCommand : InterfaceBaseToolCommand
{
    private PlayerController    controller = null;

    public ShieldGuridCommand(PlayerController _controller)
    {
        controller = _controller;
    }
    public ToolTag GetToolTag()
    {
        return ToolTag.Shield;
    }

    private bool CheckStopState()
    {
        CharacterTagList.StateTag state = controller.CurrentState;
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
        if (!controller.Landing) { return; }
        //�K�[�h�{�^���������Ă鎞
        if (controller.GetKeyInput().GuardHoldButton)
        {
            //�������u�Ԃ������ʉ����Đ�
            if (controller.GetKeyInput().GuardPushButton && controller.GuardState == CharacterTagList.GuardState.Null)
            {
                controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.ShildPosture);
            }
            //�J���������ڒ���������
            //�K�[�h��Ԃ̃^�O��ύX
            if (controller.GetKeyInput().IsCameraLockEnabled())
            {
                controller.GuardState = CharacterTagList.GuardState.Normal;
            }
            else
            {
                controller.GuardState = CharacterTagList.GuardState.Crouch;
            }
        }
        else
        {
            //�K�[�h��Ԃ̃^�O��ύX
            controller.GuardState = CharacterTagList.GuardState.Null;
        }
        GuardStateInput();
    }

    private void GuardStateInput()
    {
        if(controller.GuardState != CharacterTagList.GuardState.Crouch){return;}
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
        if (controller.GuardState == CharacterTagList.GuardState.Normal)
        {
            controller.GetMotion().Change(controller.GetClip());
        }
        else
        {
            controller.GetMotion().Change(controller.GetNullClip());
        }
    }
}
