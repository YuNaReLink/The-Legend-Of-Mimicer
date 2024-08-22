using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour
{
    [SerializeField]
    private List<Button> buttonList = new List<Button>();

    private int currentButtonIndex = 0;

    public List<Button> ButtonList { get { return buttonList; } set { buttonList = value; } }

    public void ButtonUpdate()
    {
        if (InputManager.UpButton())
        {
            currentButtonIndex--;
            if (currentButtonIndex < 0)
            {
                currentButtonIndex = 0;
            }
            EventSystem.current.SetSelectedGameObject(buttonList[currentButtonIndex].gameObject);
        }
        else if (InputManager.DownButton())
        {
            currentButtonIndex++;
            if (currentButtonIndex >= buttonList.Count)
            {
                currentButtonIndex = buttonList.Count - 1;
            }
            EventSystem.current.SetSelectedGameObject(buttonList[currentButtonIndex].gameObject);
        }
        if (InputManager.GetItemButton())
        {
            buttonList[currentButtonIndex].onClick.Invoke();
        }
    }
}
