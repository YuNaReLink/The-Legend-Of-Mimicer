using UnityEngine;

public class RollingCommand : InterfaceBaseCommand
{
    private PlayerController controller = null;
    public RollingCommand(PlayerController _controller)
    {
        controller = _controller;
    }

    private const float MinRollProgress = 0.05f;

    private const float EndRollProgress = 1.0f;

    public void Execute()
    {
        PlayerTimers timer = controller.GetTimer();
        if (!timer.GetTimerNoAccele().IsEnabled()){ return; }
        controller.GetKeyInput().RollTimer += Time.deltaTime;
        float rollProgress = controller.GetKeyInput().RollTimer / timer.GetTimerNoAccele().GetInitCount();
        if(rollProgress <= MinRollProgress)
        {
            controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Rolling);
        }
        //���[�����O���I������烍�[�����O�J�n���̏����x���v���C���[�ɕԂ�
        else if(rollProgress > EndRollProgress)
        {
            //���[�����O���I������珉���x����
            controller.CharacterRB.velocity = controller.GetKeyInput().InitVelocity;
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Run);
        }
        //���[�����O���R�W�����v���������s���ꂽ��
        else if (controller.GetObstacleCheck().CliffJumpFlag)
        {
            controller.CharacterRB.velocity = controller.GetKeyInput().InitVelocity;
            controller.GetTimer().GetTimerNoAccele().End();
        }
        //���[�����O��XY�����̉���
        else
        {
            // �A�j���[�V�����J�[�u���g�p���đ��x�𒲐�
            Vector3 rollDirection = controller.GetKeyInput().InitVelocity.normalized;
            switch (controller.GetKeyInput().CurrentDirection)
            {
                case CharacterTagList.DirectionTag.Up:
                    rollDirection = controller.transform.forward;
                    break;
                case CharacterTagList.DirectionTag.Down:
                    rollDirection = -controller.transform.forward;
                    break;
                case CharacterTagList.DirectionTag.Left:
                    rollDirection = -controller.transform.right;
                    break;
                case CharacterTagList.DirectionTag.Right:
                    rollDirection = controller.transform.right;
                    break;
            }


            float currentRollSpeed = controller.GetRollCurve().Evaluate(rollProgress) * SetRollingAcceleration();
            RollingAccele(rollDirection * currentRollSpeed);
        }
        //�o�N�]���̃W�����v����
        if(controller.GetKeyInput().CurrentDirection == CharacterTagList.DirectionTag.Down)
        {
            AnimatorStateInfo animInfo = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
            if (!animInfo.IsName("backFlip")) { return; }
            if(animInfo.normalizedTime <= 0.0f) { return; }
            if (controller.CharacterStatus.Landing&&!controller.CharacterStatus.Jumping)
            {
                RollingJump(controller.GetData().RollingJumpPower);
                controller.CharacterStatus.Jumping = true;
            }
        }
    }

    private float SetRollingAcceleration()
    {
        float accele = 0.0f;
        PlayerInput keyInput = controller.GetKeyInput();
        switch (controller.GetKeyInput().CurrentDirection)
        {
            case CharacterTagList.DirectionTag.Up:
                if (controller.GetKeyInput().CurrentDirection == CharacterTagList.DirectionTag.Up &&
                controller.GetKeyInput().Vertical == 0)
                {
                    accele = controller.GetData().RollingUPStaticAcceleration;
                }
                else
                {
                    accele = controller.GetData().RollingUPDynamicAcceleration;
                }
                break;
            case CharacterTagList.DirectionTag.Down:
                accele = controller.GetData().RollingDOWNAcceleration;
                break;
            case CharacterTagList.DirectionTag.Left:
                accele = controller.GetData().RollingLEFTAcceleration;
                break;
            case CharacterTagList.DirectionTag.Right:
                accele = controller.GetData().RollingRIGHTAcceleration;
                break;
        }
        return accele;
    }

    private void RollingAccele(Vector3 force)
    {
        controller.CharacterStatus.Velocity += force;
    }

    private void RollingJump(float force)
    {
        controller.CharacterRB.AddForce(controller.transform.up * force, ForceMode.Impulse);
    }

}
