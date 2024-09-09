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
    private GraphicRaycaster    graphicRaycaster; // UI��GraphicRaycaster
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
        // PointerEventData���쐬
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        // Raycast���ʂ��i�[���郊�X�g
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        // GraphicRaycaster��Raycast�����s
        graphicRaycaster.Raycast(pointerEventData, raycastResults);
        // ���C�������ɓ����������ǂ������`�F�b�N
        foreach (RaycastResult result in raycastResults)
        {
            // Button�R���|�[�l���g�����邩�m�F
            Button button = result.gameObject.GetComponent<Button>();
            if (button != null)
            {
                if(button == mouseOverButton) { return false; }
                mouseOverButton = button;
                Debug.Log("�}�E�X��UI��Button�I�u�W�F�N�g�ɓ�����܂����I");
                // �{�^�����N���b�N���ꂽ���̏�����ǉ�
                return true; // �ŏ��Ƀq�b�g�����{�^���̏����̂ݎ��s
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
