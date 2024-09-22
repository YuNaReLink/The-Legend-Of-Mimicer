using UnityEngine;

public class BossState
{
    private BossController      controller = null;
    public BossState(BossController _controller)
    {
        controller = _controller;
    }

    /// <summary>
    ///プレイヤーがボスと最大距離離れた時の基準
    /// </summary>
    private const float         MaxMoveDistance = 12.0f;
    /// <summary>
    /// プレイヤーがボスに最低限近づいた時に攻撃する基準
    /// </summary>
    private const float         MinAttackDistance = 4.0f;

    public void Initilaize()
    {
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Idle);
    }

    /// <summary>
    /// プレイヤーとの距離を計る関数
    /// </summary>
    /// <returns></returns>
    private float MathDistanceAndPlayer()
    {
        PlayerController player = controller.Target;
        Vector3 sub = controller.transform.position - player.transform.position;
        return sub.magnitude;
    }

    public void StateInput()
    {
        if(controller.Target == null) 
        {
            IdleInput();
            return; 
        }
        if (controller.CharacterStatus.DeathFlag) { return; }
        //どんな状況にも限らず処理させたい入力
        //ボスが倒れる処理
        controller.GetBossDamageCommand().DoUpdate();
        //ボスが起き上がる処理
        RevivalInput();
        //ボスが特定の条件下でガードする処理
        GuardInput();
        if (controller.GetTimer().GetTimerStun().IsEnabled()) { return; }
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Damage:
            case CharacterTagList.StateTag.GetUp:
                return;
        }
        //プレイヤーとの距離を計る
        float dis = MathDistanceAndPlayer();
        //距離が離れすぎてたら
        if(dis > MaxMoveDistance)
        {
            //プレイヤーに近づく
            NearsToPlayerInput();
        }
        else
        {
            //プレイヤーと戦闘
            BattleInput();
        }
    }

    private void NearsToPlayerInput()
    {
        WalkInput();
    }

    private void BattleInput()
    {
        //距離を計る
        float dis = MathDistanceAndPlayer();
        //攻撃を行う最低距離よりも距離が近ければ
        if (dis < MinAttackDistance)
        {
            //攻撃入力
            AttackInput();
        }
        else
        {
            //そうじゃなければ別行動入力
            WalkInput();
        }

    }

    /// <summary>
    /// 下記は入力別関数
    /// 待機
    /// 歩き
    /// 攻撃
    /// 防御
    /// 怯み
    /// </summary>
    private void IdleInput()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.Gurid:
                return;
        }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Idle);
    }

    private void WalkInput()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.Gurid:
                return;
        }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Run);
    }

    private void AttackInput()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Gurid:
            case CharacterTagList.StateTag.Attack:
                return;
        }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Attack);
        controller.GetTimer().GetTimerAttackCoolDown().StartTimer(5f);
    }

    public void GuardInput()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.Damage:
            case CharacterTagList.StateTag.GetUp:
                return;
        }
        CameraController cameraController = controller.Target.GetCameraController();
        if(cameraController == null) { return; }
        if (!cameraController.IsFPSMode()) { return; }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Gurid);
    }

    public void RevivalInput()
    {
        if (!controller.RevivalFlag) {  return; }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.GetUp);
        controller.SetRevivalFlag(false);
    }

}
