using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// �񓯊��Ń��[�f�B���O�������s���N���X
/// </summary>
public class LoadingScene : MonoBehaviour
{
    [SerializeField] 
    private GameObject _loadingUI;
    [SerializeField] 
    private Slider _slider;
    /// <summary>
    /// �V�[�������[�h����֐�
    /// </summary>
    /// <param name="sceneName">
    /// ���[�h����V�[��������
    /// </param>
    public void LoadNextScene(string sceneName)
    {
        _loadingUI.SetActive(true);
        StartCoroutine(LoadScene(sceneName));
    }
    /// <summary>
    /// �R���[�`�����g���Ĕ񓯊�����������
    /// </summary>
    /// <param name="sceneName">
    /// ���[�h����V�[����
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
