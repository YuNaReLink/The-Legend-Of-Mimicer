using UnityEngine;

/// <summary>
/// �L�m�R�����X�^�[�̏�Ԃ�ύX��������N���X
/// </summary>
public class MushroomState
{
    private MushroomController      controller = null;

    private const float             MinAttackDistance = 2.0f;

    private const float             AttackCoolDownCount = 3f;

    private PatrolState             patrolState = null;

    public MushroomState(MushroomController c)
    {
        controller = c;
        patrolState = c.GetComponent<PatrolState>();
        if (patrolState == null)
        {
            Debug.LogError("PatrolState���A�^�b�`����Ă��܂���");
        }
        else
        {
            patrolState?.AwakeInitilaize();
        }
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
            //�v���C���[�Ɛ퓬
            AttackMode();
        }
    }
    public void StateFixedUpdate(float time)
    {
        //���񏈗�
        patrolState?.PatrolFixedUpdate(time);
    }
    /// <summary>
    /// �v���C���[�Ƃ̋������v��֐�
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
    private void AttackMode()
    {
        if (controller.GetTimer().GetTimerAttackCoolDown().IsEnabled()) { return; }
        if (controller.Target != null)
        {
            float dis = MathDistanceAndPlayer();
            if (dis > MinAttackDistance)
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
        controller.GetTimer().GetTimerAttackCoolDown().StartTimer(AttackCoolDownCount);
    }
}
