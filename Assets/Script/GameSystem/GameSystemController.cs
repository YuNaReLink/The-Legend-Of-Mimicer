using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemController : MonoBehaviour
{
    public enum TriggerTag
    {
        Null,
        Door,
        Chest
    }

    private static TriggerTag keyTriggerTag = TriggerTag.Null;

    public static TriggerTag KeyTriggerTag {  get { return keyTriggerTag; }set { keyTriggerTag = value; } }

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
