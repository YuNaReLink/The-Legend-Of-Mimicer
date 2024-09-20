using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// NavMeshAgent�̏������Ǘ�����N���X
/// </summary>
public class NavMeshController
{
    private NavMeshAgent        agent = null;

    private EnemyController     controller = null;
    public bool                 Arrival() { return agent.remainingDistance <= agent.stoppingDistance; }

    /// <summary>
    /// NavMeshAgent�Ŏg���ϐ�
    /// </summary>
    //NavMeshAgent�̃S�[�����W��������ϐ�
    [SerializeField]
    protected Vector3 goalPosition = Vector3.zero;
    //�� GetSet�֐�
    public Vector3 GoalPosition { get { return goalPosition; } set { goalPosition = value; } }
    //�p�j���Ƀ����_���ɍ��W��ݒ肷�鎞�̔��a�ϐ�
    [SerializeField]
    protected float loiterRadius = 10f;
    //��Get�֐�
    public float GetLoiterRadius() { return loiterRadius; }

    public NavMeshController(NavMeshAgent _agent,EnemyController _controller)
    {
        agent = _agent;
        controller = _controller;
        agent.acceleration = controller.GetData().Acceleration;
        agent.speed = controller.GetData().MaxSpeed;
    }

    public void SetGoalPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * loiterRadius;
        randomDirection += controller.transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, loiterRadius, 1))
        {
            goalPosition = hit.position;
        }
        else
        {
            SetGoalPosition();
        }
        //NavMeshAgent�̖ڕW�ʒu��ݒ肷��
        agent.SetDestination(goalPosition);
    }

    public void SetTargetPosition()
    {
        agent.SetDestination(controller.Target.transform.position);
    }

    public void PositionReset()
    {
        agent.velocity = Vector3.zero;
        agent.ResetPath();
    }

}
