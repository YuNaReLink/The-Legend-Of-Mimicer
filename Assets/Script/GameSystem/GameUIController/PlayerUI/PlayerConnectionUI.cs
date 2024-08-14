using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConnectionUI : MonoBehaviour
{
    private PlayerController playerController = null;
    public PlayerController GetPlayerController() { return playerController; }

    private PlayerLifeUI playerLifeUI = null;
    public PlayerLifeUI GetPlayerLifeUI() { return playerLifeUI; }

    private KeyInputUIController keyInputUIController = null;
    public KeyInputUIController GetKeyInputUIController() { return keyInputUIController; }

    public void Initilaize()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }

        playerLifeUI = GetComponentInChildren<PlayerLifeUI>();
        if(playerLifeUI != null)
        {
            playerLifeUI.Initilaize();
        }

        keyInputUIController = GetComponentInChildren<KeyInputUIController>();
        if(keyInputUIController != null)
        {
            keyInputUIController.Initialize();
        }
    }

    public void PlayerUIUpdate()
    {
        playerLifeUI.LifeUpdate(playerController.HP);
        keyInputUIController.KeyUIInputUpdate();
    }
}
