using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBowShot : MonoBehaviour
{
    [SerializeField]
    private GameObject arrow = null;
    [SerializeField]
    private float bulletSpeed = 5f;

    [SerializeField]
    private Transform arrowTransform = null;

    [SerializeField]
    private bool fire = false;
    public bool Fire { get { return fire; }set { fire = value; } }

    [SerializeField]
    private CrossBowAnimation animation = null;

    public void ArrowFire()
    {
        if(animation != null)
        {
            AnimatorStateInfo animinfo = animation.GetAnimator().GetCurrentAnimatorStateInfo(0);
            if (!animinfo.IsName("Hold")) { return; }
        }
        if(arrow == null) { return; }
        if(arrowTransform == null) { return; }
        GameObject Bullet = Instantiate(arrow, arrowTransform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));
        Rigidbody bulletRb = Bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(arrowTransform.forward * bulletSpeed);
        Destroy(Bullet, 10.0f);
        fire = true;
    }
}
