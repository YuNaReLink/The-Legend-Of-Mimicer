using UnityEngine;

public class CrossBowShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject arrow = null;
    [SerializeField]
    private float bulletSpeed = 5f;
    [SerializeField]
    private Transform arrowTransform = null;

    private bool fire = false;
    public bool Fire { get { return fire; }set { fire = value; } }

    private CrossBowAnimation animation = null;

    public bool ArrowFire()
    {
        if(animation != null)
        {
            AnimatorStateInfo animinfo = animation.GetAnimator().GetCurrentAnimatorStateInfo(0);
            if (!animinfo.IsName("Hold")) { return false; }
        }
        if (fire) { return false; }
        if(arrow == null) { return false; }
        if(arrowTransform == null) { return false; }
        GameObject Bullet = Instantiate(arrow, arrowTransform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));
        Rigidbody bulletRb = Bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(arrowTransform.forward * bulletSpeed);
        Destroy(Bullet, 10.0f);
        fire = true;
        return true;
    }
}
