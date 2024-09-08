using UnityEngine;

public class MushroomState : MonoBehaviour
{
    private MushroomController controller = null;

    private const float minAttackDistance = 2.0f;

    private PatrolState patrolState = null;
    public void SetController(MushroomController _controller)
    {
        controller = _controller;
        patrolState = GetComponent<PatrolState>();
        if(patrolState == null)
        {
            Debug.LogError("PatrolStateがアタッチされていません");
        }
        patrolState?.AwakeInitilaize();
    }

    public void StartInitilaize()
    {
        patrolState?.StartInitilaize();
    }

    public void StateUpdate()
    {
        if (controller.CharacterStatus.DeathFlag) { return; }
        if (!controller.FoundPlayer)
        {
            if (controller.BattleMode)
            {
                controller.BattleMode = false;
            }
            if (controller.Target != null)
            {
                controller.FoundPlayer = true;
            }
            MoveState();
        }
        else
        {
            if (!controller.BattleMode)
            {
                controller.BattleMode = true;
            }
            //プレイヤーと戦闘
            AttackMode();
        }
    }


    public void StateFixedUpdate(float time)
    {
        if (controller.CharacterStatus.DeathFlag) { return; }
        //巡回処理
        patrolState?.PatrolFixedUpdate(time);
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

    private void MoveState()
    {
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Run);
    }

    private void StopInput()
    {
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Idle);
        controller.CharacterRB.velocity = Vector3.zero;
        controller.GetTimer().GetTimerIdle().StartTimer(3f);
    }

    private void AttackMode()
    {
        if (controller.GetTimer().GetTimerAttackCoolDown().IsEnabled()) { return; }
        if (controller.Target != null)
        {
            float dis = MathDistanceAndPlayer();
            if (dis > minAttackDistance)
            {
                controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Run);
            }
            else
            {
                AttackInput();
            }
        }
        else
        {
            controller.FoundPlayer = false;
        }
    }

    private void AttackInput()
    {
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Attack);
        controller.GetTimer().GetTimerAttackCoolDown().StartTimer(3f);
    }
}
