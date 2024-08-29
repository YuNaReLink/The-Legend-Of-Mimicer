using UnityEngine;

/// <summary>
/// OpenDoorクラスと一緒に使うクラス
/// 檻付の扉の檻を解除するクラス
/// </summary>
public class MoveCage : MonoBehaviour
{
    [SerializeField]
    private bool            open = false;
    [SerializeField]
    private Vector3         goleCagePos = new Vector3(0, 7, 0);

    [SerializeField]
    private float           openSpeed = 10f;

    [SerializeField]
    private OpenDoor        door = null;
    private SphereCollider  sphereCollider = null;

    [SerializeField]
    private SwitchGimic     switchGimic = null;

    private void Awake()
    {
        sphereCollider = GetComponentInParent<SphereCollider>();
    }

    private void Start()
    {
        open = false;
        if(door != null)
        {
            door.enabled = false;
            sphereCollider.enabled = false;
        }
    }

    private void Update()
    {
        if(switchGimic == null) { return; }
        if (switchGimic.IsSwitchFlag())
        {
            open = true;
        }
        Move();
    }

    private void Move()
    {
        if (!open) { return; }
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, goleCagePos, openSpeed * Time.deltaTime);
        Vector3 doorpos = transform.localPosition;
        if (doorpos.y >= goleCagePos.y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, goleCagePos.y, transform.localPosition.z);
            open = false;
            if(door != null)
            {
                door.enabled = true;
                sphereCollider.enabled = true;
            }
            if(switchGimic != null)
            {
                switchGimic = null;
            }
        }
    }
}
