using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���񎞂̏������s���N���X
/// </summary>
public class PatrolState : MonoBehaviour
{
    private Transform       thisTransform;

    private int             currentPoint = 0;
    public int              CurrentPoint => currentPoint;
    /// <summary>
    /// ����|�C���g��ێ�����Vector3
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
    /// ����̈ړ������Ə���|�C���g�̐ݒ���s��
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
    /// ���̏���|�C���g��ݒ肷��֐�
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
    /// ���񂷂�I�u�W�F�N�g�����ԋ߂�����|�C���g������J�n�n�_�ɐݒ肷��֐�
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
