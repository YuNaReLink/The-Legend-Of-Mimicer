using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController
{
    /// <summary>
    /// �J�E���g�_�E���N���X
    /// </summary>
    protected InterfaceCountDown[] updateCountDowns;

    public virtual void InitializeAssignTimer() { }

    public void TimerUpdate()
    {
        foreach (var countdown in updateCountDowns)
        {
            if (countdown.IsEnabled())
            {
                countdown.Update();
            }
        }
    }
}
