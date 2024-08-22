using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuToggleController : MonoBehaviour
{
    // フォーカスを移動させたいToggleのリスト
    [SerializeField]
    private List<Toggle> toggleList; 
    public List<Toggle> ToggleList { get { return toggleList; } set { toggleList = value; } }
    private int currentToggleIndex = 0;

    public void ToggleYUpdate()
    {
        if (InputManager.UpButton())
        {
            currentToggleIndex--;
            if (currentToggleIndex < 0)
            {
                currentToggleIndex = 0;
            }
            toggleList[currentToggleIndex].isOn = !toggleList[currentToggleIndex].isOn;
            EventSystem.current.SetSelectedGameObject(toggleList[currentToggleIndex].gameObject);
        }
        else if (InputManager.DownButton())
        {
            currentToggleIndex++;
            if (currentToggleIndex >= toggleList.Count)
            {
                currentToggleIndex = toggleList.Count - 1;
            }
            toggleList[currentToggleIndex].isOn = !toggleList[currentToggleIndex].isOn;
            EventSystem.current.SetSelectedGameObject(toggleList[currentToggleIndex].gameObject);
        }
    }

    public void ToggleXUpdate()
    {
        if (InputManager.LeftButton())
        {
            currentToggleIndex--;
            if (currentToggleIndex < 0)
            {
                currentToggleIndex = 0;
            }
            toggleList[currentToggleIndex].isOn = !toggleList[currentToggleIndex].isOn;
        }
        else if (InputManager.RightButton())
        {
            currentToggleIndex++;
            if (currentToggleIndex >= toggleList.Count)
            {
                currentToggleIndex = toggleList.Count - 1;
            }
            toggleList[currentToggleIndex].isOn = !toggleList[currentToggleIndex].isOn;
        }
    }
}
