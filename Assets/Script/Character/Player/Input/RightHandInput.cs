using CharacterTagList;
using UnityEngine;

/// <summary>
/// �v���C���[�̉E��Ɏ�����̏������Ǘ�����N���X
/// </summary>
public class RightHandInput
{
    //�����Ɠ�����PlayerController�̏���ێ�����N���X
    private PlayerController controller;
    //�R���X�g���N�^
    public RightHandInput(PlayerController _controller)
    {
        controller = _controller;
    }
    //PlayerInput�ɋL�q���Ă�֐�
    public void Execute()
    {
        //�E��N���X�ɓ���R�}���h��ݒ肷�鏈��
        SetToolInput();
        //�E��̓���
        if (controller.RightAction == null) { return; }
        controller.RightAction.Input();
    }
    /// <summary>
    /// �w�肵����Ԃ���Ȃ����true��Ԃ�bool�֐�
    /// </summary>
    /// <returns></returns>
    private bool CheckStopState()
    {
        StateTag state = controller.CurrentState;
        switch (state)
        {
            case StateTag.Null:
            case StateTag.Idle:
            case StateTag.Run:
            case StateTag.Jump:
            case StateTag.Fall:
            case StateTag.ChangeMode:
                return false;
        }
        return true;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool NoChangeModeState()
    {
        StateTag state = controller.CurrentState;
        switch (state)
        {
            case StateTag.Idle:
            case StateTag.ChangeMode:
                if (controller.GetKeyInput().GuardHoldButton||controller.MoveInput)
                {
                    return false;
                }
                return true;
        }
        return false;
    }
    /// <summary>
    /// �N���X�{�E�������Ă�������Ԃ����f����
    /// </summary>
    private bool CheckCrossBowEnabledState()
    {
        StateTag state = controller.CurrentState;
        switch (state)
        {
            case StateTag.Idle:
            case StateTag.Run:
            case StateTag.Rolling:
            case StateTag.Gurid:
                return true;
        }
        return false;
    }
    /// <returns></returns>
    //���̃A�C�e���������Ă邩���肷��bool�֐�
    private bool IsSwordCheck() => controller.GetToolController().CheckNullTool(ToolInventoryController.ToolObjectTag.Sword);
    //�N���X�{�E�������Ă��邩���肷��bool�֐�
    private bool IsCrossBowCheck() => controller.GetToolController().CheckNullTool(ToolInventoryController.ToolObjectTag.CrossBow);

    private void SetToolInput()
    {
        //���̃^�O�̓��ꕨ���쐬
        var tooltag = controller.GetToolController().CurrentToolTag;
        //�e���͂ɍ������^�O����
        if (controller.GetKeyInput().AttackButton)
        {
            //�����C���x���g���ɂȂ����`�F�b�N
            if (IsSwordCheck() || CheckStopState()) { return; }
            tooltag = ToolInventoryController.ToolObjectTag.Sword;
        }
        else if (controller.GetKeyInput().ToolButton)
        {
            if (IsCrossBowCheck()) { return; }
            if(!CheckCrossBowEnabledState()) { return; }
            tooltag = ToolInventoryController.ToolObjectTag.CrossBow;
        }
        else if (controller.GetKeyInput().ChangeButton)
        {
            if (!NoChangeModeState()) { return; }
            if (tooltag == ToolInventoryController.ToolObjectTag.Null)
            {
                if (IsSwordCheck()) { return; }
                tooltag = ToolInventoryController.ToolObjectTag.Sword;
            }
            else
            {
                tooltag = ToolInventoryController.ToolObjectTag.Null;
            }
        }
        //�����^�O�Ȃ烊�^�[��
        if(controller.GetToolController().CurrentToolTag == tooltag) { return; }
        //�������炻�̃I�u�W�F�N�g�̃^�O����
        ChangeControllerToolTag(tooltag); ;
    }

    private void ChangeControllerToolTag(ToolInventoryController.ToolObjectTag tooltag)
    {
        controller.GetToolController().ChangeToolTag(tooltag);

        if (tooltag == ToolInventoryController.ToolObjectTag.CrossBow)
        {
            if (controller.BattleMode)
            {
                controller.BattleMode = false;
                controller.GetMotion().ChangeMotion(StateTag.ChangeMode);
            }
        }

        //�ύX���ꂽ�c�[���̃R�}���h�𐶐�����
        InterfaceBaseToolAction tool = controller.RightAction;
        switch (controller.GetToolController().CurrentToolTag)
        {
            case ToolInventoryController.ToolObjectTag.Null:
                tool = null;
                break;
            case ToolInventoryController.ToolObjectTag.Sword:
                tool = new SwordAttackAction(controller);
                break;
            case ToolInventoryController.ToolObjectTag.CrossBow:
                GameObject crossBow = controller.GetToolController().GetInventoryData().ToolItemList[(int)ToolInventoryController.ToolObjectTag.CrossBow];
                CrossBowShoot shot = crossBow.GetComponent<CrossBowShoot>();
                tool = new CrossBowTool(controller, shot);
                break;
        }
        SetTool(tool);
    }

    private void SetTool(InterfaceBaseToolAction tool)
    {
        if (controller.RightAction != null)
        {
            if (controller.RightAction.Equals(tool)) { return; }
        }
        controller.RightAction = tool;
    }

}
