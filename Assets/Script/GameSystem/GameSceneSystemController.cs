using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneSystemController : MonoBehaviour
{
    private static PlayerController playerController = null;

    [SerializeField]
    private GameObject gameClearTarget = null;

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
    private static TriggerTag keyTriggerTag = TriggerTag.Null;

    public static TriggerTag KeyTriggerTag {  get { return keyTriggerTag; }set { keyTriggerTag = value; } }

    /// <summary>
    /// カーソルを表示・非表示を管理するクラス
    /// </summary>
    CursorController cursor = null;

    private static DeltaTimeCountDown gameOverStartTimer = null;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        cursor = CursorController.GetInstance();

        gameOverStartTimer = new DeltaTimeCountDown();
    }

    private void Start()
    {
        cursor.SetCursorLookMode(CursorLockMode.Locked);
        cursor.SetCursorState(false);

        GameManager.GameState = GameManager.GameStateEnum.Game;
    }

    private void Update()
    {
        gameOverStartTimer.Update();

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
            cursor.SetCursorLookMode(CursorLockMode.None);
            cursor.SetCursorState(true);
            Time.timeScale = 0;
        }
        else
        {
            cursor.SetCursorLookMode(CursorLockMode.Locked);
            cursor.SetCursorState(false);
            Time.timeScale = 1f;
        }

        GameResultCheck();

    }

    private void GameResultCheck()
    {
        if (!playerController.DeathFlag&&gameClearTarget!=null) { return; }
        if(gameClearTarget == null)
        {
            GameManager.GameState = GameManager.GameStateEnum.GameClear;
        }
        else if (playerController.DeathFlag)
        {
            GameManager.GameState = GameManager.GameStateEnum.GameOver;
        }
        cursor.SetCursorLookMode(CursorLockMode.None);
        cursor.SetCursorState(true);
    }
}
