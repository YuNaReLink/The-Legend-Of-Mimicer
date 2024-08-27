using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneController : MonoBehaviour
{
    /// <summary>
    /// �J�[�\����\���E��\�����Ǘ�����N���X
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
