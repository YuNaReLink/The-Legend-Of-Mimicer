using System.Collections.Generic;
using UnityEngine;

public class PlayerConnectionUI : MonoBehaviour
{
    //ゲーム全体のUIのクラス
    private GameUIController gameUIController = null;
    public GameUIController GetGameUIController() { return gameUIController; }

    private PlayerController playerController = null;
    public PlayerController GetPlayerController() { return playerController; }
    /// <summary>
    /// PlayerConnectionUIの子のオブジェクトを判断するためのenum 
    /// </summary>
    private enum PlayerUINumber
    {
        Life,
        ArrowCount,
        KeyInput,
        Target,
        Menu
    }
    /// <summary>
    /// 子オブジェクトのUIをまとめて取得するためのList
    /// </summary>
    private List<GameObject> playerUIList = new List<GameObject>();
    /// <summary>
    /// プレイヤーの体力のUIを管理するクラス
    /// </summary>
    private PlayerLifeUI playerLifeUI = null;
    public PlayerLifeUI GetPlayerLifeUI() { return playerLifeUI; }
    /// <summary>
    /// プレイヤーが持つクロスボウの矢の数のUIを管理するクラス
    /// </summary>
    private ArrowCount arrowCount = null;
    /// <summary>
    /// プレイヤーが入力するキーのUIを管理するクラス
    /// </summary>
    private KeyInputUIController keyInputUIController = null;
    public KeyInputUIController GetKeyInputUIController() { return keyInputUIController; }
    /// <summary>
    /// 注目したときにターゲットに表示させるUIの管理をするクラス
    /// </summary>
    private TargetLockController targetLockController = null;
    /// <summary>
    /// Tabキーを押したときに表示するメニューの管理をするクラス
    /// </summary>
    private MenuActiveController menuActiveController = null;

    //GameUIControllerのAwakeに記述する関数
    public void AwakeInitilaize(GameUIController _gameUIController)
    {
        gameUIController = _gameUIController;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        GameObject g = null;
        for(int i = 0;i < transform.childCount; i++)
        {
            g = transform.GetChild(i).gameObject;
            playerUIList.Add(g);
        }

        playerLifeUI = GetComponentInChildren<PlayerLifeUI>();
        if(playerLifeUI != null)
        {
            playerLifeUI.Initilaize(this);
        }

        arrowCount = GetComponentInChildren<ArrowCount>();
        if(arrowCount != null)
        {
            arrowCount.AwakeInitilaize(playerController);
        }

        keyInputUIController = GetComponentInChildren<KeyInputUIController>();
        if(keyInputUIController != null)
        {
            keyInputUIController.Initialize();
        }

        targetLockController = GetComponentInChildren<TargetLockController>();
        if(targetLockController != null)
        {
            targetLockController.Initialize(this);
        }

        menuActiveController = GetComponentInChildren<MenuActiveController>();
        if(menuActiveController != null)
        {
            menuActiveController.AwakeInitialize();
        }
    }
    //GameUIControllerのStartに記述する関数
    public void StartInitialize()
    {
        if (menuActiveController != null)
        {
            menuActiveController.StartInitialize();
        }
    }
    //GameUIControllerのUpdateに記述する関数
    public void PlayerUIUpdate()
    {
        playerLifeUI.LifeUpdate(playerController.HP);

        if (playerController.GetCameraController().IsFPSMode()&&playerController.GetToolController().GetQuiver() != null)
        {
            if (!playerUIList[(int)PlayerUINumber.ArrowCount].activeSelf)
            {
                playerUIList[(int)PlayerUINumber.ArrowCount].SetActive(true);
            }
            arrowCount.ArrowCountUpdate();
        }
        else
        {
            if (playerUIList[(int)PlayerUINumber.ArrowCount].activeSelf)
            {
                playerUIList[(int)PlayerUINumber.ArrowCount].SetActive(false);
            }
        }

        keyInputUIController.KeyUIInputUpdate();

        targetLockController.LockUIActiveCheck();

        menuActiveController.MenuActiveUpdate();
    }
}
