using UnityEngine;
using CharacterTagList;

/// <summary>
/// プレイヤーのモーション関係の処理を行うクラス
/// </summary>
public class PlayerMotion : MotionController
{
    private PlayerController controller;
    public PlayerMotion(PlayerController _controller)
    {
        controller = _controller;
    }
    //Parametersの名前を取得
    private readonly string dirname         = "Direction";
    private readonly string jumpcountname   = "JumpCount";
    private readonly string threeattackname = "ThreeAttack";
    private readonly string damagename      = "Damage";
    private readonly string pushingname     = "Pushing";

    /// <summary>
    /// アニメーターのパラメーターに値を代入する関数
    /// </summary>
    /// <param name="anim">
    /// プレイヤーのアニメーター
    /// </param>
    /// <param name="_state">
    /// プレイヤーの状態
    /// </param>
    /// <param name="input">
    /// プレイヤーの入力クラス
    /// </param>
    private void SetAnimatorParameters(Animator anim,StateTag _state,PlayerInput input)
    {
        //状態の数値を代入
        int statenumber = (int)_state;
        int dirnumber = (int)input.CurrentDirection;
        int jumpCount = controller.GetObstacleCheck().GetLowJumpCount();
        int threeAttackCount = (int)controller.TripleAttack;
        int damageCount = (int)controller.DamageTag;
        int pushingcount = (int)controller.PushTag;
        //過去と現在の状態を記録
        controller.CharacterStatus.PastState = controller.CharacterStatus.CurrentState;
        controller.CharacterStatus.CurrentState = _state;
        input.PastDirection = input.CurrentDirection;
        //アニメーション遷移の設定
        anim.SetBool(boolname, controller.BattleMode);
        anim.SetInteger(stateName, statenumber);
        anim.SetInteger(dirname, dirnumber);
        anim.SetInteger(jumpcountname, jumpCount);
        anim.SetInteger(threeattackname, threeAttackCount);
        anim.SetInteger(damagename, damageCount);
        anim.SetInteger(pushingname, pushingcount);
    }
    /// <summary>
    /// モーションを変更する処理関数
    /// </summary>
    /// <param name="_state"></param>
    public override void ChangeMotion(StateTag _state)
    {
        //アニメーターを代入
        Animator anim = controller.GetAnimator();
        //入力クラスの代入
        PlayerInput input = controller.GetKeyInput();
        if (ChangeMotionStopCheck(_state,anim, input)) { return; }
        SetAnimatorParameters(anim,_state,input);
    }
    /// <summary>
    /// モーションを再生するかしないか判断処理する関数
    /// </summary>
    /// <param name="_state">
    /// 遷移させようとしてる状態
    /// </param>
    /// <param name="anim">
    /// プレイヤーのアニメーター
    /// </param>
    /// <param name="input">
    /// プレイヤーの入力クラス
    /// </param>
    /// <returns></returns>
    private bool ChangeMotionStopCheck(StateTag _state, Animator anim, PlayerInput input)
    {
        bool animBoolCheck = anim.GetBool(boolname) == controller.BattleMode;
        bool dirCheck = input.CurrentDirection == input.PastDirection;
        //同じ状態＆"BattleMode"と戦闘モードの条件が同じならリターン
        if (controller.CharacterStatus.CurrentState == _state && dirCheck
            && animBoolCheck&&controller.CharacterStatus.CurrentState != StateTag.Attack&&
            controller.CharacterStatus.CurrentState != StateTag.Push)
        {
            return true; 
        }
        return false;
    }
    /// <summary>
    /// 現在のモーションが終わってるか確認する関数
    /// </summary>
    /// <returns></returns>
    public override bool MotionEndCheck()
    {
        Animator anim = controller.GetAnimator();
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if(animInfo.normalizedTime >= 1.0f) { return true; }
        return false;
    }
    /// <summary>
    /// モーションを状態に限らず強制的に変更する関数
    /// </summary>
    /// <param name="_state">
    /// 遷移しようとしてる状態
    /// </param>
    public override void ForcedChangeMotion(StateTag _state)
    {
        //アニメーターを代入
        Animator anim = controller.GetAnimator();
        //入力クラスの代入
        PlayerInput input = controller.GetKeyInput();

        if (controller.CharacterStatus.CurrentState == _state) { return; }
        SetAnimatorParameters(anim, _state, input);
    }
    /// <summary>
    /// ローリングモーションが終わってるか確認する関数
    /// </summary>
    /// <returns></returns>
    public override bool IsEndRollingMotionNameCheck()
    {
        Animator anim = controller.GetAnimator();
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        PlayerMotionName motionName = new PlayerMotionName();
        string[] rollingName = motionName.GetRollingName();
        foreach(string motion in rollingName)
        {
            if (animInfo.IsName(motion)&&animInfo.normalizedTime >=1.0f)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// モーションを再生中特定の状態なら停止させる処理を行う関数
    /// </summary>
    public override void StopMotionCheck()
    {
        Animator anim = controller.GetAnimator();
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        PlayerMotionName motionName = new PlayerMotionName();

        foreach (string motion in motionName.GetMotionName())
        {
            if (animInfo.IsName(motion))
            {
                StopMotionCommand(motion, anim);
                return;
            }
        }
    }

    private const float jumpAttackMotionStop = 0.5f;
    /// <summary>
    /// モーションを停止させる詳細関数
    /// </summary>
    /// <param name="motion">
    /// 停止させるモーション名
    /// </param>
    /// <param name="anim">
    /// プレイヤーのアニメーター
    /// </param>
    private void StopMotionCommand(string motion, Animator anim)
    {
        switch (motion)
        {
            case "jumpAttack":
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= jumpAttackMotionStop)
                {
                    if (!controller.CharacterStatus.Landing)
                    {
                        anim.enabled = false;
                    }
                    else
                    {
                        anim.enabled = true;
                    }
                }
                break;
        }
    }
    //保持してるモーション名が終わったか判定する処理
    public override void EndMotionNameCheck()
    {
        Animator anim = controller.GetAnimator();
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime < 1.0f) { return; }
        PlayerMotionName motionName = new PlayerMotionName();

        foreach (string motion in motionName.GetMotionName())
        {
            if (animInfo.IsName(motion))
            {
                EndMotionCommand(motion);
                return;
            }
        }
    }
    //モーションが終わった時にする処理
    private void EndMotionCommand(string motion)
    {
        switch (motion)
        {
            case "getUp":
                controller.CharacterStatus.CurrentState = StateTag.Null;
                break;
            case "attack1":
            case "attack2":
            case "attack3":
                if (controller.CharacterStatus.CurrentState == StateTag.ReadySpinAttack) { return; }
                controller.GetKeyInput().ThreeAttackCount = 0;
                controller.CharacterStatus.CurrentState = StateTag.Null;
                controller.TripleAttack = TripleAttack.Null;
                break;
            case "jumpAttack":
                controller.GetKeyInput().ThreeAttackCount = 0;
                controller.CharacterStatus.CurrentState = StateTag.Null;
                break;
            case "spinAttack":
                controller.GetKeyInput().ThreeAttackCount = 0;
                controller.CharacterStatus.CurrentState = StateTag.Null;
                controller.TripleAttack = TripleAttack.Null;
                break;
            case "storing":
            case "posture":
                controller.CharacterStatus.CurrentState = StateTag.Null;
                break;
            case "DamageLanding":
            case "damageImpact":
                controller.GetKeyInput().ThreeAttackCount = 0;
                controller.TripleAttack = TripleAttack.Null;
                controller.CharacterStatus.CurrentState = StateTag.Null;
                break;
        }
    }
    //モーションの一部を変える処理を行う関数
    public override void Change(AnimationClip clip)
    {
        AllocateMotion("noGuard", clip);
    }
    /// <summary>
    /// 上記の詳細
    /// </summary>
    /// <param name="name">
    /// 変更先のモーション
    /// </param>
    /// <param name="clip">
    /// 変更するモーション
    /// </param>
    private void AllocateMotion(string name, AnimationClip clip)
    {
        AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[controller.GetAnimator().layerCount];
        for (int i = 0; i < controller.GetAnimator().layerCount; i++)
        {
            layerInfo[i] = controller.GetAnimator().GetCurrentAnimatorStateInfo(i);
        }

        // (3)  AnimationClipを差し替えて、強制的にアップデート
        // ステートがリセットされる
        //条件に当てはまらなければ何もしない
        PlayerMotionName setmotionnames = new PlayerMotionName();
        foreach (var motionname in setmotionnames.GetGuardEnabledMotionName())
        {
            if (controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName(motionname))
            {
                if (controller.GetAnimatorOverrideController()[motionname] == clip) { return; }
                controller.GetAnimatorOverrideController()[name] = clip;
                controller.GetAnimator().Update(0.0f);
            }
        }
    }
    
}
