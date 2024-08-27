using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneController : MonoBehaviour
{
    /// <summary>
    /// カーソルを表示・非表示を管理するクラス
    /// </summary>
    CursorController cursor = null;

    private void Awake()
    {
        cursor = CursorController.GetInstance();
    }
    void Start()
    {
        Time.timeScale = 1f;
        cursor.SetCursorLookMode(CursorLockMode.None);
        cursor.SetCursorState(true);
    }
}
