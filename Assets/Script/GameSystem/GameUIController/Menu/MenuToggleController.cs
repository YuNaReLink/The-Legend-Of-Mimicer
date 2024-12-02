using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuToggleController : MonoBehaviour
{
    // フォーカスを移動させたいToggleのリスト
    [SerializeField]
    private List<Toggle>        toggleList; 
    public List<Toggle>         ToggleList { get { return toggleList; } set { toggleList = value; } }
    private int                 currentToggleIndex = 0;
    private int                 pastToggleIndex = 0;
    [SerializeField]
    private GraphicRaycaster    graphicRaycaster; // UIのGraphicRaycaster
    [SerializeField]
    private EventSystem         eventSystem; // EventSystem
    private Toggle              mouseOverToggle = null;

    public void AwakeInitilaize()
    {
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
    }

    public void ToggleYUpdate()
    {
        for (int i = 0; i < toggleList.Count; i++)
        {
            if (toggleList[i] == null)
            {
                return;
            }
        }
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
        for(int i = 0; i < toggleList.Count; i++)
        {
            if(toggleList[i] == null)
            {
                return;
            }
        }
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

    public bool ToggleIndexCheck()
    {
        if(currentToggleIndex == pastToggleIndex) { return false; }
        pastToggleIndex = currentToggleIndex;
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
            Toggle toggle = result.gameObject.GetComponent<Toggle>();
            if (toggle != null)
            {
                if (toggle == mouseOverToggle) { return false; }
                mouseOverToggle = toggle;
                Debug.Log("マウスがUIのButtonオブジェクトに当たりました！");
                // ボタンがクリックされた時の処理を追加
                return true; // 最初にヒットしたボタンの処理のみ実行
            }
        }
        return false;
    }
}
