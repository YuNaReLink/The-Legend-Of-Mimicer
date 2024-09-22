using UnityEngine;
using System.Collections;

/// <summary>
/// ヒットスローを行うシングルトンクラス
/// シーンのヒエラルキーのオブジェクトにアタッチして使用
/// </summary>
public class HitThrowManager : MonoBehaviour
{
    // どこからでも呼び出せるようにする
    public static HitThrowManager    instance;

    private bool                    hitStop = false;
    public bool                     IsHitThrow() { return hitStop; }

    private const float             HitStopCount = 0.5f;

    public float                    GetHitStopCount => HitStopCount;

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
        instance.StartCoroutine(instance.HitThrowCoroutine(duration));
    }

    // コルーチンの内容
    private IEnumerator HitThrowCoroutine(float duration)
    {
        hitStop = true;
        // ヒットスローの開始
        Time.timeScale = 0.1f;
        // 指定した時間だけ停止
        yield return new WaitForSecondsRealtime(duration);
        hitStop = false;
        // ヒットスローの終了
        Time.timeScale = 1f;
    }
}
