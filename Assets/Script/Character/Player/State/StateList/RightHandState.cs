using UnityEngine;

/// <summary>
/// �v���C���[�̉E��Ɏ�����̏������Ǘ�����N���X
/// </summary>
public class RightHandState : InterfaceState
{
    //�����Ɠ�����PlayerController�̏���ێ�����N���X
    private PlayerController controller;
    //�R���X�g���N�^
    public RightHandState(PlayerController _controller)
    {
        controller = _controller;
    }
    //PlayerInput�ɋL�q���Ă�֐�
    public void DoUpdate()
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
        CharacterTagList.StateTag state = controller.CharacterStatus.CurrentState;
        bool stopState = state == CharacterTagList.StateTag.Null ||
                         state == CharacterTagList.StateTag.Idle ||
                         state == CharacterTagList.StateTag.Run ||
                         state == CharacterTagList.StateTag.Jump ||
                         state == CharacterTagList.StateTag.Fall ||
                         state == CharacterTagList.StateTag.ChangeMode;
        if(stopState) { return false; }
        return true;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool NoChangeModeState()
    {
        CharacterTagList.StateTag state = controller.CharacterStatus.CurrentState;
        switch (state)
        {
            case CharacterTagList.StateTag.Idle:
            case CharacterTagList.StateTag.ChangeMode:
                if (controller.GetKeyInput().GuardHoldButton||controller.CharacterStatus.MoveInput)
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
        CharacterTagList.StateTag state = controller.CharacterStatus.CurrentState;
        bool enabledState = state == CharacterTagList.StateTag.Idle ||
                            state == CharacterTagList.StateTag.Run ||
                            state == CharacterTagList.StateTag.Rolling ||
                            state == CharacterTagList.StateTag.Gurid;
        if (enabledState) { return true; }
        return false;
    }
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
                controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.ChangeMode);
            }
        }

        //�ύX���ꂽ�c�[���̃R�}���h�𐶐�����
        InterfaceBaseToolCommand tool = controller.RightAction;
        switch (controller.GetToolController().CurrentToolTag)
        {
            case ToolInventoryController.ToolObjectTag.Null:
                tool = null;
                break;
            case ToolInventoryController.ToolObjectTag.Sword:
                tool = new SwordAttackCommand(controller);
                break;
            case ToolInventoryController.ToolObjectTag.CrossBow:
                GameObject crossBow = controller.GetToolController().GetInventoryData().ToolItemList[(int)ToolInventoryController.ToolObjectTag.CrossBow];
                CrossBowShoot shot = crossBow.GetComponent<CrossBowShoot>();
                tool = new CrossBowTool(controller, shot);
                break;
        }
        SetTool(tool);
    }

    private void SetTool(InterfaceBaseToolCommand tool)
    {
        if (controller.RightAction != null)
        {
            if (controller.RightAction.Equals(tool)) { return; }
        }
        controller.RightAction = tool;
    }

}
