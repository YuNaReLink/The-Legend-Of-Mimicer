using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    /// <summary>
    /// �v���C���[HPUI���Ǘ�����N���X
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
