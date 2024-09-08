using UnityEngine;

public class PatrolState : MonoBehaviour
{
    private Transform thisTransform;

    int currentPoint = 0;
    public int CurrentPoint => currentPoint;

    [SerializeField]
    Vector3[] patrolPoints;

    [SerializeField]
    float moveSpeed = 8;
    [SerializeField]
    float rotationSpeed = 8;
    [SerializeField]
    float moveSpeedChangeRate = 8;

    private Movement movement = null;
    public Movement GetMovement() { return movement; }

    public void AwakeInitilaize()
    {
        thisTransform = gameObject.transform;
        movement = new Movement(gameObject);
    }

    public void StartInitilaize()
    {
        currentPoint = GetMinDistancePointIndex();
    }

    public void PatrolFixedUpdate(float time)
    {
        movement.MoveTo(patrolPoints[currentPoint], moveSpeed, moveSpeedChangeRate, rotationSpeed, time);

        Vector3 targetVec = patrolPoints[currentPoint] - thisTransform.position;
        targetVec.y = 0.0f;
        float targetDistance = targetVec.magnitude;
        const float minPatrolDistance = 0.1f;
        if (targetDistance < minPatrolDistance + moveSpeed * time)
        {
            NextPoint();
        }
    }

    void NextPoint()
    {
        currentPoint++;
        if (currentPoint >= patrolPoints.Length)
        {
            currentPoint = 0;
        }
    }

    private int GetMinDistancePointIndex()
    {
        int re = currentPoint;
        float minDistance = (patrolPoints[currentPoint] - thisTransform.position).magnitude;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Vector3 targetVec = patrolPoints[i] - thisTransform.position;
            float targetDistance = targetVec.magnitude;
            if (targetDistance < minDistance)
            {
                minDistance = targetDistance;
                re = i;
            }
        }
        return re;
    }
}
