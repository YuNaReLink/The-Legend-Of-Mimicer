using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity
{
    private Vector3 velocity;

    void Start()
    {
        velocity = Vector3.zero;
    }

    public float AddGravity()
    {
        return velocity.y += Physics.gravity.y * Time.deltaTime;
    }
}
