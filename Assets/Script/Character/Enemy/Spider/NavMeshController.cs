using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController
{
    private NavMeshAgent agent = null;

    private EnemyController controller = null;

    public NavMeshController(NavMeshAgent _agent,EnemyController _controller)
    {
        agent = _agent;
        controller = _controller;
        agent.acceleration = controller.GetData().Acceleration;
        agent.speed = controller.GetData().MaxSpeed;
    }

    public void SetGoalPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * controller.GetLoiterRadius();
        randomDirection += controller.transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, controller.GetLoiterRadius(), 1))
        {
            controller.GoalPosition = hit.position;
        }
        else
        {
            SetGoalPosition();
        }
        //NavMeshAgentÇÃñ⁄ïWà íuÇê›íËÇ∑ÇÈ
        agent.SetDestination(controller.GoalPosition);
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

    public bool Arrival() { return agent.remainingDistance <= 0.1f; }
}
