using UnityEngine;

//カメラオブジェクトから前方向にRayを飛ばす処理を行うシングルトンのクラス
public class CameraAimRaycast : MonoBehaviour
{
    private static CameraAimRaycast     instance;
    public static CameraAimRaycast      Instance => instance;

    private  Vector3                    sightPosition = Vector3.zero;
    public  Vector3                     GetSightPosition() { return sightPosition; }
    
    [Header("線を飛ばす距離"),SerializeField]
    private float                       aimRaycastOrigin = 10f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Update()
    {
        AimRaycast();
    }
    private void AimRaycast()
    {
        RaycastHit hit;
        Ray aim = new Ray(transform.position + transform.forward * aimRaycastOrigin, transform.forward);
        Physics.Raycast(aim, out hit);
        Debug.DrawRay(aim.origin, aim.direction,Color.red);
        if (hit.collider == null) { return; }
        sightPosition = hit.point;
    }
}
