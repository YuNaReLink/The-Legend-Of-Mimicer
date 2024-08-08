using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInput
{
    private BossController controller = null;
    public BossInput(BossController _controller)
    {
        controller = _controller;
    }

    /// <summary>
    ///プレイヤーがボスと最大距離離れた時の基準
    /// </summary>
    private float maxMoveDistance = 12.0f;
    /// <summary>
    /// プレイヤーがボスに最低限近づいた時に攻撃する基準
    /// </summary>
    private float minAttackDistance = 4.0f;

    public void Initilaize()
    {
        controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Idle);
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
        if(controller.Target == null) { return; }
        //どんな状況にも限らず処理させたい入力
        StunInput();
        GuardInput();
        if (controller.GetTimer().GetTimerStun().IsEnabled()||controller.CurrentState == CharacterTag.StateTag.Damage) { return; }
        //プレイヤーとの距離を計る
        float dis = MathDistanceAndPlayer();
        //距離が離れすぎてたら
        if(dis > maxMoveDistance)
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
        Debug.Log("離れすぎたため接近中");
    }

    private void BattleInput()
    {
        //距離を計る
        float dis = MathDistanceAndPlayer();
        //攻撃を行う最低距離よりも距離が近ければ
        if (dis < minAttackDistance)
        {
            //攻撃入力
            AttackInput();
            Debug.Log("攻撃中");
        }
        else
        {
            //そうじゃなければ別行動入力
            WalkInput();
            Debug.Log("プレイヤーに接近中");
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
        switch (controller.CurrentState)
        {
            case CharacterTag.StateTag.Attack:
            case CharacterTag.StateTag.Gurid:
                return;
        }
        controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Idle);
    }

    private void WalkInput()
    {
        switch (controller.CurrentState)
        {
            case CharacterTag.StateTag.Attack:
            case CharacterTag.StateTag.Gurid:
                return;
        }
        controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Run);
    }

    private void AttackInput()
    {
        switch (controller.CurrentState)
        {
            case CharacterTag.StateTag.Gurid:
                return;
        }
        controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Attack);
        controller.GetTimer().GetTimerAttackCoolDown().StartTimer(5f);
    }

    public void GuardInput()
    {
        switch (controller.CurrentState)
        {
            case CharacterTag.StateTag.Attack:
            case CharacterTag.StateTag.Damage:
                return;
        }
        PlayerCameraController cameraController = controller.Target.GetCameraScript();
        if(cameraController == null) { return; }
        if (!cameraController.IsFPSMode()) { return; }
        controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Gurid);
    }

    private void StunInput()
    {
        if (!controller.StunFlag) { return; }
        controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Damage);
        controller.GetTimer().GetTimerStun().StartTimer(5f);
        controller.StunFlag = false;
    }

}
