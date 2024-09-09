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
            Debug.Log("CharacterControllerがアタッチされていません");
        }
        thisTransform = gameObject.transform;
    }

    private const float             MoveSpeedApproximately = 0.1f;

    public void MoveTo(Vector3 targetPoint, float targetMoveSpeed, float moveSpeedChangeRate, float rotationSpeed, float time)
    {
        //現在の目標地点に向かうベクトルを求める
        Vector3 targetVec = targetPoint - thisTransform.position;
        //縦には移動しないようにする
        targetVec.y = 0;
        //ターゲット方向に向きを変えていく
        var targetRotation = Quaternion.LookRotation(targetVec, Vector3.up);
        thisTransform.rotation = Quaternion.Slerp(thisTransform.rotation, targetRotation, rotationSpeed * time);

        Vector3 velocity = controller.CharacterStatus.Velocity;
        //移動速度の加速・減速
        float speed = targetMoveSpeed;
        float currentSpeed = velocity.magnitude;
        if (currentSpeed < targetMoveSpeed - MoveSpeedApproximately ||
            currentSpeed > targetMoveSpeed + MoveSpeedApproximately)
        {
            speed = Mathf.Lerp(currentSpeed, targetMoveSpeed, moveSpeedChangeRate * time);
            speed = Mathf.Round(speed * 1000.0f) * 0.001f;
        }

        //前方に向かって移動する
        Vector3 currentVelocity = velocity;
        currentVelocity.x = thisTransform.forward.x * speed;
        currentVelocity.z = thisTransform.forward.z * speed;
        controller.CharacterStatus.Velocity = currentVelocity;
    }
}
