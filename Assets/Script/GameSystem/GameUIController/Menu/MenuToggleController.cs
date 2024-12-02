using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuToggleController : MonoBehaviour
{
    // �t�H�[�J�X���ړ���������Toggle�̃��X�g
    [SerializeField]
    private List<Toggle>        toggleList; 
    public List<Toggle>         ToggleList { get { return toggleList; } set { toggleList = value; } }
    private int                 currentToggleIndex = 0;
    private int                 pastToggleIndex = 0;
    [SerializeField]
    private GraphicRaycaster    graphicRaycaster; // UI��GraphicRaycaster
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
            Toggle toggle = result.gameObject.GetComponent<Toggle>();
            if (toggle != null)
            {
                if (toggle == mouseOverToggle) { return false; }
                mouseOverToggle = toggle;
                Debug.Log("�}�E�X��UI��Button�I�u�W�F�N�g�ɓ�����܂����I");
                // �{�^�����N���b�N���ꂽ���̏�����ǉ�
                return true; // �ŏ��Ƀq�b�g�����{�^���̏����̂ݎ��s
            }
        }
        return false;
    }
}
