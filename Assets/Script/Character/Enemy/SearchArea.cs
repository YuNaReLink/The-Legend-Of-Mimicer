using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchArea : MonoBehaviour
{
    [SerializeField]
    private EnemyController controller = null;
    [SerializeField]
    private SphereCollider  searchCollider = null;
    [SerializeField]
    private float searchAngle = 180f;

    private Ray ray;
    private RaycastHit hit;
    //Rayを飛ばす方向
    private Vector3 direction;
    //Rayを飛ばす距離
    [SerializeField]
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        searchCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        HandleCollision(other);
    }
    private void OnCollisionStay(Collision collision)
    {
        HandleCollision(collision.collider);
    }

    private void HandleCollision(Collider other)
    {
        if (other.tag != "Player") { return; }
        //Rayを飛ばす方向を計算
        Vector3 temp = other.transform.position - transform.position;
        direction = temp.normalized;
        //Rayを飛ばす
        ray = new Ray(transform.position, direction);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);  // Rayをシーン上に描画
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
                    controller.Target = other.GetComponent<PlayerController>();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") { return; }
        //敵の状態を変更
        controller.Target = null;
    }
}
