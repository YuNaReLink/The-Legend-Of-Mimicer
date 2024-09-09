using UnityEngine;

[System.Serializable]
public class CharacterStatus
{
    /// <summary>
    /// 状態管理変数
    /// </summary>
    [Header("キャラクターの状態")]
    //状態のインスタンス
    [SerializeField]
    protected CharacterTagList.StateTag     currentState = CharacterTagList.StateTag.Null;
    public CharacterTagList.StateTag        CurrentState { get { return currentState; } set { currentState = value; } }
    [SerializeField]
    protected CharacterTagList.StateTag     pastState = CharacterTagList.StateTag.Null;
    public CharacterTagList.StateTag        PastState { get { return pastState; } set { pastState = value; } }

    /// <summary>
    /// HP関係の変数
    /// </summary>
    //最大HP
    [SerializeField]
    protected float                         maxHp = 0;
    public float                            GetMaxHP() { return maxHp; }
    public void                             SetMaxHP(float _maxHp) { maxHp = _maxHp; }
    //増減するHP
    [SerializeField]
    protected float                         hp = 0;
    public float                            HP { get { return hp; } set { hp = value; } }
    [SerializeField]
    protected bool                          death = false;
    public bool                             DeathFlag { get { return death; } set { death = value; } }

    /// <summary>
    /// 移動関連の変数
    /// </summary>
    [SerializeField]
    protected Vector3                       velocity = Vector3.zero;
    public Vector3                          Velocity { get { return velocity; } set { velocity = value; } }

    /// <summary>
    /// キャラクターの防御の状態を決めるeunmクラス
    /// </summary>
    [SerializeField]
    protected CharacterTagList.GuardState   guardState = CharacterTagList.GuardState.Null;
    public CharacterTagList.GuardState      GuardState { get { return guardState; } set { guardState = value; } }

    /// <summary>
    /// 動いているかいないかを判定するフラグ
    /// </summary>
    [SerializeField]
    protected bool                          input = false;
    public bool                             MoveInput { get { return input; } set { input = value; } }

    [SerializeField]
    protected bool                          landing = false;
    public bool                             Landing { get { return landing; } set { landing = value; } }
    /// <summary>
    /// 着地してる間取得する座標
    /// </summary>
    protected Vector3                       landingPosition = Vector3.zero;

    public Vector3                          GetLandingPosition() { return landingPosition; }
    public void                             SetLandingPosition(Vector3 _landingPosition) {  landingPosition = _landingPosition;}


    /// <summary>
    /// ジャンプフラグ
    /// </summary>
    [SerializeField]
    protected bool                          jumping = false;

    public bool                             Jumping { get { return jumping; } set { jumping = value; } }

    [SerializeField]
    protected float                         jumpPower = 0;
    public float                            JumpPower => jumpPower;
    public void                             SetJumpPower(float _jumpPower) { jumpPower = _jumpPower;}

    /// <summary>
    /// キャラクターの動きを停止させるフラグ
    /// </summary>
    [SerializeField]
    protected bool                          stopController = false;
    public bool                             StopController { get { return stopController; } set { stopController = value; } }
}
