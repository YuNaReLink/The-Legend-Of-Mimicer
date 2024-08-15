using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    /// <summary>
    /// プレイヤーHPUIを管理するクラス
    /// </summary>
    private PlayerConnectionUI playerUIController = null;
    public PlayerConnectionUI GetPlayerUIController() { return playerUIController; }
    private void Awake()
    {
        playerUIController = GetComponentInChildren<PlayerConnectionUI>();
        if(playerUIController != null)
        {
            playerUIController.Initilaize(this);
        }
    }

    private void Start()
    {
        GameManager.MenuEnabled = false;
    }

    void Update()
    {
        playerUIController.PlayerUIUpdate();
    }
}
