using CharacterTag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandInput
{
    private PlayerController controller;

    public RightHandInput(PlayerController _controller)
    {
        controller = _controller;
    }

    public void Execute()
    {
        //�E��N���X�ɓ���R�}���h��ݒ肷�鏈��
        SetRightTool();
        //�E��̓���
        if (controller.RightCommand == null) { return; }
        controller.RightCommand.Input();
    }

    private void SetRightTool()
    {
        //�e���͂Ń^�O��ݒ肷�鏈��
        SetToolInput();

        BaseToolCommand tool = controller.RightCommand;
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
                tool = new CrossBowTool(controller,shot);
                break;
        }
        SetTool(tool);
    }

    private void SetToolInput()
    {
        //���̃^�O�̓��ꕨ���쐬
        ToolInventoryController.ToolObjectTag tooltag = controller.GetToolController().CurrentToolTag; ;
        //�e���͂ɍ������^�O����
        if (controller.GetKeyInput().AttackButton)
        {
            if (controller.GetToolController().CheckNullToolObject(controller.GetToolController().GetInventoryData().ToolItemList[(int)ToolInventoryController.ToolObjectTag.Sword]))
            {
                return;
            }
            tooltag = ToolInventoryController.ToolObjectTag.Sword;
        }
        else if (controller.GetKeyInput().ToolButton)
        {
            if (controller.GetToolController().CheckNullToolObject(controller.GetToolController().GetInventoryData().ToolItemList[(int)ToolInventoryController.ToolObjectTag.CrossBow]))
            {
                return;
            }
            tooltag = ToolInventoryController.ToolObjectTag.CrossBow;
            if (controller.BattleMode)
            {
                controller.BattleMode = false;
                controller.GetMotion().ChangeMotion(StateTag.ChangeMode);
            }
        }
        else if (controller.GetKeyInput().ChangeButton)
        {
            if (controller.GetToolController().CurrentToolTag == ToolInventoryController.ToolObjectTag.Null)
            {
                if (controller.GetToolController().CheckNullToolObject(controller.GetToolController().GetInventoryData().ToolItemList[(int)ToolInventoryController.ToolObjectTag.Sword]))
                {
                    return;
                }
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
        controller.GetToolController().CurrentToolTag = tooltag;
    }

    private void SetTool(BaseToolCommand tool)
    {
        if (controller.RightCommand != null)
        {
            if (controller.RightCommand.Equals(tool)) { return; }
        }
        controller.RightCommand = tool;
    }

}
