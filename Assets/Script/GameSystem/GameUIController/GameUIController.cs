using UnityEngine;

public class GameUIController : MonoBehaviour
{
    /// <summary>
    /// プレイヤーHPUIを管理するクラス
    /// </summary>
    private PlayerConnectionUI playerUIController = null;
    public PlayerConnectionUI GetPlayerUIController() { return playerUIController; }

    /// <summary>
    /// ゲームオーバーUIを管理するクラス
    /// </summary>
    private GameOverUIController gameOverUIController = null;

    /// <summary>
    /// ゲームクリアUIを管理するクラス
    /// </summary>
    private GameClearUIController gameClearUIController = null;
    [SerializeField]
    private CanvasSoundController canvasSoundController = null;
    public CanvasSoundController GetCanvasSoundController() { return canvasSoundController; }
    [SerializeField]
    private SoundController keySoundController = null;
    public SoundController GetKeySoundController() {return keySoundController; }

    private void Awake()
    {
        playerUIController = GetComponentInChildren<PlayerConnectionUI>();
        if(playerUIController != null)
        {
            playerUIController.AwakeInitilaize(this);
        }
        gameOverUIController = GetComponentInChildren<GameOverUIController>();
        if(gameOverUIController != null)
        {
            gameOverUIController.AwakeInitilaize();
        }
        gameClearUIController = GetComponentInChildren<GameClearUIController>();
        if (gameClearUIController != null)
        {
            gameClearUIController.AwakeInitilaize();
        }
        canvasSoundController = GetComponent<CanvasSoundController>();
        if(canvasSoundController  != null)
        {
            canvasSoundController.AwakeInitilaize();
        }
        keySoundController = playerUIController.GetKeyInputUIController().SelfObject().GetComponentInChildren<SoundController>();
        if(keySoundController != null)
        {
            keySoundController.AwakeInitilaize();
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
            case GameManager.GameStateEnum.Pose:
                playerUIController.PlayerUIUpdate();
                break;
            case GameManager.GameStateEnum.GameOver:
                GameOverUIStartCheck();
                break;
            case GameManager.GameStateEnum.GameClear:
                GameClearUIStartCheck();
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
            if(GameSceneSystemController.GetCameraFocusObject() != null) { return; }
            gameClearUIController.SetActiveObject(true);
        }
        else
        {
            gameClearUIController.GameClearUIUpdate();
        }
    }
}
