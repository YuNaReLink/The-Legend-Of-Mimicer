using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusArea : MonoBehaviour
{
    [SerializeField]
    private PlayerController controller;
    [SerializeField]
    private SphereCollider areaCollider;

    private Ray ray;
    private RaycastHit hit;
    //Rayを飛ばす方向
    private Vector3 direction;
    //Rayを飛ばす距離
    [SerializeField]
    private float distance = 10f;

    [SerializeField]
    private float searchAngle = 130f;

    // Start is called before the first frame update
    void Start()
    {
        areaCollider = GetComponent<SphereCollider>();
        if (areaCollider == null)
        {
            Debug.Log("areaColliderがアタッチされませんでした(Enemy)");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Target") { return; }
        //Rayを飛ばす方向を計算
        Vector3 temp = other.transform.position - transform.position;
        direction = temp.normalized;
        //Rayを飛ばす
        ray = new Ray(transform.position, direction);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);  // Rayをシーン上に描画
        //主人公の方向
        var playerDirection = other.transform.position - transform.position;
        //プレイヤーの前方からの主人公の方向
        var angle = Vector3.Angle(transform.forward, playerDirection);
        //サーチする角度内だったら発見
        // Rayが最初に当たった物体を調べる
        if(angle > searchAngle) { return; }
        RaycastHit[] hits = Physics.RaycastAll(ray.origin,ray.direction * distance);
        foreach(var hit in hits)
        {
            if (hit.collider.CompareTag("Target"))
            {
                CheckSameEnemy(other);
            }
        }
    }

    private void CheckSameEnemy(Collider other)
    {
        if (PlayerCameraController.LockObject != null) { return; }
        PlayerCameraController.FocusFlag = true;
        PlayerCameraController.LockObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Target")
        {
            RemoveEnemyList(other);
        }
    }

    private void RemoveEnemyList(Collider other)
    {
        if (PlayerCameraController.LockObject == null) { return; }
        if (PlayerCameraController.LockObject == other.gameObject)
        {
            PlayerCameraController.FocusFlag = false;
            PlayerCameraController.LockObject = null;
        }
    }
}
