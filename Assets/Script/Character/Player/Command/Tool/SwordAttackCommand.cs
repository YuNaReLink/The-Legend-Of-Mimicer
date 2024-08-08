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
        collider = controller?.GetToolController().Tools[(int)PlayerToolController.ToolObjectTag.Sword].GetComponent<BoxCollider>();
    }

    public ToolTag GetToolTag()
    {
        return ToolTag.Sword;
    }
    //PlayerInputで記述してる内容
    public void Input()
    {
        ThreeAttackInput();

        JumpAttackInput();

        SpinAttackInput();
    }

    /// <summary>
    /// 三段攻撃の処理
    /// ThreeAttackCountの数値の詳細
    /// 0:1撃目
    /// 1:2撃目
    /// 2:3撃目
    /// </summary>
    private void ThreeAttackInput()
    {
        bool stopstate = controller.CurrentState == StateTag.Rolling || controller.CurrentState == StateTag.Damage;
        if (stopstate) { return; }
        if (!controller.Landing) { return; }
        if (!controller.GetKeyInput().LeftMouseDownClick) { return; }
        //左クリックを押した最初は長押しフラグをfalseに
        controller.GetKeyInput().LeftMouseClick = false;
        controller.BattleMode = true;
        if (controller.GetKeyInput().ThreeAttackCount >= (int)TripleAttack.DataEnd) { return; }
        AnimatorStateInfo animInfo = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
        switch (controller.GetKeyInput().ThreeAttackCount)
        {
            case 0:
                AttackDetailHandle();
                break;
            case 1:
                if (!animInfo.IsName("attack1")) { return; }
                if (animInfo.normalizedTime < 0.5f) { return; }
                AttackDetailHandle();
                break;
            case 2:
                if (!animInfo.IsName("attack2")) { return; }
                if (animInfo.normalizedTime < 0.5f) { return; }
                AttackDetailHandle();
                break;
        }
    }
    private void AttackDetailHandle()
    {
        controller.TripleAttack = (TripleAttack)controller.GetKeyInput().ThreeAttackCount;
        controller.GetKeyInput().GetMotion().ChangeMotion(StateTag.Attack);
        controller.GetKeyInput().ThreeAttackCount++;
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
                return true;
        }
        return false;
    }

    public void JumpAttackInput()
    {
        if (CheckStopState()) { return; }
        if(controller.GetKeyInput().Horizontal != 0&& controller.GetKeyInput().Vertical == 0) { return; }
        if (controller.GetKeyInput().ShiftKey&& controller.GetKeyInput().IsCKeyEnabled())
        {
            controller.GetTimer().GetTimerJumpAttackAccele().StartTimer(0.5f);
            controller.GetKeyInput().GetMotion().ChangeMotion(StateTag.JumpAttack);
            controller.GetKeyInput().ShiftKey = false;
        }
        else if (controller.GetKeyInput().LeftMouseDownClick && !controller.Landing)
        {
            controller.GetTimer().GetTimerJumpAttackAccele().StartTimer(0.5f);
            controller.GetKeyInput().GetMotion().ChangeMotion(StateTag.JumpAttack);
            controller.GetKeyInput().LeftMouseDownClick = false;
            controller.BattleMode = true;
        }
    }

    private void SpinAttackInput()
    {
        if (!controller.Landing) { return; }
        if(controller.GetKeyInput().ThreeAttackCount >= (int)TripleAttack.Second) { return; }
        bool readystartflag = controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("attack1")&&
            controller.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f;
        
        bool spinflag = controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("readySpinAttack");
        if (controller.GetKeyInput().LeftMouseClick&&readystartflag)
        {
            controller.GetKeyInput().GetMotion().ChangeMotion(StateTag.ReadySpinAttack);
        }
        else if (!controller.GetKeyInput().LeftMouseClick&&spinflag)
        {
            controller.GetKeyInput().GetMotion().ChangeMotion(StateTag.SpinAttack);
        }
    }

    /// <summary>
    /// PlayerControllerで記述してる内容
    /// </summary>
    public void Execute()
    {
        bool attackflag = controller.CurrentState == StateTag.Attack || controller.CurrentState == StateTag.JumpAttack ||
            controller.CurrentState == StateTag.SpinAttack;
        if (!attackflag) {
            //collider.enabled = false;
            return; 
        }
        //通常の三段攻撃処理
        ThreeAttackCommand();
        //ジャンプ攻撃
        JumpAttackCommand();
        //回転攻撃
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
