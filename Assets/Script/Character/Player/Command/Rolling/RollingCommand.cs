using UnityEngine;
using CharacterTag;

public class RollingCommand
{
    private PlayerController controller = null;
    public RollingCommand(PlayerController _controller)
    {
        controller = _controller;
    }

    public void Execute()
    {
        PlayerTimers timer = controller.GetTimer();
        if (!timer.GetTimerRolling().IsEnabled()){ return; }
        controller.GetKeyInput().RollTimer += Time.deltaTime;
        float rollProgress = controller.GetKeyInput().RollTimer / timer.GetTimerRolling().GetInitCount();
        //ローリングが終わったらローリング開始時の初速度をプレイヤーに返す
        if(rollProgress > 1.0f)
        {
            //ローリングが終わったら初速度を代入
            controller.CharacterRB.velocity = controller.GetKeyInput().InitVelocity;
            controller.GetMotion().ChangeMotion(StateTag.Run);
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
                case DirectionTag.Up:
                    rollDirection = controller.transform.forward;
                    break;
                case DirectionTag.Down:
                    rollDirection = -controller.transform.forward;
                    break;
                case DirectionTag.Left:
                    rollDirection = -controller.transform.right;
                    break;
                case DirectionTag.Right:
                    rollDirection = controller.transform.right;
                    break;
            }


            float currentRollSpeed = controller.GetRollCurve().Evaluate(rollProgress) * SetRollingAcceleration();
            RollingAccele(rollDirection * currentRollSpeed);
        }
        //バク転時のジャンプ処理
        if(controller.GetKeyInput().CurrentDirection == DirectionTag.Down)
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
            case DirectionTag.Up:
                if (controller.GetKeyInput().CurrentDirection == DirectionTag.Up &&
                controller.GetKeyInput().Vertical == 0)
                {
                    accele = controller.GetData().RollingUPStaticAcceleration;
                }
                else
                {
                    accele = controller.GetData().RollingUPDynamicAcceleration;
                }
                break;
            case DirectionTag.Down:
                accele = controller.GetData().RollingDOWNAcceleration;
                break;
            case DirectionTag.Left:
                accele = controller.GetData().RollingLEFTAcceleration;
                break;
            case DirectionTag.Right:
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
