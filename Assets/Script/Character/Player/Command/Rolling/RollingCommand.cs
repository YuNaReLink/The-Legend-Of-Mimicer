using UnityEngine;

public class RollingCommand : InterfaceBaseCommand, InterfaceBaseInput
{
    private PlayerController controller = null;
    public RollingCommand(PlayerController _controller)
    {
        controller = _controller;
    }

    public void Input()
    {
        switch (controller.CurrentState)
        {
            case CharacterTagList.StateTag.Rolling:
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.JumpAttack:
            case CharacterTagList.StateTag.ReadySpinAttack:
            case CharacterTagList.StateTag.SpinAttack:
            case CharacterTagList.StateTag.GetUp:
                return;
        }
        if (!controller.Landing) { return; }
        if (controller.GetMotion().IsEndRollingMotionNameCheck()) { return; }
        if (!controller.GetKeyInput().ActionButton) { return; }
        if (!controller.GetKeyInput().IsCameraLockEnabled())
        {
            //完全に止まっていたらリターン
            if (controller.GetKeyInput().Vertical == 0 && controller.GetKeyInput().Horizontal == 0) { return; }
            controller.GetKeyInput().CurrentDirection = CharacterTagList.DirectionTag.Up;
            //ローリングの移動を行うための初速度を代入
            controller.GetKeyInput().InitVelocity = controller.CharacterRB.velocity;
            controller.GetKeyInput().RollTimer = 0.0f;

            controller.GetTimer().GetTimerRolling().StartTimer(0.4f);
            controller.GetTimer().GetTimerNoAccele().StartTimer(0.4f);
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Rolling);
            //Shiftキーを無効にする
            controller.GetKeyInput().ActionButton = false;
        }
        else
        {
            if (controller.BattleMode && (controller.GetKeyInput().Vertical > 0 || controller.GetKeyInput().Vertical == 0 && controller.GetKeyInput().Horizontal == 0)) { return; }
            if (controller.GetKeyInput().Vertical > 0 || controller.GetKeyInput().Vertical == 0 && controller.GetKeyInput().Horizontal == 0)
            {
                controller.GetKeyInput().CurrentDirection = CharacterTagList.DirectionTag.Up;
            }
            DirectionRollingInput();
        }
    }
    private void DirectionRollingInput()
    {
        float rollingcount = 0.4f;
        float noaccele = 0.4f;
        if (controller.GetKeyInput().Horizontal > 0)
        {
            controller.GetKeyInput().CurrentDirection = CharacterTagList.DirectionTag.Right;
        }
        if (controller.GetKeyInput().Horizontal < 0)
        {
            controller.GetKeyInput().CurrentDirection = CharacterTagList.DirectionTag.Left;
        }
        if (controller.GetKeyInput().Vertical < 0 && controller.GetKeyInput().Horizontal == 0)
        {
            rollingcount = 0.5f;
            noaccele = 0.5f;
            controller.GetKeyInput().CurrentDirection = CharacterTagList.DirectionTag.Down;
        }
        controller.GetKeyInput().InitVelocity = controller.CharacterRB.velocity;
        controller.GetKeyInput().RollTimer = 0.0f;
        controller.GetTimer().GetTimerRolling().StartTimer(rollingcount);
        controller.GetTimer().GetTimerNoAccele().StartTimer(noaccele);
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Rolling);
        //Shiftキーを無効にする
        controller.GetKeyInput().ActionButton = false;
    }

    public void Execute()
    {
        PlayerTimers timer = controller.GetTimer();
        if (!timer.GetTimerRolling().IsEnabled()){ return; }
        controller.GetKeyInput().RollTimer += Time.deltaTime;
        float rollProgress = controller.GetKeyInput().RollTimer / timer.GetTimerRolling().GetInitCount();
        if(rollProgress <= 0.05f)
        {
            controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Rolling);
        }
        //ローリングが終わったらローリング開始時の初速度をプレイヤーに返す
        else if(rollProgress > 1.0f)
        {
            //ローリングが終わったら初速度を代入
            controller.CharacterRB.velocity = controller.GetKeyInput().InitVelocity;
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Run);
        }
        //ローリング中崖ジャンプ処理が実行されたら
        else if (controller.GetObstacleCheck().CliffJumpFlag)
        {
            controller.CharacterRB.velocity = controller.GetKeyInput().InitVelocity;
            timer.GetTimerRolling().End();
        }
        //ローリングのXY方向の加速
        else
        {
            // アニメーションカーブを使用して速度を調整
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
        //バク転時のジャンプ処理
        if(controller.GetKeyInput().CurrentDirection == CharacterTagList.DirectionTag.Down)
        {
            AnimatorStateInfo animInfo = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
            if (!animInfo.IsName("backFlip")) { return; }
            if(animInfo.normalizedTime <= 0.0f) { return; }
            if (controller.Landing&&!controller.Jumping)
            {
                RollingJump(1000f);
                controller.Jumping = true;
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
        controller.Velocity += force;
    }

    private void RollingJump(float force)
    {
        controller.CharacterRB.AddForce(controller.transform.up * force, ForceMode.Impulse);
    }

}
