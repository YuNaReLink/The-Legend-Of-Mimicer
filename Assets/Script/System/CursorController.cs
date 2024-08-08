using UnityEngine;

public class CursorController
{
    private CursorController()
    {

    }

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
