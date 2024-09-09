using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOperationUIController : MonoBehaviour
{
    private List<Image>             imageList = new List<Image>();

    private enum MenuOperation
    {
        Key,
        Controller
    }

    [SerializeField]
    private List<SpriteObjectData>  spriteObjectDataList = new List<SpriteObjectData>();


    public void AwakeInitilaize()
    {
        GameObject g = null;
        Image image = null;
        for(int i = 0; i < transform.childCount; i++)
        {
            g = transform.GetChild(i).gameObject;
            image = g.GetComponent<Image>();
            if(image != null)
            {
                imageList.Add(image);
            }
        }
    }

    public void DoUpdate()
    {
        switch (InputManager.GetDeviceInput())
        {
            case InputManager.DeviceInput.Key:
                SetUI(spriteObjectDataList[(int)MenuOperation.Key]);
                break;
            case InputManager.DeviceInput.Controller:
                SetUI(spriteObjectDataList[(int)MenuOperation.Controller]);
                break;
        }
    }

    private void SetUI(SpriteObjectData data)
    {
        for (int i = 0; i < data.SpriteList.Count; i++)
        {
            if (data.SpriteList[i] == null) { continue; }
            if (imageList[i].sprite == data.SpriteList[i]) { continue; }
            imageList[i].sprite = data.SpriteList[i];
        }
    }

}
