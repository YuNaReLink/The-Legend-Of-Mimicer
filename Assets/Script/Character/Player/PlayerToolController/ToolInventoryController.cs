using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[���g������̈ʒu�ύX�A�擾�Ȃǂ̏������Ǘ�����N���X
/// </summary>
public class ToolInventoryController : MonoBehaviour
{
    private PlayerController        controller;

    public void SetController(PlayerController _controller) { controller = _controller; }
    public enum ToolObjectTag
    {
        Null = -1,
        Sword,
        Shild,
        CrossBow,
        DataEnd
    }

    private ToolObjectTag           currentToolTag = ToolObjectTag.Null;
    private bool                    currentToolChange = false;
    public ToolObjectTag            CurrentToolTag => currentToolTag;
    public bool                     IsCurrentToolChange => currentToolChange;
    public void ChangeToolTag(ToolObjectTag tooltag)
    {
        currentToolTag = tooltag;
        currentToolChange = true;
    }

    private InventoryData           inventoryData = null;
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
            if (inventoryData.ToolItemList[i] == tool) { return false; }
        }
        return true;
    }

    public bool CheckNullTool(ToolObjectTag toolTag) =>
        CheckNullToolObject(inventoryData.ToolItemList[(int)toolTag]);

    /// <summary>
    /// ����̊Ǘ��N���X�̃��X�g
    /// </summary>
    [SerializeField]
    private List<ToolController>    toolControllers = new List<ToolController>();

    private Quiver                  quiver = null;
    public Quiver                   GetQuiver() { return quiver; }

    /// <summary>
    /// ���A�N���X�{�E��܂��Ă��鎞�Ƒ������Ă鎞��Transform
    /// </summary>
    [SerializeField]
    private Transform               swordTransform;
    [SerializeField]
    private Transform               crossBowTransform;
    [SerializeField]
    private Vector3                 localCrossBowPos = new Vector3(1, -0.8f, 1);
    [SerializeField]
    private Transform               rightHandTransform;

    /// <summary>
    /// ����܂��Ă��鎞�Ƒ������Ă鎞��Transform
    /// </summary>
    [SerializeField]
    private Transform               shieldTransform;
    [SerializeField]
    private Transform               leftHandTransform;

    public void Initilaize()
    {
        inventoryData = GetComponent<InventoryData>();
        InitializeToolSetting();
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


    public void UpdateTool()
    {
        currentToolChange = false;
        ChangeSwordTransform();
        ChangeCrossBowTransform();
        ChangeShieldTransform();
    }



    /// <summary>
    /// �r���A������Q�b�g�������̏���
    /// </summary>
    /// <param name="tag">
    /// �擾���铹��̃^�O
    /// </param>
    /// <param name="tool">
    /// �擾���铹��̃I�u�W�F�N�g
    /// </param>
    public void GetToolSetting(ToolObjectTag tag,GameObject tool)
    {
        if (inventoryData.ToolItemList[(int)tag] != null) { return; }
        GameObject toolObject = Instantiate(tool);
        //����List�ɓo�^
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
    /// <summary>
    /// ����̈ʒu�����ւ��鏈��
    /// </summary>
    /// <param name="tool">
    /// �ʒu�����ւ��铹��̃^�O
    /// </param>
    /// <param name="transform">
    /// �ʒu�����ւ�����Transform
    /// </param>
    private void SetToolPosition(ToolObjectTag tool,Transform transform)
    {
        int t = (int)tool;
        if (inventoryData.ToolItemList[t].transform.parent == transform) { return; }
        inventoryData.ToolItemList[t].transform.SetParent(null);
        inventoryData.ToolItemList[t].transform.SetParent(transform);
        inventoryData.ToolItemList[t].transform.position = transform.position;
        inventoryData.ToolItemList[t].transform.rotation = transform.rotation;
        inventoryData.ToolItemList[t].transform.localScale = new Vector3(1f, 1f, 1f);
    }
    //���̈ʒu�����ւ��鏈��
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
    //�N���X�{�E�̈ʒu�����ւ��鏈��
    private void ChangeCrossBowTransform()
    {
        int c = (int)ToolObjectTag.CrossBow;
        if (inventoryData.ToolItemList[c] == null) { return; }
        if (currentToolTag == ToolObjectTag.CrossBow)
        {
            SetToolPosition(ToolObjectTag.CrossBow, CameraController.Instance.transform);
            inventoryData.ToolItemList[c].transform.localPosition = localCrossBowPos;
        }
        else
        {
            SetToolPosition(ToolObjectTag.CrossBow, crossBowTransform);
        }
    }
    //���̈ʒu�����ւ��鏈��
    private void ChangeShieldTransform()
    {
        if (inventoryData.ToolItemList[(int)ToolObjectTag.Shild] == null) { return; }
        if (controller.BattleMode|| controller.CharacterStatus.GuardState != CharacterTagList.GuardState.Null)
        {
            SetToolPosition(ToolObjectTag.Shild, leftHandTransform);
        }
        else
        {
            SetToolPosition(ToolObjectTag.Shild, shieldTransform);
        }
    }

}
