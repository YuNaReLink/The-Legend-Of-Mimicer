using UnityEngine;

/// <summary>
/// �^�C�g���V�[���S�̂ōs���������Ǘ�����N���X
/// </summary>
public class TitleSceneController : MonoBehaviour
{
    private TitleSoundController soundController = null;

    private void Awake()
    {
        soundController = GetComponent<TitleSoundController>();
        if (soundController != null)
        {
            soundController.AwakeInitilaize();
        }
    }
    void Start()
    {
        GameManager.GameState = GameManager.GameStateEnum.Title;
        Time.timeScale = 1f;
        CursorController.GetInstance().SettingCursor(CursorLockMode.None, true);
        soundController.PlayBGM((int)TitleSoundController.TitleBGMTag.Title);
    }

    private void Update()
    {
        soundController.TitleSoundUpdate();
    }
}
