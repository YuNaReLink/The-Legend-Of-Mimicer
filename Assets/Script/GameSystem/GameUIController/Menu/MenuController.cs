using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private List<Toggle> menuButtonToggle = new List<Toggle>();

    [SerializeField]
    private List<GameObject> menuInsideList = new List<GameObject>();

    private ItemManager itemManager = null;
    public ItemManager GetItemManager() {  return itemManager; }
    
    public void AwakeInitialize()
    {
        //子オブジェクトを取得処理
        int childCount = transform.childCount;
        GameObject g = null;
        //子があるなら
        if(childCount > 0)
        {
            //MenuControllerの子オブジェクトを取得
            for(int i = 0; i < childCount; i++)
            {
                g = transform.GetChild(i).gameObject;
                menuControllerChildList.Add(g);
                //取得した子オブジェクトにさらに子オブジェクトがあるなら
                int count = menuControllerChildList[i].transform.childCount;
                if(count > 0)
                {
                    List<GameObject> list = new List<GameObject>();
                    //子オブジェクトのカウントによって取得するListを変更
                    switch (i)
                    {
                        case 0:
                            list = menuButtonList;
                            break;
                        case 1:
                            list = menuInsideList;
                            break;
                    }
                    //子オブジェクトの子オブジェクトを取得
                    GameObject cg = null;
                    for(int j = 0; j < count; j++)
                    {
                        cg = menuControllerChildList[i].transform.GetChild(j).gameObject;
                        list.Add(cg);
                    }
                }
            }
        }
        g = null;
        Toggle toggle = null;
        for(int i = 0;i < menuButtonList.Count; i++)
        {
            g = menuButtonList[i];
            toggle = g.GetComponent<Toggle>();
            if(toggle != null)
            {
                menuButtonToggle.Add(toggle);
            }
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
        menuButtonToggle[(int)MenuField.Inventory].isOn = true;
        for (int i = 1; i < menuButtonToggle.Count; i++)
        {
            menuButtonToggle[i].isOn = false;
        }
    }

    public void MenuUpdate()
    {
        if (itemManager != null)
        {
            itemManager.ItemManagerUpdate();
            itemManager.GetItemUpdate();
        }

        for (int i = 0; i < menuButtonToggle.Count; i++)
        {
            if (menuButtonToggle[i].isOn)
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

    public void StatesButtonClick()
    {
        menuButtonToggle[(int)MenuField.Inventory].isOn = true;
    }

    public void OptionButtonClick()
    {
        menuButtonToggle[(int)MenuField.Option].isOn = true;
    }
}
