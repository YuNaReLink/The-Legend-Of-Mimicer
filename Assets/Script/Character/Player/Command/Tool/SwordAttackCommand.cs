using UnityEngine;

public class SwordAttackCommand : InterfaceBaseToolCommand
{
    private PlayerController controller = null;

    public SwordAttackCommand(PlayerController _controller)
    {
        controller = _controller;
    }

    //PlayerInput�ŋL�q���Ă���e
    public void Input()
    {
        //�O�e�U���̓���
        ThreeAttackInput();
        //�W�����v�U���̓���
        JumpAttackInput();
        //��]�U���̓���
        SpinAttackInput();
    }

    private const float MaxMotionNormalizedTimeOfAttack = 0.4f;
    private const float MaxMotionNormalizedTimeOfAttack3 = 0.6f;

    /// <summary>
    /// �O�i�U���̏���
    /// ThreeAttackCount�̐��l�̏ڍ�
    /// 0:1����
    /// 1:2����
    /// 2:3����
    /// </summary>
    private void ThreeAttackInput()
    {
        //�w�肵����Ԃ���Ȃ����
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Rolling:
            case CharacterTagList.StateTag.ReadySpinAttack:
            case CharacterTagList.StateTag.SpinAttack:
            case CharacterTagList.StateTag.JumpAttack:
            case CharacterTagList.StateTag.Damage:
                return;
        }
        //���n����
        if (!controller.CharacterStatus.Landing) { return; }
        //�L�[����
        if (!controller.GetKeyInput().AttackButton) { return; }
        //���N���b�N���������ŏ��͒������t���O��false��
        controller.GetKeyInput().AttackHoldButton = false;
        controller.BattleMode = true;
        AnimatorStateInfo animInfo = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
        switch (controller.GetKeyInput().ThreeAttackCount)
        {
            //��i��
            case (int)CharacterTagList.TripleAttack.First:
                if (controller.GetToolController().IsCurrentToolChange)
                {
                    controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.FirstAttack);
                }
                if (animInfo.IsName("attack3"))
                {
                    if (animInfo.normalizedTime < MaxMotionNormalizedTimeOfAttack3) { return; }
                    AttackDetailHandle();
                }
                else
                {
                    AttackDetailHandle();
                }
                break;
            //��i��
            case (int)CharacterTagList.TripleAttack.Second:
                if (!animInfo.IsName("attack1")) { return; }
                if (animInfo.normalizedTime < MaxMotionNormalizedTimeOfAttack) { return; }
                AttackDetailHandle();
                break;
            //�O�i��
            case (int)CharacterTagList.TripleAttack.Third:
                if (!animInfo.IsName("attack2")) { return; }
                if (animInfo.normalizedTime < MaxMotionNormalizedTimeOfAttack) { return; }
                AttackDetailHandle();
                break;
        }
    }
    private void AttackDetailHandle()
    {
        //�O�i�U���J�E���genum�ɑ��
        controller.TripleAttack = (CharacterTagList.TripleAttack)controller.GetKeyInput().ThreeAttackCount;
        //�O�i�ڍU�����[�V�����Đ�
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Attack);
        //�O�i�U���J�E���g�����Z
        controller.GetKeyInput().ThreeAttackCount++;
        //�O�i�ڂȂ�J�E���g��0�Ƀ��Z�b�g
        if(controller.GetKeyInput().ThreeAttackCount >= (int)CharacterTagList.TripleAttack.DataEnd)
        {
            controller.GetKeyInput().ThreeAttackCount = 0;
        }
    }
    private bool CheckStopState()
    {
        CharacterTagList.StateTag state = controller.CharacterStatus.CurrentState;
        switch (state)
        {
            case CharacterTagList.StateTag.Rolling:
            case CharacterTagList.StateTag.Damage:
            case CharacterTagList.StateTag.Null:
            case CharacterTagList.StateTag.JumpAttack:
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.Grab:
            case CharacterTagList.StateTag.ClimbWall:
            case CharacterTagList.StateTag.WallJump:
                return true;
        }
        return false;
    }

    private const float JumpAttackAcceleCount = 0.5f;

    public void JumpAttackInput()
    {
        //�w�肵�����[�V������������
        if (CheckStopState()) { return; }
        //�ړ��L�[���������͂���Ă��Ȃ�������
        if(controller.GetKeyInput().Horizontal != 0&& controller.GetKeyInput().Vertical == 0) { return; }
        //�J�������ڒ��̃W�����v�a�����
        if (controller.GetKeyInput().ActionButton&& controller.GetKeyInput().IsCameraLockEnabled())
        {
            controller.GetTimer().GetTimerJumpAttackAccele().StartTimer(JumpAttackAcceleCount);
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.JumpAttack);
            controller.GetKeyInput().ActionButton = false;
        }
        //�󒆂ɂ��鎞�̃W�����v�a�����
        else if (controller.GetKeyInput().AttackButton && !controller.CharacterStatus.Landing)
        {
            controller.GetTimer().GetTimerJumpAttackAccele().StartTimer(JumpAttackAcceleCount);
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.JumpAttack);
            controller.GetKeyInput().AttackButton = false;
            controller.BattleMode = true;
        }
    }

    private const float MotionNormalizedTimeOfAttack1 = 0.7f;

    private void SpinAttackInput()
    {
        //�����n����
        if (!controller.CharacterStatus.Landing) { return; }
        //�O�i�U������i�ڈȏ�Ȃ�
        if(controller.GetKeyInput().ThreeAttackCount >= (int)CharacterTagList.TripleAttack.Second) { return; }
        //��]�U����������J�n�t���O
        bool readystartflag = controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("attack1")&&
            controller.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= MotionNormalizedTimeOfAttack1;
        //��]�U������J�n�t���O
        bool spinflag = controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("readySpinAttack");
        //��]�U����������
        if (controller.GetKeyInput().AttackHoldButton&&readystartflag)
        {
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.ReadySpinAttack);
        }
        //��]�U������
        else if (!controller.GetKeyInput().AttackHoldButton&&spinflag)
        {
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.SpinAttack);
        }
    }

    /// <summary>
    /// PlayerController�ŋL�q���Ă���e
    /// </summary>
    public void Execute()
    {
        //�ȉ��̏�Ԃ��Ƒ������^�[��
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.JumpAttack:
            case CharacterTagList.StateTag.SpinAttack:
                break;
            default:
                return;
        }
        //���v���C���[�I�u�W�F�N�g���U�����ɓ����������s���Ă���
        //�O�i�U������
        ThreeAttackCommand();
        //�W�����v�U��
        JumpAttackCommand();
        //��]�U��
        SpinAttackCommand();
    }

    private void ThreeAttackCommand()
    {
        if (!controller.CharacterStatus.Landing) { return; }
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Rolling:
            case CharacterTagList.StateTag.Damage:
                return;
        }

        switch (controller.TripleAttack)
        {
            case CharacterTagList.TripleAttack.Third:
                ThreeAttack();
                break;
        }
    }

    private const float MaxThreeAttackMotionNormalizedTime = 0.7f;

    private void ThreeAttack()
    {
        AnimatorStateInfo info = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
        if(info.normalizedTime > MaxThreeAttackMotionNormalizedTime) { return; }
        controller.ForwardAccele(controller.GetData().ThirdAttackForwardPower);
    }

    private void JumpAttackCommand()
    {
        if (!controller.GetTimer().GetTimerJumpAttackAccele().IsEnabled()) { return; }
        if (controller.CharacterStatus.Landing && !controller.CharacterStatus.Jumping)
        {
            controller.JumpForce(controller.GetData().JumpPowerOfJumpAttack);
            controller.CharacterStatus.Jumping = true;
        }
        controller.ForwardAccele(controller.GetData().ForwardPowerOfJumpAttack);
    }

    private const float MotionNormalizedTimeOfSpinAttack = 0.3f;

    private void SpinAttackCommand()
    {
        if(controller.CharacterStatus.CurrentState != CharacterTagList.StateTag.SpinAttack) {  return; }
        AnimatorStateInfo info = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
        if(info.normalizedTime > MotionNormalizedTimeOfSpinAttack) { return; }
        controller.ForwardAccele(controller.GetData().ForwardPowerOfSpinAttack);
    }
}
