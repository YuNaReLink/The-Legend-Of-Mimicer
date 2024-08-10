using CharacterTag;
using UnityEngine;

public class MotionController
{
    protected string statename = "State";
    protected string guardName = "Guard";

    /// <summary>
    /// ‰º‹L‚ÍŠÖ”‚¾‚¯‚ğéŒ¾
    /// À‘•‚ÍŒp³æ‚Ås‚¤
    /// </summary>
    /// <param name="_state"></param>
    public virtual void ChangeMotion(StateTag _state) { }

    public virtual bool MotionEndCheck() { return false; }

    public virtual bool IsMotionNameCheck(string name) { return false; }

    public virtual bool IsEndRollingMotionNameCheck() { return false; }

    public virtual void Change(AnimationClip clip) { }

    public virtual void StopMotionCheck() { }

    public virtual void EndMotionNameCheck() { }
}
