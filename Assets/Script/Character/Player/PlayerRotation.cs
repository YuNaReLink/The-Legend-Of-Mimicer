using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの回転を行うクラス
/// </summary>
public class PlayerRotation
{
    private PlayerController controller = null;
    public PlayerRotation(PlayerController _controller, Quaternion rotation)
    {
        controller = _controller;
        targetRotation = rotation;
    }

    private Quaternion targetRotation = Quaternion.identity;

    public Quaternion SelfRotation(PlayerController controller)
    {
        PlayerInput input = controller.GetKeyInput();

        //自身が動いてる場合の注目処理
        if (PlayerCameraController.FocusFlag&& input.IsCameraLockEnabled()&&
            PlayerCameraController.LockObject != null)
        {
            // 敵の方向ベクトルを取得
            Vector3 targetObject = PlayerCameraController.LockObject.transform.position;
            targetObject.y = controller.transform.position.y;
            Vector3 enemyDirection = (targetObject - controller.transform.position).normalized;
            // プレイヤーが常に敵の方向を向く
            Quaternion enemyRotation = Quaternion.LookRotation(enemyDirection, Vector3.up);
            return enemyRotation;
        }
        //条件に当てはまっていたらプレイヤーの向きを
        //カメラの方向によって取得しないようにする
        else if (input.IsCameraLockEnabled())
        {
            Vector3 forwardPos = new Vector3(0,0,2.5f);
            //前方方向を取得
            Vector3 resetfocusObjectDirection = ((controller.transform.position + controller.transform.forward) - controller.transform.position).normalized;
            resetfocusObjectDirection.y = 0;
            // プレイヤーが常に前方方向を向く
            Quaternion resetcameraRotation = Quaternion.LookRotation(resetfocusObjectDirection, Vector3.up);
            return resetcameraRotation;
        }
        else
        {
            //そうじゃなければ通常の三人称カメラ処理
            var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
            var velocity = horizontalRotation * new Vector3(controller.GetKeyInput().Horizontal, 0, controller.GetKeyInput().Vertical).normalized;
            var rotationSpeed = 600 * Time.deltaTime;
            if (velocity.magnitude > 0.5f)
            {
                targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
            }
            return Quaternion.RotateTowards(controller.transform.rotation, targetRotation, rotationSpeed);
        }
    }
}
