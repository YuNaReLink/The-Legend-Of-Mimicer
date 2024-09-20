using UnityEngine;


/// <summary>
/// ñÓÇî≠éÀÇ∑ÇÈèàóùÇä«óùÇ∑ÇÈÉNÉâÉX
/// </summary>
public class CrossBowShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject          arrow = null;
    [SerializeField]
    private float               bulletSpeed = 5f;
    [SerializeField]
    private Transform           arrowTransform = null;

    private bool                fire = false;
    public bool                 Fire { get { return fire; }set { fire = value; } }

    private CrossBowAnimation   animation = null;

    private PlayerController    playerController = null;

    private void Awake()
    {
        animation = GetComponent<CrossBowAnimation>();
    }

    private const float DestroyCount = 10.0f;

    public bool ArrowFire()
    {
        if(animation == null) { return false; }

        AnimatorStateInfo animinfo = animation.GetAnimator().GetCurrentAnimatorStateInfo(0);
        if (!animinfo.IsName("Hold")) { return false; }

        if (fire) { return false; }
        if(arrow == null) { return false; }
        if(arrowTransform == null) { return false; }
        GameObject bullet = Instantiate(arrow, arrowTransform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));
        bullet.transform.LookAt(CameraAimRaycast.Instance.GetSightPosition());
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(bullet.transform.forward * bulletSpeed);
        Destroy(bullet, DestroyCount);
        fire = true;
        return true;
    }
}
