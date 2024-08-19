using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyInputUIController : MonoBehaviour
{
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

    public void Initialize()
    {
        GameUIController gameUIController = GetComponentInParent<GameUIController>();
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
        CrossBowActiveCheck();
        FKeyActiveCheck();

        EKeyUI();
        QKeyUI();
        CKeyUI();
        FKeyUI();
        TabKeyUI();
        ShiftKeyUI();
    }

    private void OutputText(string text,bool active)
    {
        if (keyUIObjectArray[(int)KeyUINumber.F].activeSelf == active) { return; }
        fKeyText.text = text;
        keyUIObjectArray[(int)KeyUINumber.F].SetActive(active);
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
        switch (GameSceneSystemController.KeyTriggerTag)
        {
            case GameSceneSystemController.TriggerTag.Null:
                OutputText("", false);
                break;
            case GameSceneSystemController.TriggerTag.Door:
            case GameSceneSystemController.TriggerTag.Chest:
                OutputText("ŠJ‚¯‚é", true);
                break;
            case GameSceneSystemController.TriggerTag.Item:
                OutputText("Žæ‚é", true);
                break;
        }
    }

    private void EKeyUI()
    {
        if (inputCoolDownsTimers[(int)KeyUINumber.E].IsEnabled()) { return; }
        if (InputManager.PushEKey())
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
        if (InputManager.PushQKey())
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
        if (InputManager.PushCKey())
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
        if (InputManager.PushFKey())
        {
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
        if (InputManager.PushTabKey())
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
        if (InputManager.PushShiftKey())
        {
            keyImageArray[(int)KeyUINumber.Shift].color = new Color32(100, 100, 100, 255);
            inputCoolDownsTimers[(int)KeyUINumber.Shift].StartTimer(0.1f);
        }
        else
        {
            keyImageArray[(int)KeyUINumber.Shift].color = new Color32(255, 255, 255, 255);
        }
    }
}
