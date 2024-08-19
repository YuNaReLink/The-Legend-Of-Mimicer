using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの回転を行うクラス
/// </summary>
public class PlayerRotation
{
    private PlayerController controller = null;
    public PlayerRotation(PlayerController _controller)
    {
        controller = _controller;
    }
    /// <summary>
    /// カメラの回転関係
    /// </summary>
    //プレイヤーの移動量
    private Vector3 cameraVelocity;
    public Vector3 GetCameraVelocity() { return cameraVelocity; }
    //プレイヤーの進行方向に向くクォータニオン
    private Quaternion playerRot;
    //現在の回転各速度
    private float currentAngularVelocity;
    //最大の回転角速度[deg/s]
    [SerializeField]
    private float maxAngularVelocity = Mathf.Infinity;
    //進行方向にかかるおおよその時間[s]
    private float smoothTime = 0.05f;
    //現在の向きと進行方向の角度
    private float diffAngle;
    //現在の回転する角度
    private float rotAngle;

    public void MathPlayerPos(Transform transform)
    {
        //現在の位置
        controller.CurrentPos = transform.position;
        //移動量計算
        cameraVelocity = controller.CurrentPos - controller.PastPos;
        //yだけ0
        cameraVelocity.y = 0;
        //過去の位置を更新
        controller.PastPos = controller.CurrentPos;
    }

    public Quaternion SelfRotation(PlayerController controller)
    {
        PlayerInput input = controller.GetKeyInput();

        //自身が動いてる場合の注目処理
        if (PlayerCameraController.FocusFlag&& input.IsCKeyEnabled()&&
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
        else if (input.IsCKeyEnabled())
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
            playerRot = Quaternion.LookRotation(cameraVelocity, Vector3.up);
            diffAngle = Vector3.Angle(controller.transform.forward, cameraVelocity);
            //回転速度を計算する
            float targetRotationSpeed = Mathf.Min(diffAngle / smoothTime, maxAngularVelocity);
            // 回転速度を調整する
            currentAngularVelocity = Mathf.MoveTowards(currentAngularVelocity, targetRotationSpeed, maxAngularVelocity * Time.deltaTime);
            // 回転を適用する
            rotAngle = currentAngularVelocity * Time.deltaTime;
            return Quaternion.RotateTowards(controller.transform.rotation, playerRot, rotAngle);
        }
    }
}
