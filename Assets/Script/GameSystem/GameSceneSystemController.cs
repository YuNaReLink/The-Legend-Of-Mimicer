using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneSystemController : MonoBehaviour
{
    public enum TriggerTag
    {
        Null,
        Door,
        Chest,
        Item
    }

    private static TriggerTag keyTriggerTag = TriggerTag.Null;

    public static TriggerTag KeyTriggerTag {  get { return keyTriggerTag; }set { keyTriggerTag = value; } }

    /// <summary>
    /// カーソルを表示・非表示を管理するクラス
    /// </summary>
    CursorController cursor = null;

    private void Awake()
    {
        cursor = CursorController.GetInstance();
    }

    private void Start()
    {
        cursor.SetCursorLookMode(CursorLockMode.Locked);
        cursor.SetCursorState(false);
    }

    private void Update()
    {
        if (GameManager.MenuEnabled)
        {
            cursor.SetCursorLookMode(CursorLockMode.None);
            cursor.SetCursorState(true);
            Time.timeScale = 0;
        }
        else
        {
            cursor.SetCursorLookMode(CursorLockMode.Locked);
            cursor.SetCursorState(false);
            Time.timeScale = 1f;
        }
    }
}
