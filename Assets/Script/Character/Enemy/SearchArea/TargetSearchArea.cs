using UnityEngine;

public class TargetSearchArea : MonoBehaviour
{
    private EnemyController     controller = null;
    [SerializeField]
    public float                angle = 130f;

    private void Awake()
    {
        controller = GetComponentInParent<EnemyController>();
        if(controller == null)
        {
            Debug.LogError("EnemyControllerがアタッチされていません");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //視界の範囲内の当たり判定
        if (other.gameObject.tag != "Player") { return; }
        //視界の角度内に収まっているか
        Vector3 posDelta = other.transform.position - transform.position;
        float target_angle = Vector3.Angle(transform.forward, posDelta);
        //target_angleがangleに収まっているかどうか
        if (target_angle < angle) 
        {
            if (Physics.Raycast(transform.position, posDelta, out RaycastHit hit)) //Rayを使用してtargetに当たっているか判別
            {
                if (hit.collider == other)
                {
                    if (controller.Target == null)
                    {
                        controller.Target = other.GetComponent<PlayerController>();
                    }
                }
                else
                {
                    controller.Target = null;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController == null) { return; }
        //敵の状態を変更
        controller.Target = null;
    }
}
