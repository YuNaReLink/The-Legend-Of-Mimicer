using System.Collections.Generic;
using UnityEngine;

public class PlayerConnectionUI : MonoBehaviour
{
    //�Q�[���S�̂�UI�̃N���X
    private GameUIController        gameUIController = null;
    public GameUIController         GetGameUIController() { return gameUIController; }

    private PlayerController        playerController = null;
    public PlayerController         GetPlayerController() { return playerController; }
    /// <summary>
    /// PlayerConnectionUI�̎q�̃I�u�W�F�N�g�𔻒f���邽�߂�enum 
    /// </summary>
    private enum PlayerUINumber
    {
        Life,
        ArrowCount,
        KeyInput,
        Target,
        Sight,
        Menu
    }
    /// <summary>
    /// �q�I�u�W�F�N�g��UI���܂Ƃ߂Ď擾���邽�߂�List
    /// </summary>
    private List<GameObject>        playerUIList = new List<GameObject>();
    /// <summary>
    /// �v���C���[�̗̑͂�UI���Ǘ�����N���X
    /// </summary>
    private PlayerLifeUI            playerLifeUI = null;
    public PlayerLifeUI             GetPlayerLifeUI() { return playerLifeUI; }
    /// <summary>
    /// �v���C���[�����N���X�{�E�̖�̐���UI���Ǘ�����N���X
    /// </summary>
    private ArrowCount              arrowCount = null;
    /// <summary>
    /// �v���C���[�����͂���L�[��UI���Ǘ�����N���X
    /// </summary>
    private KeyInputUIController    keyInputUIController = null;
    public KeyInputUIController     GetKeyInputUIController() { return keyInputUIController; }
    /// <summary>
    /// ���ڂ����Ƃ��Ƀ^�[�Q�b�g�ɕ\��������UI�̊Ǘ�������N���X
    /// </summary>
    private TargetLockController    targetLockController = null;
    /// <summary>
    /// Tab�L�[���������Ƃ��ɕ\�����郁�j���[�̊Ǘ�������N���X
    /// </summary>
    private MenuActiveController    menuActiveController = null;

    //GameUIController��Awake�ɋL�q����֐�
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
    //GameUIController��Start�ɋL�q����֐�
    public void StartInitialize()
    {
        if (menuActiveController != null)
        {
            menuActiveController.StartInitialize();
        }
    }
    public void AllGameSceneUpdatePlayerUI()
    {
        playerLifeUI.LifeUpdate(playerController.CharacterStatus.HP);
    }
    //GameUIController��Update�ɋL�q����֐�
    public void GamingEnabledUIUpdate()
    {
        //�N���X�{�E�Ŏg����̎c��̃J�E���g��\������UI�̏���
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
        keyInputUIController.ActiveKeyUI(true);
        //�L�[����UI�̏���
        keyInputUIController.KeyUIInputUpdate();
        //�^�[�Q�b�g�ɒ��ڂ������Ƀ^�[�Q�b�g�ɕ\������UI�̏���
        targetLockController.LockUIActiveCheck();
        //���j���[UI�̏���
        menuActiveController.MenuActiveUpdate();
    }
    public void PoseingEnabledUIUpdate()
    {
        //���j���[UI�̏���
        menuActiveController.MenuActiveUpdate();
        keyInputUIController.ActiveKeyUI(false);
        targetLockController.ActiveLockUI(false);
    }
    public void EndGameUIUpdate()
    {
        keyInputUIController.ActiveKeyUI(false);
        targetLockController.ActiveLockUI(false);
    }
}
