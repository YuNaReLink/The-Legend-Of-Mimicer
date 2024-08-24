using UnityEngine;
using System.Collections;

public class HitStopManager : MonoBehaviour
{
    // どこからでも呼び出せるようにする
    public static HitStopManager instance;

    private bool hitStop = false;
    public bool IsHitStop() { return hitStop; }

    private void Start()
    {
        instance = this;
    }

    // ヒットストップを開始する関数
    public void StartHitStop(float duration)
    {
        instance.StartCoroutine(instance.HitStopCoroutine(duration));
    }

    // コルーチンの内容
    private IEnumerator HitStopCoroutine(float duration)
    {
        hitStop = true;
        // ヒットストップの開始
        Time.timeScale = 0f;

        // 指定した時間だけ停止
        yield return new WaitForSecondsRealtime(duration);
        hitStop = false;
        // ヒットストップの終了
        Time.timeScale = 1f;
    }
}
