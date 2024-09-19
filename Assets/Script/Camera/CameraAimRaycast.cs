using UnityEngine;

//�J�����I�u�W�F�N�g����O������Ray���΂��������s���V���O���g���̃N���X
public class CameraAimRaycast : MonoBehaviour
{
    private static CameraAimRaycast     instance;
    public static CameraAimRaycast      Instance => instance;

    private  Vector3                    sightPosition = Vector3.zero;
    public  Vector3                     GetSightPosition() { return sightPosition; }
    
    [Header("�����΂�����"),SerializeField]
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
