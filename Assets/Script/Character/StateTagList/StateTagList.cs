
/// <summary>
/// Characterクラスを継承したクラスで使用するタグをまとめたnamespace
/// </summary>
namespace CharacterTagList
{
    //キャラクター共通で使うタグ
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
    //プレイヤーで使う移動方向を判定するタグ
    public enum DirectionTag
    {
        Null = -1,
        Up,
        Down,
        Left,
        Right,
        DataEnd
    }
    //プレイヤーの三段攻撃を判定するタグ
    public enum TripleAttack
    {
        Null = -1,
        First,
        Second,
        Third,
        DataEnd
    }
    //プレイヤーのガード状態を判定するタグ
    public enum GuardState
    {
        Null = -1,
        Crouch,
        Normal,
        DataEnd
    }
    //キャラクターのダメージを判別するタグ
    public enum DamageTag
    {
        Null = -1,
        Fall,
        NormalAttack,
        DataEnd
    }
    //プレイヤーがオブジェクトを押す状態を判別させるタグ
    public enum PushTag
    {
        Null = -1,
        Start,
        Pushing,
        DataEnd
    }
    //キャラクターが着地してるかしてないかでPhysicMaterialを変更するために判別するタグ
    public enum PhysicState
    {
        Null = -1,
        Land,
        Jump,
        DataEnd,
    }
}
