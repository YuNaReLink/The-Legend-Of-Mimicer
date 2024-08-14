using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConnectionUI : MonoBehaviour
{
    private GameUIController gameUIController = null;
    public GameUIController GetGameUIController() { return gameUIController; }

    private PlayerController playerController = null;
    public PlayerController GetPlayerController() { return playerController; }

    private PlayerLifeUI playerLifeUI = null;
    public PlayerLifeUI GetPlayerLifeUI() { return playerLifeUI; }

    private KeyInputUIController keyInputUIController = null;
    public KeyInputUIController GetKeyInputUIController() { return keyInputUIController; }

    private TargetLockController targetLockController = null;

    public void Initilaize(GameUIController _gameUIController)
    {
        gameUIController = _gameUIController;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }

        playerLifeUI = GetComponentInChildren<PlayerLifeUI>();
        if(playerLifeUI != null)
        {
            playerLifeUI.Initilaize(this);
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
    }

    public void PlayerUIUpdate()
    {
        playerLifeUI.LifeUpdate(playerController.HP);

        keyInputUIController.KeyUIInputUpdate();

        targetLockController.LockUIActiveCheck();
    }
}
