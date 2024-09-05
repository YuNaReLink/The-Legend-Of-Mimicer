using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSensitivityManager
{
    private static MouseSensitivityManager instance;
    private MouseSensitivityManager() { }

    public static MouseSensitivityManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new MouseSensitivityManager();
            }
            return instance;
        }
    }
    public readonly float MaxMouseSpeed = 10.0f;

    private float mouseXSpeed = 5.0f;

    public float GetMouseXSpeed => mouseXSpeed;

    public void SetMouseXSpeed(float speed)
    {
        mouseXSpeed = speed;
    }

    private float mouseYSpeed = 5.0f;

    public float GetMouseYSpeed => mouseYSpeed;

    public void SetMouseYSpeed(float speed)
    {
        mouseYSpeed = speed;
    }
}
