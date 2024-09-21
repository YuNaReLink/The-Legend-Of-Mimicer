using UnityEngine;

/// <summary>
/// プレイヤー、敵で使用してるモーションクラスのベースクラス
/// </summary>
public class MotionController
{
    protected readonly string stateName = "State";
    protected readonly string boolname = "BattleMode";
    protected readonly string guardName = "Guard";

    /// <summary>
    /// 下記は関数だけを宣言
    /// 実装は継承先で行う
    /// </summary>
    /// <param name="_state"></param>
    public virtual void ChangeMotion(CharacterTagList.StateTag _state) { }
    public virtual void ForcedChangeMotion(CharacterTagList.StateTag _state) { }
    public virtual bool MotionEndCheck() { return false; }
    public virtual bool IsEndRollingMotionNameCheck() { return false; }
    public virtual void Change(AnimationClip clip) { }
    public virtual void StopMotionCheck() { }
    public virtual void EndMotionNameCheck() { }
}
