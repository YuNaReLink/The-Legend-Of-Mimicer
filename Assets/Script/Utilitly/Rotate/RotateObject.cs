using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("X•ûŒü‰ñ“]İ’è")]
    [SerializeField]
    private bool rotateX = false;
    [Header("Y•ûŒü‰ñ“]İ’è")]
    [SerializeField]
    private bool rotateY = false;
    [Header("Z•ûŒü‰ñ“]İ’è")]
    [SerializeField]
    private bool rotateZ = false;
    [Header("‘¬“xİ’è")]
    [SerializeField]
    private float speed = 10f;
    [Header("”½“]İ’è")]
    [SerializeField]
    private bool reverse = false;

    private int Reverse()
    {
        return reverse ? -1 : 1;
    }

    // Update is called once per frame
    private void Update()
    {
        if (rotateX)
        {
            MoveRotate(Vector3.right * Reverse(), speed);
        }

        if(rotateY)
        {
            MoveRotate(Vector3.up * Reverse(), speed);
        }

        if(rotateZ)
        {
            MoveRotate(Vector3.forward * Reverse(), speed);
        }
    }

    private void MoveRotate(Vector3 dir,float _speed)
    {
        transform.Rotate(dir,_speed * Time.deltaTime);
    }
}
