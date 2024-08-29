using CharacterTagList;
using UnityEngine;

/// <summary>
/// プレイヤーの右手に持つ道具の処理を管理するクラス
/// </summary>
public class RightHandInput
{
    //生成と同時にPlayerControllerの情報を保持するクラス
    private PlayerController controller;
    //コンストラクタ
    public RightHandInput(PlayerController _controller)
    {
        controller = _controller;
    }
    //PlayerInputに記述してる関数
    public void Execute()
    {
        //右手クラスに道具コマンドを設定する処理
        SetToolInput();
        //右手の入力
        if (controller.RightAction == null) { return; }
        controller.RightAction.Input();
    }
    /// <summary>
    /// 指定した状態じゃなければtrueを返すbool関数
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
    /// クロスボウを持ってもいい状態か判断する
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
    //剣のアイテムを持ってるか判定するbool関数
    private bool IsSwordCheck() => controller.GetToolController().CheckNullTool(ToolInventoryController.ToolObjectTag.Sword);
    //クロスボウを持っているか判定するbool関数
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
