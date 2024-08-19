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
    }

    private void Start()
    {
        playerUIController.StartInitialize();

        gameOverUIController.StartInitialize();
    }

    void Update()
    {
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
                break;
        }
    }

    private void GameOverUIStartCheck()
    {
        if (!gameOverUIController.gameObject.activeSelf)
        {
            PlayerController controller = playerUIController.GetPlayerController();
            Animator animator =controller.GetAnimator();
            AnimatorStateInfo animInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (!animInfo.IsName("death")|| animInfo.normalizedTime < 1.0f) { return; }
            gameOverUIController.gameObject.SetActive(true);
        }
        else
        {
            gameOverUIController.GameOverUIUpdate();
        }
    }
}
