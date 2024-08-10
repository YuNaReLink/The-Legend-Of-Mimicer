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
        //右手クラスに道具コマンドを設定する処理
        SetRightTool();
        //右手の入力
        if (controller.RightCommand == null) { return; }
        controller.RightCommand.Input();
    }

    private void SetRightTool()
    {
        //各入力でタグを設定する処理
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
        //仮のタグの入れ物を作成
        ToolObjectTag tooltag = controller.GetToolController().CurrentToolTag; ;
        //各入力に合ったタグを代入
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
        //同じタグならリターン
        if(controller.GetToolController().CurrentToolTag == tooltag) { return; }
        //あったらそのオブジェクトのタグを代入
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
