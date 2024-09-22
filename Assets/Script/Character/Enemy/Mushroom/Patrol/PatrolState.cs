using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 巡回時の処理を行うクラス
/// </summary>
public class PatrolState : MonoBehaviour
{
    private Transform       thisTransform;

    private int             currentPoint = 0;
    public int              CurrentPoint => currentPoint;
    /// <summary>
    /// 巡回ポイントを保持するVector3
    /// </summary>
    [SerializeField]
    private Vector3[]       patrolPoints;

    [SerializeField]
    private float           moveSpeed = 8f;
    [SerializeField]
    private float           rotationSpeed = 8f;
    [SerializeField]
    private float           moveSpeedChangeRate = 8f;

    private Movement        movement = null;
    public Movement         GetMovement() { return movement; }

    public void AwakeInitilaize()
    {
        thisTransform = gameObject.transform;
        movement = new Movement(gameObject);
    }

    public void StartInitilaize()
    {
        currentPoint = GetMinDistancePointIndex();
    }
    /// <summary>
    /// 巡回の移動処理と巡回ポイントの設定を行う
    /// </summary>
    /// <param name="time"></param>
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
    /// <summary>
    /// 次の巡回ポイントを設定する関数
    /// </summary>
    void NextPoint()
    {
        currentPoint++;
        if (currentPoint >= patrolPoints.Length)
        {
            currentPoint = 0;
        }
    }
    /// <summary>
    /// 巡回するオブジェクトから一番近い巡回ポイントを巡回開始地点に設定する関数
    /// </summary>
    /// <returns></returns>
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
