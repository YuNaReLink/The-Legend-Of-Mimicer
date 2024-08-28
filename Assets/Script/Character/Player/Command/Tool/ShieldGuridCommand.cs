using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterTag;

public class ShieldGuridCommand : BaseToolAction
{
    private PlayerController controller = null;

    private BoxCollider collider = null;

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
        StateTag state = controller.CurrentState;
        switch (state)
        {
            case StateTag.Rolling:
            case StateTag.Jump:
            case StateTag.WallJump:
            case StateTag.Grab:
            case StateTag.Attack:
            case StateTag.JumpAttack:
            case StateTag.ReadySpinAttack:
            case StateTag.SpinAttack:
                return true;
        }
        return false;
    }

    public void Input()
    {
        if (CheckStopState()) { return; }
        if (controller.Landing)
        {
            if (controller.GetKeyInput().IsCameraLockEnabled())
            {
                if (controller.GetKeyInput().GuardHoldButton)
                {
                    if (controller.GetKeyInput().GuardPushButton && controller.GuardState == GuardState.Null)
                    {
                        controller.GetSoundController().PlaySESound((int)PlayerSoundController.PlayerSoundTag.ShildPosture);
                    }
                    controller.GuardState = GuardState.Normal;
                }
                else
                {
                    controller.GuardState = GuardState.Null;
                }
            }
            else
            {
                if (controller.GetKeyInput().GuardHoldButton)
                {
                    if (controller.GetKeyInput().GuardPushButton && controller.GuardState == GuardState.Null)
                    {
                        controller.GetSoundController().PlaySESound((int)PlayerSoundController.PlayerSoundTag.ShildPosture);
                    }
                    controller.GuardState = GuardState.Crouch;
                }
                else
                {
                    controller.GuardState = GuardState.Null;
                }
            }
        }
        else
        {
            if (controller.GetKeyInput().GuardPushButton && controller.GuardState == GuardState.Null)
            {
                controller.GetSoundController().PlaySESound((int)PlayerSoundController.PlayerSoundTag.ShildPosture);
            }
            controller.GuardState = GuardState.Normal;
        }
        GuardStateInput();
    }

    private void GuardStateInput()
    {
        switch (controller.GuardState)
        {
            case GuardState.Crouch:
                controller.GetMotion().ChangeMotion(StateTag.Gurid);
                break;
        }
    }

    public void Execute()
    {
        //状態によって左腕のモーションを変更する(現在は盾を持つモーションだけ)
        ArmMotionCommand();
    }

    private void ArmMotionCommand()
    {
        //状態によって左腕のモーションを変更する(現在は盾を持つモーションだけ)
        if (controller.GuardState == GuardState.Normal)
        {
            controller.GetMotion().Change(controller.GetClip());
        }
        else
        {
            controller.GetMotion().Change(controller.GetNullClip());
        }
    }
}
