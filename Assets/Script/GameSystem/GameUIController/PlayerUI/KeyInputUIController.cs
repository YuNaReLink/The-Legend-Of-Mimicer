using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyInputUIController : MonoBehaviour
{
    private GameUIController gameUIController = null;
    private PlayerController controller = null;
    public enum KeyUINumber
    {
        E,
        Q,
        C,
        F,
        Tab,
        Shift
    }

    [SerializeField]
    private List<GameObject> keyUIObjectArray = new List<GameObject>();

    [SerializeField]
    private List<Image> keyImageArray = new List<Image>();

    private List<DeltaTimeCountDown> inputCoolDownsTimers = new List<DeltaTimeCountDown>();

    private GameObject crossbowUI = null;

    private Text fKeyText = null;

    public GameObject SelfObject() { return gameObject; }
    public bool IsActiveObject() { return gameObject.activeSelf; }
    public void SetActiveObject(bool enabled) { gameObject.SetActive(enabled); }

    private bool controllerInput = false;


    private enum ControllerTag
    {
        Key,
        Controller
    }
    [SerializeField]
    private List<SpriteObjectData> spriteObjectList = new List<SpriteObjectData>();

    public void Initialize()
    {
        gameUIController = GetComponentInParent<GameUIController>();
        if(gameUIController != null)
        {
            controller = gameUIController.GetPlayerUIController().GetPlayerController();
        }

        GameObject child = null;
        Image image = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            child = null;
            child = transform.GetChild(i).gameObject;
            if(child != null)
            {
                keyUIObjectArray.Add(child);
            }
            image = child.GetComponent<Image>();
            if(image != null)
            {
                keyImageArray.Add(image);
            }
            inputCoolDownsTimers.Add(new DeltaTimeCountDown());

        }

        child = keyUIObjectArray[(int)KeyUINumber.E].transform.GetChild(0).gameObject;
        if(child != null)
        {
            crossbowUI = child;
        }

        Text t = keyUIObjectArray[(int)KeyUINumber.F].GetComponentInChildren<Text>();
        if(t != null)
        {
            fKeyText = t;
        }

    }

    public void KeyUIInputUpdate()
    {
        for(int i = 0;i < inputCoolDownsTimers.Count; i++)
        {
            inputCoolDownsTimers[i].Update();
        }
        //入力してるのがキーボードかコントローラーか判定
        if (InputManager.GetDeviceInput() == InputManager.DeviceInput.Controller)
        {
            SetUI(spriteObjectList[(int)ControllerTag.Controller]);
        }
        else
        {
            SetUI(spriteObjectList[(int)ControllerTag.Key]);
        }

        CrossBowActiveCheck();
        FKeyActiveCheck();

        EKeyUI();
        QKeyUI();
        CKeyUI();
        FKeyUI();
        TabKeyUI();
        ShiftKeyUI();
    }

    private void SetUI(SpriteObjectData data)
    {
        for(int i = 0; i < data.SpriteList.Count; i++)
        {
            if(data.SpriteList[i] == null) { continue; }
            if (keyImageArray[i].sprite == data.SpriteList[i]) { continue; }
            keyImageArray[i].sprite = data.SpriteList[i];
        }
    }

    private void OutputText(string text,bool active)
    {
        if(fKeyText.text != text)
        {
            fKeyText.text = text;
        }
        if (keyUIObjectArray[(int)KeyUINumber.F].activeSelf != active)
        {
            keyUIObjectArray[(int)KeyUINumber.F].SetActive(active); 
        }
    }

    private void CrossBowActiveCheck()
    {
        if (controller.GetToolController().GetInventoryData().ToolItemList[(int)ToolInventoryController.ToolObjectTag.CrossBow] != null)
        {
            crossbowUI.SetActive(true);
        }
        else
        {
            crossbowUI.SetActive(false);
        }
    }

    private void FKeyActiveCheck()
    {
        switch (GameSceneSystemController.Instance.KeyTriggerTag)
        {
            case GameSceneSystemController.TriggerTag.Null:
                OutputText("", false);
                break;
            case GameSceneSystemController.TriggerTag.Door:
            case GameSceneSystemController.TriggerTag.Chest:
                OutputText("開ける", true);
                break;
            case GameSceneSystemController.TriggerTag.Item:
                OutputText("取る", true);
                break;
        }
    }

    private void EKeyUI()
    {
        if (inputCoolDownsTimers[(int)KeyUINumber.E].IsEnabled()) { return; }
        if (InputManager.ToolButton())
        {
            keyImageArray[(int)KeyUINumber.E].color = new Color32(100, 100, 100, 255);
            inputCoolDownsTimers[(int)KeyUINumber.E].StartTimer(0.1f);
        }
        else
        {
            keyImageArray[(int)KeyUINumber.E].color = new Color32(255, 255, 255, 255);
        }
    }

    private void QKeyUI()
    {
        if (inputCoolDownsTimers[(int)KeyUINumber.Q].IsEnabled()) { return; }
        if (InputManager.ChangeButton())
        {
            keyImageArray[(int)KeyUINumber.Q].color = new Color32(100, 100, 100, 255);
            inputCoolDownsTimers[(int)KeyUINumber.Q].StartTimer(0.1f);
        }
        else
        {
            keyImageArray[(int)KeyUINumber.Q].color = new Color32(255, 255, 255, 255);
        }
    }

    private void CKeyUI()
    {
        if (inputCoolDownsTimers[(int)KeyUINumber.C].IsEnabled()) { return; }
        if (InputManager.LockCameraButton())
        {
            keyImageArray[(int)KeyUINumber.C].color = new Color32(100, 100, 100, 255);
            inputCoolDownsTimers[(int)KeyUINumber.C].StartTimer(0.1f);
        }
        else
        {
            keyImageArray[(int)KeyUINumber.C].color = new Color32(255, 255, 255, 255);
        }
    }

    private void FKeyUI()
    {
        if (inputCoolDownsTimers[(int)KeyUINumber.F].IsEnabled()) { return; }
        if (InputManager.GetItemButton())
        {
            switch (GameSceneSystemController.Instance.KeyTriggerTag)
            {
                case GameSceneSystemController.TriggerTag.Item:
                    gameUIController.GetKeySoundController().PlaySESound((int)SoundTagList.GetToolSoundTag.Get);
                    break;
            }
            keyImageArray[(int)KeyUINumber.F].color = new Color32(100, 100, 100, 255);
            inputCoolDownsTimers[(int)KeyUINumber.F].StartTimer(0.1f);
        }
        else
        {
            keyImageArray[(int)KeyUINumber.F].color = new Color32(255, 255, 255, 255);
        }
    }
    private void TabKeyUI()
    {
        if (inputCoolDownsTimers[(int)KeyUINumber.Tab].IsEnabled()) { return; }
        if (InputManager.MenuButton())
        {
            keyImageArray[(int)KeyUINumber.Tab].color = new Color32(100, 100, 100, 255);
            inputCoolDownsTimers[(int)KeyUINumber.Tab].StartTimer(0.1f);
        }
        else
        {
            keyImageArray[(int)KeyUINumber.Tab].color = new Color32(255, 255, 255, 255);
        }
    }
    private void ShiftKeyUI()
    {
        if (inputCoolDownsTimers[(int)KeyUINumber.Shift].IsEnabled()) { return; }
        if (InputManager.ActionButton())
        {
            keyImageArray[(int)KeyUINumber.Shift].color = new Color32(100, 100, 100, 255);
            inputCoolDownsTimers[(int)KeyUINumber.Shift].StartTimer(0.1f);
        }
        else
        {
            keyImageArray[(int)KeyUINumber.Shift].color = new Color32(255, 255, 255, 255);
        }
    }

    public void ActiveKeyUI(bool active)
    {
        for (int i = 0; i < keyUIObjectArray.Count; i++)
        {
            if (keyUIObjectArray[i].activeSelf != active)
            {
                keyUIObjectArray[i].SetActive(active);
            }
        }
    }
}
