using UnityEngine;

public class GameSceneSystemController : MonoBehaviour
{
    private static GameSceneSystemController    instance;
    public static GameSceneSystemController     Instance => instance;

    private PlayerController                    playerController = null;
    public PlayerController                     PlayerController { get { return playerController; }set { playerController = value; } }

    private GameObject                          cameraFocusObject = null;
    public GameObject                           GetCameraFocusObject() { return cameraFocusObject; }

    private bool                                gameClearFlag = false;

    public void GameClearUpdate(GameObject o)
    {
        gameClearFlag = true;
        cameraFocusObject = o;
    }

    /// <summary>
    /// プレイヤーが操作可能なオブジェクトに当たっている時に
    /// 何に当たっているかを判別するためのenum
    /// </summary>
    public enum TriggerTag
    {
        Null,
        Door,
        Chest,
        Item
    }
    /// <summary>
    /// 上記の内容を保存するstatic型のインスタンス宣言
    /// </summary>
    private TriggerTag                          keyTriggerTag = TriggerTag.Null;

    public TriggerTag                           KeyTriggerTag {  get { return keyTriggerTag; }set { keyTriggerTag = value; } }

    private DeltaTimeCountDown                  gameOverStartTimer = null;

    private bool                                battleStart = false;
    public bool                                 BattleStart { get { return battleStart; } set { battleStart = value; } }

    private bool                                bossBattleStart = false;
    public bool                                 BossBattleStart { get { return bossBattleStart; }set { bossBattleStart = value; } }

    private GameSoundController                 gameSoundController = null;
    public GameSoundController                  GetGameSoundController() { return gameSoundController; }
    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if(player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }

        gameOverStartTimer = new DeltaTimeCountDown();

        gameSoundController = GetComponent<GameSoundController>();
        if(gameSoundController != null)
        {
            gameSoundController.AwakeInitilaize();
        }

        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        CursorController.GetInstance().SetCursorLookMode(CursorLockMode.Locked);
        CursorController.GetInstance().SetCursorState(false);
        gameClearFlag = false;
        bossBattleStart = false;
        GameManager.GameState = GameManager.GameStateEnum.Game;

        gameSoundController.PlayBGM((int)GameSoundController.GameBGMSoundTag.Stage);
    }

    private void Update()
    {
        if (HitStopManager.instance.IsHitStop()) { return; }
        gameOverStartTimer.Update();
        gameSoundController.GameSoundUpdate();
        if(CameraController.LockObject == null)
        {
            battleStart = false;
        }

        switch (GameManager.GameState)
        {
            case GameManager.GameStateEnum.Game:
            case GameManager.GameStateEnum.Pose:
                break;
            default:
                return;
        }

        if (GameManager.GameState == GameManager.GameStateEnum.Pose)
        {
            CursorController.GetInstance().SetCursorLookMode(CursorLockMode.None);
            CursorController.GetInstance().SetCursorState(true);
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0;
            }
        }
        else
        {
            CursorController.GetInstance().SetCursorLookMode(CursorLockMode.Locked);
            CursorController.GetInstance().SetCursorState(false);
            if(Time.timeScale <= 0)
            {
                Time.timeScale = 1f;
            }
        }

        GameResultCheck();

    }

    private void GameResultCheck()
    {
        if (!playerController.DeathFlag&& !gameClearFlag) { return; }
        if(gameClearFlag)
        {
            GameManager.GameState = GameManager.GameStateEnum.GameClear;
        }
        else if (playerController.DeathFlag)
        {
            GameManager.GameState = GameManager.GameStateEnum.GameOver;
        }
        CursorController.GetInstance().SetCursorLookMode(CursorLockMode.None);
        CursorController.GetInstance().SetCursorState(true);
    }
}
