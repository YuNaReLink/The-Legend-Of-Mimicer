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
    [SerializeField]
    private MenuToggleController menuToggleController = null;
    [SerializeField]
    private MenuButtonController menuButtonController = null;

    private int currentHorizontalIndex = 0;


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
        }

        if (menuInsideList[(int)MenuField.Inventory] != null)
        {
            itemManager = menuInsideList[(int)MenuField.Inventory].GetComponent<ItemManager>();
            if(itemManager != null)
            {
                itemManager.AwakeInitialize();
            }
        }

    }

    public void StartInitialize()
    {
        if (itemManager != null)
        {
            itemManager.StartInitialize();
        }

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
        if (itemManager != null)
        {
            itemManager.ItemManagerUpdate();
            itemManager.GetItemUpdate();
        }

        if(currentHorizontalIndex <= 0)
        {
            menuToggleController.ToggleYUpdate();

            for (int i = 0; i < menuToggleController.ToggleList.Count; i++)
            {
                if (menuToggleController.ToggleList[i].isOn)
                {
                    if (!menuInsideList[i].activeSelf)
                    {
                        menuInsideList[i].SetActive(true);
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
        }
        else if(currentHorizontalIndex > 0)
        {
            if (menuInsideList[(int)MenuField.Inventory].activeSelf)
            {
                //�C���x���g�����̃{�^������
                itemManager.GetItemToggleController().ToggleXUpdate();
            }
            else if (menuInsideList[(int)MenuField.Option].activeSelf)
            {
                //�I�v�V�������̃{�^������
                menuButtonController.ButtonUpdate();
            }
        }

        if (InputManager.ActionButton()||Input.GetKeyDown(KeyCode.Escape))
        {
            currentHorizontalIndex = 0;
        }
        else if (InputManager.GetItemButton())
        {
            currentHorizontalIndex = 1;
        }
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
