using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterTag;

public class SwordAttackCommand : BaseToolCommand
{
    private PlayerController controller = null;

    Collider collider = null;
    public SwordAttackCommand(PlayerController _controller)
    {
        controller = _controller;
        collider = controller?.GetToolController().GetInventoryData().ToolItemList[(int)ToolInventoryController.ToolObjectTag.Sword].GetComponent<BoxCollider>();
    }

    public ToolTag GetToolTag()
    {
        return ToolTag.Sword;
    }
    //PlayerInput�ŋL�q���Ă���e
    public void Input()
    {
        ThreeAttackInput();

        JumpAttackInput();

        SpinAttackInput();
    }

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
        bool stopstate = controller.CurrentState == StateTag.Rolling || controller.CurrentState == StateTag.Damage;
        if (stopstate) { return; }
        //���n����
        if (!controller.Landing) { return; }
        //�L�[����
        if (!controller.GetKeyInput().AttackButton) { return; }
        //���N���b�N���������ŏ��͒������t���O��false��
        controller.GetKeyInput().AttackHoldButton = false;
        controller.BattleMode = true;
        AnimatorStateInfo animInfo = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
        switch (controller.GetKeyInput().ThreeAttackCount)
        {
            //��i��
            case 0:
                if (animInfo.IsName("attack3"))
                {
                    if (animInfo.normalizedTime < 0.6f) { return; }
                    AttackDetailHandle();
                }
                else
                {
                    AttackDetailHandle();
                }
                break;
            //��i��
            case 1:
                if (!animInfo.IsName("attack1")) { return; }
                if (animInfo.normalizedTime < 0.4f) { return; }
                AttackDetailHandle();
                break;
            //�O�i��
            case 2:
                if (!animInfo.IsName("attack2")) { return; }
                if (animInfo.normalizedTime < 0.4f) { return; }
                AttackDetailHandle();
                break;
        }
    }
    private void AttackDetailHandle()
    {
        //�O�i�U���J�E���genum�ɑ��
        controller.TripleAttack = (TripleAttack)controller.GetKeyInput().ThreeAttackCount;
        //�O�i�ڍU�����[�V�����Đ�
        controller.GetMotion().ChangeMotion(StateTag.Attack);
        //�O�i�U���J�E���g�����Z
        controller.GetKeyInput().ThreeAttackCount++;
        //�O�i�ڂȂ�J�E���g��0�Ƀ��Z�b�g
        if(controller.GetKeyInput().ThreeAttackCount >= (int)TripleAttack.DataEnd)
        {
            controller.GetKeyInput().ThreeAttackCount = 0;
        }
    }
    private bool CheckStopState()
    {
        StateTag state = controller.CurrentState;
        switch (state)
        {
            case StateTag.Rolling:
            case StateTag.Damage:
            case StateTag.Null:
            case StateTag.JumpAttack:
            case StateTag.Attack:
            case StateTag.Grab:
            case StateTag.ClimbWall:
            case StateTag.WallJump:
                return true;
        }
        return false;
    }

    public void JumpAttackInput()
    {
        //�w�肵�����[�V������������
        if (CheckStopState()) { return; }
        //�ړ��L�[���������͂���Ă��Ȃ�������
        if(controller.GetKeyInput().Horizontal != 0&& controller.GetKeyInput().Vertical == 0) { return; }
        //�J�������ڒ��̃W�����v�a�����
        if (controller.GetKeyInput().ActionButton&& controller.GetKeyInput().IsCameraLockEnabled())
        {
            controller.GetTimer().GetTimerJumpAttackAccele().StartTimer(0.5f);
            controller.GetMotion().ChangeMotion(StateTag.JumpAttack);
            controller.GetKeyInput().ActionButton = false;
        }
        //�󒆂ɂ��鎞�̃W�����v�a�����
        else if (controller.GetKeyInput().AttackButton && !controller.Landing)
        {
            controller.GetTimer().GetTimerJumpAttackAccele().StartTimer(0.5f);
            controller.GetMotion().ChangeMotion(StateTag.JumpAttack);
            controller.GetKeyInput().AttackButton = false;
            controller.BattleMode = true;
        }
    }

    private void SpinAttackInput()
    {
        //�����n����
        if (!controller.Landing) { return; }
        //�O�i�U������i�ڈȏ�Ȃ�
        if(controller.GetKeyInput().ThreeAttackCount >= (int)TripleAttack.Second) { return; }
        //��]�U����������J�n�t���O
        bool readystartflag = controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("attack1")&&
            controller.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f;
        //��]�U������J�n�t���O
        bool spinflag = controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("readySpinAttack");
        //��]�U����������
        if (controller.GetKeyInput().AttackHoldButton&&readystartflag)
        {
            controller.GetMotion().ChangeMotion(StateTag.ReadySpinAttack);
        }
        //��]�U������
        else if (!controller.GetKeyInput().AttackHoldButton&&spinflag)
        {
            controller.GetMotion().ChangeMotion(StateTag.SpinAttack);
        }
    }

    /// <summary>
    /// PlayerController�ŋL�q���Ă���e
    /// </summary>
    public void Execute()
    {
        bool attackflag = controller.CurrentState == StateTag.Attack || controller.CurrentState == StateTag.JumpAttack ||
            controller.CurrentState == StateTag.SpinAttack;
        if (!attackflag) {
            //collider.enabled = false;
            return; 
        }
        //�ʏ�̎O�i�U������
        ThreeAttackCommand();
        //�W�����v�U��
        JumpAttackCommand();
        //��]�U��
        SpinAttackCommand();
    }

    private void ThreeAttackCommand()
    {
        if (!controller.Landing) { return; }
        switch (controller.CurrentState)
        {
            case StateTag.Rolling:
            case StateTag.Damage:
                return;
        }

        switch (controller.TripleAttack)
        {
            case TripleAttack.Three:
                ThreeAttack();
                break;
        }
    }

    private void ThreeAttack()
    {
        AnimatorStateInfo info = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
        if(info.normalizedTime > 0.7f) { return; }
        controller.ForwardAccele(100f);
    }

    private void JumpAttackCommand()
    {
        if (!controller.GetTimer().GetTimerJumpAttackAccele().IsEnabled()) { return; }
        if (controller.Landing && !controller.Jumping)
        {
            controller.JumpForce(1250f);
            controller.Jumping = true;
        }
        controller.ForwardAccele(500f);
    }

    private void SpinAttackCommand()
    {
        if(controller.CurrentState != StateTag.SpinAttack) {  return; }
        AnimatorStateInfo info = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
        if(info.normalizedTime > 0.3f) { return; }
        controller.ForwardAccele(250f);
    }
}
