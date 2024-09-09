using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour
{
    [SerializeField]
    private List<Button>        buttonList = new List<Button>();

    private int                 currentButtonIndex = 0;

    private int                 pastButtonIndex = 0;

    public List<Button>         ButtonList { get { return buttonList; } set { buttonList = value; } }

    private bool                desideFlag = false;

    [SerializeField]
    private GraphicRaycaster    graphicRaycaster; // UIのGraphicRaycaster
    [SerializeField]
    private EventSystem         eventSystem; // EventSystem

    private Button              mouseOverButton = null;

    public void AwakeInitilaize()
    {
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
    }

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
        if (InputManager.GetItemButton()||Input.GetMouseButtonDown(0)&& CheckIfMouseOverButton())
        {
            buttonList[currentButtonIndex].onClick.Invoke();
            desideFlag = true;
        }
    }

    public bool ButtonIndexCheck()
    {
        if(currentButtonIndex == pastButtonIndex&&!CheckIfMouseOverButton()) { return false; }
        pastButtonIndex = currentButtonIndex;
        return true;
    }

    private bool CheckIfMouseOverButton()
    {
        // PointerEventDataを作成
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        // Raycast結果を格納するリスト
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        // GraphicRaycasterでRaycastを実行
        graphicRaycaster.Raycast(pointerEventData, raycastResults);
        // レイが何かに当たったかどうかをチェック
        foreach (RaycastResult result in raycastResults)
        {
            // Buttonコンポーネントがあるか確認
            Button button = result.gameObject.GetComponent<Button>();
            if (button != null)
            {
                if(button == mouseOverButton) { return false; }
                mouseOverButton = button;
                Debug.Log("マウスがUIのButtonオブジェクトに当たりました！");
                // ボタンがクリックされた時の処理を追加
                return true; // 最初にヒットしたボタンの処理のみ実行
            }
        }
        return false;
    }

    public bool DesideCheck()
    {
        if (!desideFlag) { return false; }
        desideFlag = false;
        return true;
    }
}
