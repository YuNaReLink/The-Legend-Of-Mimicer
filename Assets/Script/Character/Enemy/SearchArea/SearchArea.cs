using UnityEngine;

public class SearchArea : MonoBehaviour
{
    [SerializeField]
    private EnemyController     controller = null;
    [SerializeField]
    private SphereCollider      searchArea = null;
    [SerializeField]
    private float               searchAngle = 180f;

    private Ray                 ray;
    private RaycastHit          hit;
    //Rayを飛ばす方向
    private Vector3             direction = Vector3.zero;
    //Rayを飛ばす距離
    [SerializeField]
    private float               distance = 0;

    private void Awake()
    {
        controller = GetComponentInParent<EnemyController>();
        if(controller == null)
        {
            Debug.LogError("EnemyControllerがアタッチされていません");
        }
        searchArea = GetComponent<SphereCollider>();
        if(searchArea == null)
        {
            Debug.LogError("SphereColliderがアタッチされていません");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        HandleCollision(other);
    }
    private void HandleCollision(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if(playerController == null) { return; }
        //Rayを飛ばす方向を計算
        Vector3 temp = other.transform.position - transform.position;
        direction = temp.normalized;
        //Rayを飛ばす
        ray = new Ray(transform.position, direction);
        // Rayをシーン上に描画
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);  
        //主人公の方向
        var playerDirection = other.transform.position - transform.position;
        //敵の前方からの主人公の方向
        var angle = Vector3.Angle(transform.forward, playerDirection);
        //サーチする角度内だったら発見
        if (angle <= searchAngle)
        {
            // プレイヤーと敵の間に障害物がないかチェック
            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    controller.Target = playerController;
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
