using UnityEngine;

/// <summary>
/// プレイヤーの右手に持つ道具の処理を管理するクラス
/// </summary>
public class RightHandState : InterfaceState
{
    //生成と同時にPlayerControllerの情報を保持するクラス
    private PlayerController controller;
    //コンストラクタ
    public RightHandState(PlayerController _controller)
    {
        controller = _controller;
    }
    //PlayerInputに記述してる関数
    public void DoUpdate()
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
    /// クロスボウを持ってもいい状態か判断する
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
                controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.ChangeMode);
            }
        }

        //変更されたツールのコマンドを生成する
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
