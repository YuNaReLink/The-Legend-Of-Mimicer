using UnityEngine;

/// <summary>
/// プレイヤーの視界クラス
/// 視界にするオブジェクトにアタッチして
/// ターゲットにロックオンする処理をする
/// </summary>
public class FocusArea : MonoBehaviour
{
    [SerializeField]
    private PlayerController    controller = null;
    [SerializeField]
    private SphereCollider      areaCollider = null;

    private Ray ray;
    private RaycastHit          hit;
    //Rayを飛ばす方向
    private Vector3             direction = Vector3.zero;
    //Rayを飛ばす距離
    [SerializeField]
    private float               distance = 10f;

    [SerializeField]
    private float               searchAngle = 130f;

    void Start()
    {
        areaCollider = GetComponent<SphereCollider>();
        if (areaCollider == null)
        {
            Debug.Log("areaColliderがアタッチされませんでした");
        }
    }
    private void Update()
    {
        if(CameraController.LockObject == null) { return; }
        if (!CameraController.LockObject.activeSelf)
        {
            CameraController.LockObject = null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Target") { return; }
        if (!GameSceneSystemController.Instance.BattleStart)
        {
            GameSceneSystemController.Instance.BattleStart = true;
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
        if (CameraController.LockObject != null) { return; }
        CameraController.FocusFlag = true;
        CameraController.LockObject = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Target") { return; }
        RemoveEnemyList(other);
        if (GameSceneSystemController.Instance.BattleStart)
        {
            GameSceneSystemController.Instance.BattleStart = false;
        }
    }
    private void RemoveEnemyList(Collider other)
    {
        if (CameraController.LockObject == null) { return; }
        if (CameraController.LockObject != other.gameObject) { return; }
        CameraController.FocusFlag = false;
        CameraController.LockObject = null;
    }
}
