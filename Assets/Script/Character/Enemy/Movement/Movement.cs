using UnityEngine;

public class Movement
{
    private CharacterController     controller = null;

    private Transform               thisTransform = null;

    public Movement(GameObject gameObject)
    {
        controller = gameObject.GetComponent<CharacterController>();
        if(controller == null)
        {
            Debug.Log("CharacterController���A�^�b�`����Ă��܂���");
        }
        thisTransform = gameObject.transform;
    }

    private const float             MoveSpeedApproximately = 0.1f;

    public void MoveTo(Vector3 targetPoint, float targetMoveSpeed, float moveSpeedChangeRate, float rotationSpeed, float time)
    {
        //���݂̖ڕW�n�_�Ɍ������x�N�g�������߂�
        Vector3 targetVec = targetPoint - thisTransform.position;
        //�c�ɂ͈ړ����Ȃ��悤�ɂ���
        targetVec.y = 0;
        //�^�[�Q�b�g�����Ɍ�����ς��Ă���
        var targetRotation = Quaternion.LookRotation(targetVec, Vector3.up);
        thisTransform.rotation = Quaternion.Slerp(thisTransform.rotation, targetRotation, rotationSpeed * time);

        Vector3 velocity = controller.CharacterStatus.Velocity;
        //�ړ����x�̉����E����
        float speed = targetMoveSpeed;
        float currentSpeed = velocity.magnitude;
        if (currentSpeed < targetMoveSpeed - MoveSpeedApproximately ||
            currentSpeed > targetMoveSpeed + MoveSpeedApproximately)
        {
            speed = Mathf.Lerp(currentSpeed, targetMoveSpeed, moveSpeedChangeRate * time);
            speed = Mathf.Round(speed * 1000.0f) * 0.001f;
        }

        //�O���Ɍ������Ĉړ�����
        Vector3 currentVelocity = velocity;
        currentVelocity.x = thisTransform.forward.x * speed;
        currentVelocity.z = thisTransform.forward.z * speed;
        controller.CharacterStatus.Velocity = currentVelocity;
    }
}
