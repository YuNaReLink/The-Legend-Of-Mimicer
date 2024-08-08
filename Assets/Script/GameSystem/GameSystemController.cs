using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemController : MonoBehaviour
{
    CursorController cursor = null;

    private void Awake()
    {
        cursor = CursorController.GetInstance();
    }

    void Start()
    {
        cursor.SetCursorLookMode(CursorLockMode.Locked);
        cursor.SetCursorState(false);
    }

    void Update()
    {
        
    }
}
