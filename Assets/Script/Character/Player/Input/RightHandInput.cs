using CharacterTag;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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
        //右手クラスに道具コマンドを設定する処理
        SetToolInput();
        //右手の入力
        if (controller.RightAction == null) { return; }
        controller.RightAction.Input();
    }

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

    private bool NoChangeModeState()
    {
        StateTag state = controller.CurrentState;
        switch (state)
        {
            case StateTag.Idle:
            case StateTag.ChangeMode:
                if (controller.GetKeyInput().GuardHoldButton)
                {
                    return false;
                }
                return true;
        }
        return false;
    }

    private bool IsSwordCheck() => controller.GetToolController().CheckNullTool(ToolInventoryController.ToolObjectTag.Sword);
    private bool IsCrossBowCheck() => controller.GetToolController().CheckNullTool(ToolInventoryController.ToolObjectTag.CrossBow);

    private void SetToolInput()
    {
        //仮のタグの入れ物を作成
        var tooltag = controller.GetToolController().CurrentToolTag;
        //各入力に合ったタグを代入
        if (controller.GetKeyInput().AttackButton)
        {
            //剣がインベントリにないかチェック
            if (IsSwordCheck() || CheckStopState()) { return; }
            tooltag = ToolInventoryController.ToolObjectTag.Sword;
        }
        else if (controller.GetKeyInput().ToolButton)
        {
            if (IsCrossBowCheck()) { return; }
            tooltag = ToolInventoryController.ToolObjectTag.CrossBow;
        }
        else if (controller.GetKeyInput().ChangeButton)
        {
            if (!NoChangeModeState()) { return; }
            if (tooltag == ToolInventoryController.ToolObjectTag.Null && controller.BattleMode)
            {
                if (IsSwordCheck()) { return; }
                tooltag = ToolInventoryController.ToolObjectTag.Sword;
            }
            else
            {
                tooltag = ToolInventoryController.ToolObjectTag.Null;
            }
        }
        //同じタグならリターン
        if(controller.GetToolController().CurrentToolTag == tooltag) { return; }
        //あったらそのオブジェクトのタグを代入
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

        //変更されたツールのコマンドを生成する
        BaseToolAction tool = controller.RightAction;
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

    private void SetTool(BaseToolAction tool)
    {
        if (controller.RightAction != null)
        {
            if (controller.RightAction.Equals(tool)) { return; }
        }
        controller.RightAction = tool;
    }

}
