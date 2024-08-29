using UnityEngine;

public class GameUIController : MonoBehaviour
{
    /// <summary>
    /// �v���C���[�Ɋ֌W����UI���Ǘ�����N���X
    /// </summary>
    private PlayerConnectionUI          playerUIController = null;
    public PlayerConnectionUI           GetPlayerUIController() { return playerUIController; }

    /// <summary>
    /// �Q�[���I�[�o�[UI���Ǘ�����N���X
    /// </summary>
    private GameOverUIController        gameOverUIController = null;

    /// <summary>
    /// �Q�[���N���AUI���Ǘ�����N���X
    /// </summary>
    private GameClearUIController       gameClearUIController = null;
    /// <summary>
    /// UI�̌��ʉ����Ǘ�����N���X
    /// </summary>
    private CanvasSoundController       canvasSoundController = null;
    public CanvasSoundController        GetCanvasSoundController() { return canvasSoundController; }
    /// <summary>
    /// �L�[���͂̌��ʉ����Ǘ�����N���X
    /// </summary>
    private SoundController             keySoundController = null;
    public SoundController              GetKeySoundController() {return keySoundController; }

    private void Awake()
    {
        playerUIController = GetComponentInChildren<PlayerConnectionUI>();
        if(playerUIController != null)
        {
            playerUIController.AwakeInitilaize(this);
        }
        else
        {
            Debug.LogError("PlayerConnectionUI���A�^�b�`����Ă��܂���");
        }
        gameOverUIController = GetComponentInChildren<GameOverUIController>();
        if(gameOverUIController != null)
        {
            gameOverUIController.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("GameOverUIController���A�^�b�`����Ă��܂���");
        }
        gameClearUIController = GetComponentInChildren<GameClearUIController>();
        if (gameClearUIController != null)
        {
            gameClearUIController.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("GameClearUIController���A�^�b�`����Ă��܂���");
        }
        canvasSoundController = GetComponent<CanvasSoundController>();
        if(canvasSoundController  != null)
        {
            canvasSoundController.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("CanvasSoundController���A�^�b�`����Ă��܂���");
        }
        keySoundController = playerUIController.GetKeyInputUIController().SelfObject().GetComponentInChildren<SoundController>();
        if(keySoundController != null)
        {
            keySoundController.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("SoundController���A�^�b�`����Ă��܂���");
        }
    }

    private void Start()
    {
        playerUIController.StartInitialize();

        gameOverUIController.StartInitialize();

        gameClearUIController.StartInitialize();
    }

    void Update()
    {
        //�Q�[���̏�Ԃɉe������Ȃ�UI�̏���
        playerUIController.AllGameSceneUpdatePlayerUI();
        //�ȉ��̓Q�[���̏�Ԃɂ���ČĂяo�����UI����
        switch (GameManager.GameState)
        {
            case GameManager.GameStateEnum.Game:
                playerUIController.GamingEnabledUIUpdate();
                break;
            case GameManager.GameStateEnum.Pose:
                playerUIController.PoseingEnabledUIUpdate();
                break;
            case GameManager.GameStateEnum.GameOver:
                GameOverUIStartCheck();
                playerUIController.EndGameUIUpdate();
                break;
            case GameManager.GameStateEnum.GameClear:
                GameClearUIStartCheck();
                playerUIController.EndGameUIUpdate();
                break;
        }
    }

    private void GameOverUIStartCheck()
    {
        if (!gameOverUIController.IsActiveObject())
        {
            PlayerController controller = playerUIController.GetPlayerController();
            Animator animator =controller.GetAnimator();
            AnimatorStateInfo animInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (!animInfo.IsName("death")|| animInfo.normalizedTime < 1.0f) { return; }
            gameOverUIController.SetActiveObject(true);
        }
        else
        {
            gameOverUIController.GameOverUIUpdate();
        }
    }

    private void GameClearUIStartCheck()
    {
        if (!gameClearUIController.IsActiveObject())
        {
            if(GameSceneSystemController.Instance.GetCameraFocusObject() != null) { return; }
            gameClearUIController.SetActiveObject(true);
        }
        else
        {
            gameClearUIController.GameClearUIUpdate();
        }
    }
}
