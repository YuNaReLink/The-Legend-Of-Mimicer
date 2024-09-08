using UnityEngine;

public class BossMotionController : MotionController
{
    private BossController controller = null;
    public BossMotionController(BossController _controller)
    {
       controller = _controller;
    }
    /// <summary>
    /// ボスのアニメーションを変更する関数
    /// </summary>
    /// <param name="tag"></param>
    public override void ChangeMotion(CharacterTagList.StateTag tag)
    {
        //状態が前と同じなら早期リターン
        if (tag == controller.CharacterStatus.CurrentState) { return; }
        //アニメーション取得
        Animator anim = controller.GetAnimator();
        //アニメーションの速度が0以下なら
        if (anim.speed <= 0)
        {
            //速度を1に
            anim.speed = 1;
        }
        //現在の状態を過去に
        controller.CharacterStatus.PastState = controller.CharacterStatus.CurrentState;
        //現在に新しい状態を
        controller.CharacterStatus.CurrentState = tag;
        //新しい状態をint型でアニメーションに設定
        anim.SetInteger(stateName, (int)tag);
    }

    private const float maxStampAttackNormalizedTime = 0.7f;

    private const float maxStunNormalizedTime = 0.8f;
    /// <summary>
    /// 個別のアニメーションで
    /// 途中でアニメーションを止めたい時に処理する関数のBossバージョン
    /// </summary>
    public override void StopMotionCheck()
    {
        //状態がなしなら早期リターン
        if (controller.CharacterStatus.CurrentState == CharacterTagList.StateTag.Null) { return; }
        //アニメーション取得
        Animator anim = controller.GetAnimator();
        //現在のアニメーションの詳細取得
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        //状態によって処理分け
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
                //攻撃中でアニメーション進行度がmaxStampAttackNormalizedTime以上なら
                if (animInfo.IsName("stampAttack") && animInfo.normalizedTime >= maxStampAttackNormalizedTime)
                {
                    //タイマーが動いていたら
                    if (controller.GetTimer().GetTimerAttackCoolDown().IsEnabled())
                    {
                        anim.speed = 0;
                    }
                    //止まっていたら
                    else
                    {
                        anim.speed = 1;
                    }
                }
                break;
            case CharacterTagList.StateTag.Damage:
                //怯み状態でアニメーション進行度がmaxStunNormalizedTime以上なら
                if (animInfo.IsName("stun") && animInfo.normalizedTime >= maxStunNormalizedTime)
                {
                    //タイマーが動いていたら
                    if (controller.GetTimer().GetTimerStun().IsEnabled())
                    {
                        anim.speed = 0;
                    }
                    //止まっていたら
                    else
                    {
                        anim.speed = 1;
                    }
                }
                break;
        }
    }
    /// <summary>
    /// 指定があるアニメーションの終わりに処理したいものを記述する関数のBossバージョン
    /// </summary>
    public override void EndMotionNameCheck()
    {
        //アニメーション取得
        Animator anim = controller.GetAnimator();
        //現在のアニメーションの詳細取得
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        //アニメーション進行度が終わっていたら
        if (animInfo.normalizedTime < 1.0f) { return; }
        //指定してるボスモーション名を取得
        BossMotionName motionName = new BossMotionName();
        //現在のアニメーションが指定してるモーション名と一致するものがあったら
        foreach (string motion in motionName.GetMotionName())
        {
            if (animInfo.IsName(motion))
            {
                //モーション終了時の処理を行う
                EndMotionCommand(motion);
                return;
            }
        }
    }
    /// <summary>
    /// モーション終了時に行う処理の詳細
    /// </summary>
    /// <param name="motion"></param>
    public void EndMotionCommand(string motion)
    {
        //アニメーション取得
        Animator anim = controller.GetAnimator();
        //アニメーション詳細取得
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        switch (motion)
        {
            case "Idle":
                break;
            case "walk":
                break;
            case "stampAttack":
                controller.CharacterStatus.CurrentState = CharacterTagList.StateTag.Null;
                break;
            case "guard":
                CameraController cameraController = controller.Target.GetCameraController();
                if (cameraController == null) { return; }
                if (cameraController.IsFPSMode()) { return; }
                controller.CharacterStatus.CurrentState = CharacterTagList.StateTag.Null;
                break;
            case "stun":
                //怯み終わりに復帰フラグをON
                controller.RevivalFlag = true;
                break;
            case "returnUp":
                controller.CharacterStatus.CurrentState = CharacterTagList.StateTag.Null;
                break;
        }
    }
}
