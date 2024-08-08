using System;
using UnityEngine;

//UnityのTime.deltaTimeを使ったカウントダウンのクラス
//CountDownとは違いフレームではなくタイマー方式でカウントする
public class DeltaTimeCountDown : InterfaceCountDown
{

    public event Action OnCompleted;

    private float count = 0;

    private float initCount = 0;

    public float GetInitCount() { return initCount; }

    public void End()
    {
        count = 0;
        OnCompleted?.Invoke();
        OnCompleted = null;
    }
    //カウントが有効かどうか
    public bool IsEnabled() { return count > 0; }

    public bool MaxCount(float _count) { return count >= _count; }

    public void StartTimer(float _count)
    {
        count = _count;
        initCount = count;
    }

    // Update is called once per frame
    public void Update()
    {
        count -= Time.deltaTime;
        if (count <= 0)
        {
            initCount = 0;
            End();
        }
    }
}
