using System.Collections.Generic;
using UnityEngine;

public class ToolInventoryController : MonoBehaviour
{
    private PlayerController controller;

    public void SetController(PlayerController _controller) { controller = _controller; }
    public enum ToolObjectTag
    {
        Null = -1,
        Sword,
        Shild,
        CrossBow,
        DataEnd
    }

    private ToolObjectTag currentToolTag = ToolObjectTag.Null;
    public ToolObjectTag CurrentToolTag { get{ return currentToolTag; }set { currentToolTag = value; } }

    private InventoryData inventoryData = null;
    public InventoryData GetInventoryData() {  return inventoryData; }

    /// <summary>
    /// �����̓������X�g�ɂ��邩�`�F�b�N
    /// </summary>
    /// <param name="tool"></param>
    /// <returns></returns>
    public bool CheckNullToolObject(GameObject tool)
    {
        if(inventoryData.ToolItemList.Count == 0) {  return true; }
        for(int i = 0; i < inventoryData.ToolItemList.Count; i++)
        {
            if (inventoryData.ToolItemList[i] == null) {  continue; }
            if (inventoryData.ToolItemList[i] == tool)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// ����̊Ǘ��N���X�̃��X�g
    /// </summary>
    [SerializeField]
    private List<ToolController> toolControllers = new List<ToolController>();
    public List<ToolController> ToolControllers { get { return toolControllers; } set { toolControllers = value; } }

    private Quiver quiver = null;
    public Quiver GetQuiver() { return quiver; }

    //�������܂��Ă��鎞�̈ʒu�E��]
    [SerializeField]
    private Transform swordTransform;

    [SerializeField]
    private Transform crossBowTransform;

    [SerializeField]
    private Vector3 localCrossBowPos = new Vector3(1, -0.8f, 1);


    //����w�����ʒu�E��]
    [SerializeField]
    private Transform shieldTransform;

    [SerializeField]
    private Transform rightHandTransform;

    [SerializeField]
    private Transform leftHandTransform;


    /// <summary>
    /// �G�t�F�N�g�֌W�̕ϐ�
    /// </summary>
    private SwordEffectController swordEffect = null;

    public void Initilaize()
    {
        inventoryData = GetComponent<InventoryData>();
        InitializeToolSetting();
    }


    public void UpdateProps()
    {
        ChangeSwordTransform();
        ChangeCrossBowTransform();
        ChangeShieldTransform();
    }


    /// <summary>
    /// �Q�[���J�n���A����������Ă������ɍs������
    /// </summary>
    public void InitializeToolSetting()
    {
        for (int i = 0; i < inventoryData.ToolItemList.Count; i++)
        {
            if (inventoryData.ToolItemList[i] == null) { continue; }
            toolControllers[i] = inventoryData.ToolItemList[i].GetComponent<ToolController>();
            toolControllers[i].SetController(controller);
        }
    }

    /// <summary>
    /// �r���A������Q�b�g�������̏���
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="tool"></param>
    public void GetToolSetting(ToolObjectTag tag,GameObject tool)
    {
        if (inventoryData.ToolItemList[(int)tag] != null) { return; }
        GameObject toolObject = Instantiate(tool);
        //����List�ɓo�^
        //tools.Add(toolObject);
        inventoryData.ToolItemList[(int)tag] = toolObject;
        //����̃R���g���[���[��o�^
        toolControllers[(int)tag] = toolObject.GetComponent<ToolController>();
        toolControllers[(int)tag].SetController(controller);
        Transform parent = null;
        switch (tag)
        {
            case ToolObjectTag.Sword:
                parent = swordTransform;
                break;
            case ToolObjectTag.CrossBow:
                parent = crossBowTransform;
                gameObject.AddComponent<Quiver>();
                quiver = GetComponent<Quiver>();
                break;
            case ToolObjectTag.Shild:
                parent = shieldTransform;
                break;
        }
        toolObject.transform.SetParent(parent);
        toolObject.transform.position = parent.position;
        toolObject.transform.rotation = parent.rotation;
        toolObject.transform.localScale = parent.localScale;
    }

    //����̈ʒu�����ւ��鏈��
    private void SetToolPosition(ToolObjectTag tool,Transform transform)
    {
        if (inventoryData.ToolItemList[(int)tool].transform.parent == transform) { return; }
        inventoryData.ToolItemList[(int)tool].transform.SetParent(null);
        inventoryData.ToolItemList[(int)tool].transform.SetParent(transform);
        inventoryData.ToolItemList[(int)tool].transform.position = transform.position;
        inventoryData.ToolItemList[(int)tool].transform.rotation = transform.rotation;
        inventoryData.ToolItemList[(int)tool].transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void ChangeSwordTransform()
    {
        if(inventoryData.ToolItemList[(int)ToolObjectTag.Sword] == null) { return; }
        if (currentToolTag == ToolObjectTag.Sword)
        {
            SetToolPosition(ToolObjectTag.Sword, rightHandTransform);
        }
        else
        {
            SetToolPosition(ToolObjectTag.Sword, swordTransform);
        }
    }

    private void ChangeCrossBowTransform()
    {
        if (inventoryData.ToolItemList[(int)ToolObjectTag.CrossBow] == null) { return; }
        if (currentToolTag == ToolObjectTag.CrossBow)
        {
            SetToolPosition(ToolObjectTag.CrossBow, controller.GetCameraObject().transform);
            inventoryData.ToolItemList[(int)ToolObjectTag.CrossBow].transform.localPosition = localCrossBowPos;
        }
        else
        {
            SetToolPosition(ToolObjectTag.CrossBow, crossBowTransform);
        }
    }

    private void ChangeShieldTransform()
    {
        if (inventoryData.ToolItemList[(int)ToolObjectTag.Shild] == null) { return; }
        if (controller.BattleMode||controller.GetKeyInput().RightMouseClick)
        {
            SetToolPosition(ToolObjectTag.Shild, leftHandTransform);
        }
        else
        {
            SetToolPosition(ToolObjectTag.Shild, shieldTransform);
        }
    }

}