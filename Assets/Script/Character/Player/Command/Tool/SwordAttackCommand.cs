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
        //指定した状態じゃなければ
        bool stopstate = controller.CurrentState == StateTag.Rolling || controller.CurrentState == StateTag.Damage;
        if (stopstate) { return; }
        //着地判定
        if (!controller.Landing) { return; }
        //キー入力
        if (!controller.GetKeyInput().AttackButton) { return; }
        //左クリックを押した最初は長押しフラグをfalseに
        controller.GetKeyInput().AttackHoldButton = false;
        controller.BattleMode = true;
        AnimatorStateInfo animInfo = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
        switch (controller.GetKeyInput().ThreeAttackCount)
        {
            //一段目
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
            //二段目
            case 1:
                if (!animInfo.IsName("attack1")) { return; }
                if (animInfo.normalizedTime < 0.4f) { return; }
                AttackDetailHandle();
                break;
            //三段目
            case 2:
                if (!animInfo.IsName("attack2")) { return; }
                if (animInfo.normalizedTime < 0.4f) { return; }
                AttackDetailHandle();
                break;
        }
    }
    private void AttackDetailHandle()
    {
        //三段攻撃カウントenumに代入
        controller.TripleAttack = (TripleAttack)controller.GetKeyInput().ThreeAttackCount;
        //三段目攻撃モーション再生
        controller.GetMotion().ChangeMotion(StateTag.Attack);
        //三段攻撃カウントを加算
        controller.GetKeyInput().ThreeAttackCount++;
        //三段目ならカウントを0にリセット
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
        //指定したモーションだったら
        if (CheckStopState()) { return; }
        //移動キーが何も入力されていなかったら
        if(controller.GetKeyInput().Horizontal != 0&& controller.GetKeyInput().Vertical == 0) { return; }
        //カメラ注目中のジャンプ斬り入力
        if (controller.GetKeyInput().ActionButton&& controller.GetKeyInput().IsCameraLockEnabled())
        {
            controller.GetTimer().GetTimerJumpAttackAccele().StartTimer(0.5f);
            controller.GetMotion().ChangeMotion(StateTag.JumpAttack);
            controller.GetKeyInput().ActionButton = false;
        }
        //空中にいる時のジャンプ斬り入力
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
        //未着地判定
        if (!controller.Landing) { return; }
        //三段攻撃が二段目以上なら
        if(controller.GetKeyInput().ThreeAttackCount >= (int)TripleAttack.Second) { return; }
        //回転攻撃準備動作開始フラグ
        bool readystartflag = controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("attack1")&&
            controller.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f;
        //回転攻撃動作開始フラグ
        bool spinflag = controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("readySpinAttack");
        //回転攻撃準備入力
        if (controller.GetKeyInput().AttackHoldButton&&readystartflag)
        {
            controller.GetMotion().ChangeMotion(StateTag.ReadySpinAttack);
        }
        //回転攻撃入力
        else if (!controller.GetKeyInput().AttackHoldButton&&spinflag)
        {
            controller.GetMotion().ChangeMotion(StateTag.SpinAttack);
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
