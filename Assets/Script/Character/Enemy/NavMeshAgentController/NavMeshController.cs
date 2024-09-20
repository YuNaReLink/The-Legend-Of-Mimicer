using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// NavMeshAgentの処理を管理するクラス
/// </summary>
public class NavMeshController
{
    private NavMeshAgent        agent = null;

    private EnemyController     controller = null;
    public bool                 Arrival() { return agent.remainingDistance <= agent.stoppingDistance; }

    /// <summary>
    /// NavMeshAgentで使う変数
    /// </summary>
    //NavMeshAgentのゴール座標を代入する変数
    [SerializeField]
    protected Vector3 goalPosition = Vector3.zero;
    //の GetSet関数
    public Vector3 GoalPosition { get { return goalPosition; } set { goalPosition = value; } }
    //徘徊時にランダムに座標を設定する時の半径変数
    [SerializeField]
    protected float loiterRadius = 10f;
    //のGet関数
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
        //NavMeshAgentの目標位置を設定する
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
