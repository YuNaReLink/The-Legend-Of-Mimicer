using UnityEngine;

/// <summary>
/// タイマーの基底クラス
/// このクラスを基底に継承先で
/// タイマーをまとめて処理を行う
/// </summary>
public class TimerController
{
    /// <summary>
    /// カウントダウンクラス
    /// </summary>
    protected InterfaceCountDown[] updateCountDowns;

    public virtual void InitializeAssignTimer() { }

    public void TimerUpdate()
    {
        if (Time.timeScale <= 0) { return; }
        foreach (var countdown in updateCountDowns)
        {
            if (countdown.IsEnabled())
            {
                countdown.Update();
            }
        }
    }
}
