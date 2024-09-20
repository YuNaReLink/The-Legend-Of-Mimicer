using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController                        controller = null;
    public void SetController(PlayerController _controller)
    {
        controller = _controller; 
    }
    /// <summary>
    /// XY移動入力
    /// </summary>
    [SerializeField]
    private float                                   horizontal = 0;
    public float                                    Horizontal => horizontal;
    [SerializeField]
    private float                                   vertical = 0;
    public float                                    Vertical => vertical;
    /// <summary>
    /// プレイヤーが注目してる時の方向を判断する変数
    /// </summary>
    [SerializeField]
    private CharacterTagList.DirectionTag            currentDirection;
    public CharacterTagList.DirectionTag             CurrentDirection 
                                                    { get { return currentDirection; }set { currentDirection = value; } }
    [SerializeField]
    private CharacterTagList.DirectionTag            pastDirection;
    public CharacterTagList.DirectionTag             PastDirection 
                                                    { get { return pastDirection; } set { pastDirection = value; } }
    /// <summary>
    /// アクションボタン入力
    /// </summary>
    [SerializeField]
    private bool                                    actionButton = false;
    public bool                                     ActionButton 
                                                    { get { return actionButton; }set { actionButton = value; } }
    //ローリングを開始した時の初期位置
    private Vector3                                 initVelocity = Vector3.zero;
    public Vector3                                  InitVelocity 
                                                    { get { return initVelocity; } set { initVelocity = value; } }
    private float                                   rollTimer = 0.0f;
    public float                                    RollTimer 
                                                    { get { return rollTimer; }set { rollTimer = value; } }
    /// <summary>
    /// カメラ固定ボタン入力
    /// </summary>
    [SerializeField]
    private bool                                    lockCamera = false;
    [SerializeField]
    private bool                                    cameraLockEnabled = false;
    public bool                                     IsCameraLockEnabled() {  return cameraLockEnabled; }
    /// <summary>
    ///切り替えボタン入力
    /// </summary>
    [SerializeField]
    private bool                                    changeButton = false;
    public bool                                     ChangeButton => changeButton;
    /// <summary>
    /// 道具ボタン入力
    /// </summary>
    [SerializeField]
    private bool                                    toolButton = false;
    public bool                                     ToolButton => toolButton;
    /// <summary>
    /// 攻撃ボタン入力
    /// </summary>
    [SerializeField]
    private bool                                    attackButton = false;
    public bool                                     AttackButton 
                                                    { get { return attackButton; } set { attackButton = value; } }
    //通常の攻撃カウント変数
    private byte                                    threeAttackCount = 0;
    public byte                                     ThreeAttackCount 
                                                    { get{ return threeAttackCount; }set{ threeAttackCount = value; } }
    [SerializeField]
    private bool                                    attackHoldButton = false;
    public bool                                     AttackHoldButton 
                                                    { get {return attackHoldButton; } set {attackHoldButton = value; } }
    /// <summary>
    /// 防御ボタン入力
    /// </summary>
    [SerializeField]
    private bool                                    guardHoldButton = false;
    public bool                                     GuardHoldButton => guardHoldButton;
    [SerializeField]
    private bool                                    guardPushButton = false;
    public bool                                     GuardPushButton => guardPushButton;
    /// <summary>
    /// ステージギミックのボタン入力
    /// </summary>
    [SerializeField]
    private bool                                    getButton = false;
    public bool                                     IsGetButton() {  return getButton; }
    public void Initialize()
    {
        GetUpInput();
    }

    public void UpdateInput()
    {
        //入力初期化
        InitializeInput();
        ForcedRelease();
    }

    /// <summary>
    /// プレイヤーではなくゲーム全体に関係する入力を行う関数
    /// </summary>
    public void SystemInput()
    {
        if (GetItemMessage.Instance.ItemData != null) { return; }
        if (InputManager.MenuButton())
        {
            switch (GameManager.GameState)
            {
                case GameManager.GameStateEnum.Game:
                    GameManager.GameState = GameManager.GameStateEnum.Pose;
                    break;
                case GameManager.GameStateEnum.Pose:
                    GameManager.GameState = GameManager.GameStateEnum.Game;
                    break;
            }
        }
    }

    private void ForcedRelease()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.JumpAttack:
            case CharacterTagList.StateTag.SpinAttack:
                break;
            default:
                if(controller.TripleAttack != CharacterTagList.TripleAttack.Null)
                {
                    controller.TripleAttack = CharacterTagList.TripleAttack.Null;
                }
                if(threeAttackCount > 0)
                {
                    threeAttackCount = 0;
                }
                break;
        }
    }
    //入力キーの初期化
    private void InitializeInput()
    {
        SetHorizontalAndVertical();
        changeButton        = InputManager.ChangeButton();
        toolButton          = InputManager.ToolButton();
        attackButton        = InputManager.AttackButton();
        attackHoldButton    = InputManager.AttackHoldButton();
        guardHoldButton     = InputManager.GuardHoldButton();
        guardPushButton     = InputManager.GuardPushButton();
        lockCamera          = InputManager.LockCameraButton();
        getButton           = Input.GetButtonDown("Get");
        if (lockCamera)
        {
            cameraLockEnabled = !cameraLockEnabled;
        }
        if (!controller.GetTimer().GetTimerRolling().IsEnabled()&&
            controller.CharacterStatus.CurrentState != CharacterTagList.StateTag.Null)
        {
            actionButton = InputManager.ActionButton();
        }
    }

    public void GetUpInput()
    {
        controller.GetMotion().ForcedChangeMotion(CharacterTagList.StateTag.GetUp);
    }
    private void SetHorizontalAndVertical()
    {
        //入力自体を消す場合
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Damage:
            case CharacterTagList.StateTag.Gurid:
                vertical = 0f;
                horizontal = 0f;
                return;
            case CharacterTagList.StateTag.Grab:
            case CharacterTagList.StateTag.ClimbWall:
            case CharacterTagList.StateTag.WallJump:
                if (!controller.GetCameraController().IsCameraVerticalRotation())
                {
                    vertical = 0f;
                    horizontal = 0f;
                    return;
                }
                break;
        }
        //入力そのままで止める場合
        if (controller.GetObstacleCheck().CliffJumpFlag){return;}
        horizontal = InputManager.HorizontalInput();
        vertical = InputManager.VerticalInput();
    }
}
