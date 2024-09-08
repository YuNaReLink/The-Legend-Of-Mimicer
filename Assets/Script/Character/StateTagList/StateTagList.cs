
/// <summary>
/// Characterクラスを継承したクラスで使用するタグをまとめたnamespace
/// </summary>
namespace CharacterTagList
{
    public enum StateTag
    {
        Null = -1,
        Idle,
        Run,
        Rolling,
        Jump,
        WallJump,
        Grab,
        ClimbWall,
        Attack,
        JumpAttack,
        SpinAttack,
        ReadySpinAttack,
        Gurid,
        Fall,
        Damage,
        Die,
        ChangeMode,
        Push,
        Pull,
        GetUp,
        DataEnd
    }

    public enum DirectionTag
    {
        Null = -1,
        Up,
        Down,
        Left,
        Right,
        DataEnd
    }

    public enum TripleAttack
    {
        Null = -1,
        First,
        Second,
        Three,
        DataEnd
    }

    public enum GuardState
    {
        Null = -1,
        Crouch,
        Normal,
        DataEnd
    }

    public enum DamageTag
    {
        Null = -1,
        Fall,
        NormalAttack,
        GreatAttack,
        DataEnd
    }

    public enum PushTag
    {
        Null = -1,
        Start,
        Pushing,
        DataEnd
    }

    public enum PhysicState
    {
        Null = -1,
        Land,
        Jump,
        DataEnd,
    }
}
