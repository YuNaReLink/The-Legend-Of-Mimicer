using UnityEngine;
using System.Collections;

/// <summary>
/// ヒットストップを行うシングルトンクラス
/// シーンのヒエラルキーのオブジェクトにアタッチして使用
/// </summary>
public class HitStopManager : MonoBehaviour
{
    // どこからでも呼び出せるようにする
    public static HitStopManager    instance;

    private bool                    hitStop = false;
    public bool                     IsHitStop() { return hitStop; }

    private const float             hitStopCount = 0.5f;

    public float                    HitStopCount => hitStopCount;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
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
        Time.timeScale = 0.1f;

        // 指定した時間だけ停止
        yield return new WaitForSecondsRealtime(duration);
        hitStop = false;
        // ヒットストップの終了
        Time.timeScale = 1f;
    }
}
