using UnityEngine;

/// <summary>
/// �J�[�\���̕\���A��\���̂Ȃǂ̐ݒ���s���V���O���g���N���X
/// </summary>
public class CursorController
{
    private CursorController(){}

    private static CursorController singleInstance = null;

    public static CursorController GetInstance()
    {
        if(singleInstance == null)
        {
            singleInstance = new CursorController();
        }
        return singleInstance;
    }

    public void SetCursorLookMode(CursorLockMode mode)
    {
        Cursor.lockState = mode;
    }
    public void SetCursorState(bool enabled)
    {
        Cursor.visible = enabled;
    }
}
