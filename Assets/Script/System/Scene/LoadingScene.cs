using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 非同期でローディング処理を行うクラス
/// </summary>
public class LoadingScene : MonoBehaviour
{
    [SerializeField] 
    private GameObject _loadingUI;
    [SerializeField] 
    private Slider _slider;
    /// <summary>
    /// シーンをロードする関数
    /// </summary>
    /// <param name="sceneName">
    /// ロードするシーン名を代入
    /// </param>
    public void LoadNextScene(string sceneName)
    {
        _loadingUI.SetActive(true);
        StartCoroutine(LoadScene(sceneName));
    }
    /// <summary>
    /// コルーチンを使って非同期処理をする
    /// </summary>
    /// <param name="sceneName">
    /// ロードするシーン名
    /// </param>
    /// <returns></returns>
    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            _slider.value = async.progress;
            yield return null;
        }
    }
}
