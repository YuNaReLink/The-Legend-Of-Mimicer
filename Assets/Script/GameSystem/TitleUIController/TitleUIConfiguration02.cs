using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleUIConfiguration02 : BaseTitleUIConfiguration
{

    private MenuButtonController menuButtonController = null;
    public override void Initilaize()
    {
        base.Initilaize();
        menuButtonController = GetComponent<MenuButtonController>();
        if(menuButtonController != null)
        {
            List<Button> buttonList = new List<Button>();
            for (int i = 0; i < uiObjectArray.Count; i++)
            {
                Button button = uiObjectArray[i].GetComponent<Button>();
                if(button != null)
                {
                    buttonList.Add(button);
                }
            }
            menuButtonController.ButtonList = buttonList;
        }
        menuButtonController.AwakeInitilaize();
    }

    private void Update()
    {
        menuButtonController.ButtonUpdate();
        if (menuButtonController.ButtonIndexCheck())
        {
            titleUIController.GetCanvasSoundController().PlaySESound((int)CanvasSoundController.CanvasSoundTag.Select);
        }
        if (menuButtonController.DesideCheck())
        {
            titleUIController.GetCanvasSoundController().PlaySESound((int)CanvasSoundController.CanvasSoundTag.Select);
        }
    }
}
