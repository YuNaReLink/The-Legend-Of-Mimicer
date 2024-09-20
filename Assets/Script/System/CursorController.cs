using UnityEngine;

/// <summary>
/// カーソルの表示、非表示のなどの設定を行うシングルトンクラス
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

    public void SettingCursor(CursorLockMode mode,bool enabled)
    {
        if(Cursor.lockState != mode)
        {
            Cursor.lockState = mode;
        }
        if(Cursor.visible != enabled)
        {
            Cursor.visible = enabled;
        }
    }
}
