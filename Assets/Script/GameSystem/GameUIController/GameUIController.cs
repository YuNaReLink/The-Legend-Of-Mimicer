using UnityEngine;

public class GameUIController : MonoBehaviour
{
    /// <summary>
    /// プレイヤーに関係するUIを管理するクラス
    /// </summary>
    private PlayerConnectionUI          playerUIController = null;
    public PlayerConnectionUI           GetPlayerUIController() { return playerUIController; }

    /// <summary>
    /// ゲームオーバーUIを管理するクラス
    /// </summary>
    private GameOverUIController        gameOverUIController = null;

    /// <summary>
    /// ゲームクリアUIを管理するクラス
    /// </summary>
    private GameClearUIController       gameClearUIController = null;
    /// <summary>
    /// UIの効果音を管理するクラス
    /// </summary>
    private CanvasSoundController       canvasSoundController = null;
    public CanvasSoundController        GetCanvasSoundController() { return canvasSoundController; }
    /// <summary>
    /// キー入力の効果音を管理するクラス
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
            Debug.LogError("PlayerConnectionUIがアタッチされていません");
        }
        gameOverUIController = GetComponentInChildren<GameOverUIController>();
        if(gameOverUIController != null)
        {
            gameOverUIController.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("GameOverUIControllerがアタッチされていません");
        }
        gameClearUIController = GetComponentInChildren<GameClearUIController>();
        if (gameClearUIController != null)
        {
            gameClearUIController.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("GameClearUIControllerがアタッチされていません");
        }
        canvasSoundController = GetComponent<CanvasSoundController>();
        if(canvasSoundController  != null)
        {
            canvasSoundController.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("CanvasSoundControllerがアタッチされていません");
        }
        keySoundController = playerUIController.GetKeyInputUIController().SelfObject().GetComponentInChildren<SoundController>();
        if(keySoundController != null)
        {
            keySoundController.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("SoundControllerがアタッチされていません");
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
        //ゲームの状態に影響されないUIの処理
        playerUIController.AllGameSceneUpdatePlayerUI();
        //以下はゲームの状態によって呼び出されるUI処理
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
