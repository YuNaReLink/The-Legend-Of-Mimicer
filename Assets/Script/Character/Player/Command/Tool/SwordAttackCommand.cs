using UnityEngine;

public class SwordAttackCommand : InterfaceBaseToolCommand
{
    private PlayerController controller = null;

    public SwordAttackCommand(PlayerController _controller)
    {
        controller = _controller;
    }

    //PlayerInputで記述してる内容
    public void Input()
    {
        //三弾攻撃の入力
        ThreeAttackInput();
        //ジャンプ攻撃の入力
        JumpAttackInput();
        //回転攻撃の入力
        SpinAttackInput();
    }

    private const float MaxMotionNormalizedTimeOfAttack = 0.4f;
    private const float MaxMotionNormalizedTimeOfAttack3 = 0.6f;

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
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Rolling:
            case CharacterTagList.StateTag.ReadySpinAttack:
            case CharacterTagList.StateTag.SpinAttack:
            case CharacterTagList.StateTag.JumpAttack:
            case CharacterTagList.StateTag.Damage:
                return;
        }
        //着地判定
        if (!controller.CharacterStatus.Landing) { return; }
        //キー入力
        if (!controller.GetKeyInput().AttackButton) { return; }
        //左クリックを押した最初は長押しフラグをfalseに
        controller.GetKeyInput().AttackHoldButton = false;
        controller.BattleMode = true;
        AnimatorStateInfo animInfo = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
        switch (controller.GetKeyInput().ThreeAttackCount)
        {
            //一段目
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
            //二段目
            case (int)CharacterTagList.TripleAttack.Second:
                if (!animInfo.IsName("attack1")) { return; }
                if (animInfo.normalizedTime < MaxMotionNormalizedTimeOfAttack) { return; }
                AttackDetailHandle();
                break;
            //三段目
            case (int)CharacterTagList.TripleAttack.Third:
                if (!animInfo.IsName("attack2")) { return; }
                if (animInfo.normalizedTime < MaxMotionNormalizedTimeOfAttack) { return; }
                AttackDetailHandle();
                break;
        }
    }
    private void AttackDetailHandle()
    {
        //三段攻撃カウントenumに代入
        controller.TripleAttack = (CharacterTagList.TripleAttack)controller.GetKeyInput().ThreeAttackCount;
        //三段目攻撃モーション再生
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Attack);
        //三段攻撃カウントを加算
        controller.GetKeyInput().ThreeAttackCount++;
        //三段目ならカウントを0にリセット
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
        //指定したモーションだったら
        if (CheckStopState()) { return; }
        //移動キーが何も入力されていなかったら
        if(controller.GetKeyInput().Horizontal != 0&& controller.GetKeyInput().Vertical == 0) { return; }
        //カメラ注目中のジャンプ斬り入力
        if (controller.GetKeyInput().ActionButton&& controller.GetKeyInput().IsCameraLockEnabled())
        {
            controller.GetTimer().GetTimerJumpAttackAccele().StartTimer(JumpAttackAcceleCount);
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.JumpAttack);
            controller.GetKeyInput().ActionButton = false;
        }
        //空中にいる時のジャンプ斬り入力
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
        //未着地判定
        if (!controller.CharacterStatus.Landing) { return; }
        //三段攻撃が二段目以上なら
        if(controller.GetKeyInput().ThreeAttackCount >= (int)CharacterTagList.TripleAttack.Second) { return; }
        //回転攻撃準備動作開始フラグ
        bool readystartflag = controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("attack1")&&
            controller.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= MotionNormalizedTimeOfAttack1;
        //回転攻撃動作開始フラグ
        bool spinflag = controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("readySpinAttack");
        //回転攻撃準備入力
        if (controller.GetKeyInput().AttackHoldButton&&readystartflag)
        {
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.ReadySpinAttack);
        }
        //回転攻撃入力
        else if (!controller.GetKeyInput().AttackHoldButton&&spinflag)
        {
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.SpinAttack);
        }
    }

    /// <summary>
    /// PlayerControllerで記述してる内容
    /// </summary>
    public void Execute()
    {
        //以下の状態だと早期リターン
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.JumpAttack:
            case CharacterTagList.StateTag.SpinAttack:
                break;
            default:
                return;
        }
        //↓プレイヤーオブジェクトが攻撃時に動く処理を行っている
        //三段攻撃処理
        ThreeAttackCommand();
        //ジャンプ攻撃
        JumpAttackCommand();
        //回転攻撃
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
