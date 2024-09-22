using UnityEngine;

/// <summary>
/// ボスがプレイヤーの情報を取得するオブジェクトにアタッチし
/// プレイヤーが当たり判定にヒットした時に処理を行う
/// </summary>
public class FoundPlayerArea : MonoBehaviour
{
    private BossController controller = null;

    private void Awake()
    {
        controller = GetComponentInParent<BossController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") { return; }
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if(player == null) { return; }
        controller.Target = player;
        if(GameSceneSystemController.Instance != null)
        {
            GameSceneSystemController.Instance.BossBattleStart = true;
        }
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag != "Player") { return; }
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player == null) { return; }
        controller.Target = player;
        if(GameSceneSystemController.Instance != null)
        {
            GameSceneSystemController.Instance.BossBattleStart = true;
        }
        Destroy(gameObject);
    }

}
