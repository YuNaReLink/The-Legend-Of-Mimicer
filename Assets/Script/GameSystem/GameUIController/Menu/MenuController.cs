using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> menuControllerChildList = new List<GameObject>();
    private enum MenuField
    {
        Inventory,
        Option
    }
    [SerializeField]
    private List<GameObject> menuButtonList = new List<GameObject>();

    [SerializeField]
    private List<GameObject> menuInsideList = new List<GameObject>();

    private ItemManager itemManager = null;
    public ItemManager GetItemManager() {  return itemManager; }

    private MouseSpeedSetting mouseSpeedSetting = null;
    [SerializeField]
    private MenuToggleController menuToggleController = null;

    private Toggle mouseDesideToggle = null;

    [SerializeField]
    private MenuButtonController menuButtonController = null;

    private int currentHorizontalIndex = 0;

    private GameUIController gameUIController = null;

    public void AwakeInitialize()
    {
        //�q�I�u�W�F�N�g���擾����
        int childCount = transform.childCount;
        GameObject g = null;
        //�q������Ȃ�
        if(childCount > 0)
        {
            //MenuController�̎q�I�u�W�F�N�g���擾
            for(int i = 0; i < childCount; i++)
            {
                g = transform.GetChild(i).gameObject;
                menuControllerChildList.Add(g);
                //�擾�����q�I�u�W�F�N�g�ɂ���Ɏq�I�u�W�F�N�g������Ȃ�
                int count = menuControllerChildList[i].transform.childCount;
                if(count > 0)
                {
                    List<GameObject> list = new List<GameObject>();
                    //�q�I�u�W�F�N�g�̃J�E���g�ɂ���Ď擾����List��ύX
                    switch (i)
                    {
                        case 0:
                            list = menuButtonList;
                            break;
                        case 1:
                            list = menuInsideList;
                            break;
                    }
                    //�q�I�u�W�F�N�g�̎q�I�u�W�F�N�g���擾
                    GameObject cg = null;
                    for(int j = 0; j < count; j++)
                    {
                        cg = menuControllerChildList[i].transform.GetChild(j).gameObject;
                        list.Add(cg);
                    }
                }
            }
        }
        menuToggleController = GetComponent<MenuToggleController>();
        if(menuToggleController != null)
        {
            g = null;
            Toggle toggle = null;
            List<Toggle> menuButtonToggle = new List<Toggle>();
            for(int i = 0;i < menuButtonList.Count; i++)
            {
                g = menuButtonList[i];
                toggle = g.GetComponent<Toggle>();
                if(toggle != null)
                {
                    menuButtonToggle.Add(toggle);
                }
            }
            menuToggleController.ToggleList = menuButtonToggle;
            menuToggleController.AwakeInitilaize();
        }

        menuButtonController = GetComponent<MenuButtonController>();
        if(menuButtonController != null)
        {
            g = null;
            Button button = null;
            List<Button> buttonList = new List<Button>();
            for(int i = 0; i < menuInsideList[(int)MenuField.Option].transform.childCount; i++)
            {
                g = menuInsideList[(int)MenuField.Option].transform.GetChild(i).gameObject;
                button = g.GetComponent<Button>();
                if(button != null)
                {
                    buttonList.Add(button);
                }
            }
            menuButtonController.ButtonList = buttonList;
            menuButtonController.AwakeInitilaize();
        }

        if (menuInsideList[(int)MenuField.Inventory] != null)
        {
            itemManager = menuInsideList[(int)MenuField.Inventory].GetComponent<ItemManager>();
            if(itemManager != null)
            {
                itemManager.AwakeInitialize();
            }
        }

        if (menuInsideList[(int)MenuField.Option] != null)
        {
            mouseSpeedSetting = menuInsideList[(int)MenuField.Option].GetComponentInChildren<MouseSpeedSetting>();
            if(mouseSpeedSetting == null)
            {
                Debug.LogError("MouseSpeedSetting���A�^�b�`����Ă��܂���");
            }
            mouseSpeedSetting?.AwakeInitilaize();
        }

        gameUIController = GetComponentInParent<GameUIController>();
    }

    public void StartInitialize()
    {
        itemManager?.StartInitialize();

        mouseSpeedSetting?.StartInitilaize();

        if (menuInsideList[(int)MenuField.Option].activeSelf)
        {
            menuInsideList[(int)MenuField.Option].SetActive(true);
        }
    }

    public void OpenMenuInitilaize()
    {
        // �ŏ���Toggle��I����Ԃɐݒ�
        menuToggleController.ToggleList[(int)MenuField.Inventory].isOn = !menuToggleController.ToggleList[(int)MenuField.Inventory].isOn;
        currentHorizontalIndex = 0;
    }

    public void MenuUpdate()
    {
        if(GameManager.GameState != GameManager.GameStateEnum.Pose) { return; }
        if (itemManager != null)
        {
            itemManager.ItemManagerUpdate();
            itemManager.GetItemUpdate();
        }

        if(currentHorizontalIndex <= 0)
        {
            menuToggleController.ToggleYUpdate();
            if (menuToggleController.ToggleIndexCheck())
            {
                gameUIController.GetCanvasSoundController().PlaySESound((int)CanvasSoundController.CanvasSoundTag.Select);
            }

            for (int i = 0; i < menuToggleController.ToggleList.Count; i++)
            {
                if (menuInsideList[i].activeSelf)
                {
                    menuInsideList[i].SetActive(false);
                }
                if (menuToggleController.ToggleList[i].isOn)
                {
                    if(menuToggleController.ToggleList[i] != mouseDesideToggle)
                    {
                        mouseDesideToggle = menuToggleController.ToggleList[i];
                        gameUIController.GetCanvasSoundController().PlaySESound((int)CanvasSoundController.CanvasSoundTag.Select);
                    }
                }
            }
        }
        else if(currentHorizontalIndex > 0)
        {
            for (int i = 0; i < menuToggleController.ToggleList.Count; i++)
            {
                if (menuToggleController.ToggleList[i].isOn)
                {
                    if (!menuInsideList[i].activeSelf)
                    {
                        menuInsideList[i].SetActive(true);
                    }
                    if (menuToggleController.ToggleList[i] != mouseDesideToggle)
                    {
                        mouseDesideToggle = menuToggleController.ToggleList[i];
                        gameUIController.GetCanvasSoundController().PlaySESound((int)CanvasSoundController.CanvasSoundTag.Select);
                    }
                }
                else
                {
                    if (menuInsideList[i].activeSelf)
                    {
                        menuInsideList[i].SetActive(false);
                    }
                }
            }
            if (menuInsideList[(int)MenuField.Inventory].activeSelf)
            {
                //�C���x���g�����̃{�^������
                itemManager.GetItemToggleController().ToggleXUpdate();
                if (itemManager.GetItemToggleController().ToggleIndexCheck())
                {
                    gameUIController.GetCanvasSoundController().PlaySESound((int)CanvasSoundController.CanvasSoundTag.Select);
                }
            }
            else if (menuInsideList[(int)MenuField.Option].activeSelf)
            {
                //�I�v�V�������̃{�^������
                menuButtonController.ButtonUpdate();
                if (menuButtonController.ButtonIndexCheck())
                {
                    gameUIController.GetCanvasSoundController().PlaySESound((int)CanvasSoundController.CanvasSoundTag.Select);
                }
                if (menuButtonController.DesideCheck())
                {
                    gameUIController.GetCanvasSoundController().PlaySESound((int)CanvasSoundController.CanvasSoundTag.Deside);
                }
            }
        }

        //���j���[���ڂ��ڍׂ�؂�ւ���
        //0:���j���[���ڃ{�^������
        //1:���j���[�ڍ׃{�^������
        if (InputManager.ActionButton()||Input.GetKeyDown(KeyCode.Escape))
        {
            currentHorizontalIndex = 0;
            gameUIController.GetCanvasSoundController().PlaySESound((int)CanvasSoundController.CanvasSoundTag.Cancel);
        }
        else if (InputManager.GetItemButton())
        {
            currentHorizontalIndex = 1;
            gameUIController.GetCanvasSoundController().PlaySESound((int)CanvasSoundController.CanvasSoundTag.Deside);
        }
    }

    public void SetHorizontalIndex(int index)
    {
        currentHorizontalIndex = index;
    }

    public void StatesButtonClick()
    {
        menuToggleController.ToggleList[(int)MenuField.Inventory].isOn = true;
    }

    public void OptionButtonClick()
    {
        menuToggleController.ToggleList[(int)MenuField.Option].isOn = true;
    }
}
