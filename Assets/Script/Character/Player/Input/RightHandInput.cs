using CharacterTag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerToolController;

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
            case ToolObjectTag.Null:
                tool = null;
                break;
            case ToolObjectTag.Sword:
                tool = new SwordAttackCommand(controller);
                break;
            case ToolObjectTag.Bow:
                GameObject crossBow = controller.GetToolController().Tools[(int)ToolObjectTag.Bow];
                CrossBowShot shot = crossBow.GetComponent<CrossBowShot>();
                tool = new CrossBowTool(controller,shot);
                break;
        }
        SetTool(tool);
    }

    private void SetToolInput()
    {
        //���̃^�O�̓��ꕨ���쐬
        ToolObjectTag tooltag = controller.GetToolController().CurrentToolTag; ;
        //�e���͂ɍ������^�O����
        if (controller.GetKeyInput().LeftMouseDownClick)
        {
            if (controller.GetToolController().CheckNullToolObject(controller.GetToolController().Tools[(int)ToolObjectTag.Sword]))
            {
                return;
            }
            tooltag = ToolObjectTag.Sword;
        }
        else if (controller.GetKeyInput().EKey)
        {
            if (controller.GetToolController().CheckNullToolObject(controller.GetToolController().Tools[(int)ToolObjectTag.Bow]))
            {
                return;
            }
            tooltag = ToolObjectTag.Bow;
            if (controller.BattleMode)
            {
                controller.BattleMode = false;
                controller.GetMotion().ChangeMotion(StateTag.ChangeMode);
            }
        }
        else if (controller.GetKeyInput().QKey)
        {
            if (controller.GetToolController().CurrentToolTag == ToolObjectTag.Null)
            {
                if (controller.GetToolController().CheckNullToolObject(controller.GetToolController().Tools[(int)ToolObjectTag.Sword]))
                {
                    return;
                }
                tooltag = ToolObjectTag.Sword;
            }
            else
            {
                tooltag = ToolObjectTag.Null;
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
