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

    private void SetToolInput()
    {
        //仮のタグの入れ物を作成
        ToolInventoryController.ToolObjectTag tooltag = controller.GetToolController().CurrentToolTag; ;
        //各入力に合ったタグを代入
        if (controller.GetKeyInput().AttackButton)
        {
            //剣がインベントリにないかチェック
            bool nullSword = controller.GetToolController().CheckNullToolObject(controller.GetToolController().GetInventoryData().ToolItemList[(int)ToolInventoryController.ToolObjectTag.Sword]);
            if (nullSword||CheckStopState()){return;}
            //剣抜き出し時の時だけのサウンド再生
            if(tooltag != ToolInventoryController.ToolObjectTag.Sword)
            {
                controller.GetSoundController().PlaySESound((int)PlayerSoundController.PlayerSoundTag.FirstAttack);
            }
            tooltag = ToolInventoryController.ToolObjectTag.Sword;
        }
        else if (controller.GetKeyInput().ToolButton&&controller.GetCameraController().IsFPSMode())
        {
            if (controller.GetToolController().CheckNullToolObject(controller.GetToolController().GetInventoryData().ToolItemList[(int)ToolInventoryController.ToolObjectTag.CrossBow]))
            {
                return;
            }
            //まだクロスボウのタグが入っていないなら
            if(tooltag != ToolInventoryController.ToolObjectTag.CrossBow)
            {
                //ボタンフラグを解除
                controller.GetKeyInput().ToolButton = false;
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
            if (!NoChangeModeState()) { return; }
            if (controller.GetToolController().CurrentToolTag == ToolInventoryController.ToolObjectTag.Null&&controller.BattleMode)
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
        else if(controller.RightCommand == tool) { return; }
        controller.RightCommand = tool;
    }

}
