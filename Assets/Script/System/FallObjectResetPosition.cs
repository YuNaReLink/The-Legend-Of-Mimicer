using UnityEngine;

/// <summary>
/// プレイヤーがステージから落ちた時の処理を行うクラス
/// プレイヤーがステージから落ちたら最後に着地していたところに
/// 位置を設定する処理を行う
/// </summary>
public class FallObjectResetPosition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") { return; }
        ResetPosition(other);
    }

    private void ResetPosition(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        controller.GetKeyInput().GetUpInput();
        controller.CharacterRB.velocity = controller.StopMoveVelocity();
        controller.Velocity = controller.StopMoveVelocity();
        other.transform.position = controller.GetLandingPosition();
        controller.GetKeyInput().GetUpInput();
    }
}
