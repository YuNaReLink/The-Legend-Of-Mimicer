using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAimRaycast : MonoBehaviour
{
    private static CameraAimRaycast instance;
    public static CameraAimRaycast Instance => instance;

    private  Vector3 sightPosition = Vector3.zero;
    public  Vector3 GetSightPosition() { return sightPosition; }

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
        Ray aim = new Ray(transform.position + transform.forward * 10f, transform.forward);
        Physics.Raycast(aim, out hit);
        Debug.DrawRay(aim.origin, aim.direction,Color.red);
        if (hit.collider != null)
        {
            sightPosition = hit.point;
        }
    }
}
