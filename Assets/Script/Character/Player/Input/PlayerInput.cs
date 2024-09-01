using UnityEngine;
using CharacterTagList;

public class PlayerInput : MonoBehaviour
{
    private PlayerController        controller = null;
    public void                     SetController(PlayerController _controller) {  controller = _controller; }

    /// <summary>
    /// XY移動入力
    /// </summary>
    [SerializeField]
    private float                   horizontal = 0;

    public float                    Horizontal => horizontal;

    [SerializeField]
    private float                   vertical = 0;

    public float                    Vertical => vertical;

    [SerializeField]
    private bool                    upKey;
    public bool                     IsUpKey() { return upKey; }
    [SerializeField]
    private bool                    downKey;
    public bool                     IsDownKey() { return downKey; }
    [SerializeField]
    private bool                    leftKey;
    public bool                     IsLeftKey() { return leftKey; }
    [SerializeField]
    private bool                    rightKey;
    public bool                     IsRightKey() { return rightKey; }

    /// <summary>
    /// プレイヤーが注目してる時の方向を判断する変数
    /// </summary>
    [SerializeField]
    private DirectionTag            currentDirection;

    public DirectionTag             CurrentDirection { get { return currentDirection; }set { currentDirection = value; } }

    [SerializeField]
    private DirectionTag            pastDirection;

    public DirectionTag             PastDirection { get { return pastDirection; } set { pastDirection = value; } }

    /// <summary>
    /// アクションボタン入力
    /// </summary>
    [SerializeField]
    private bool                    actionButton = false;
    public bool                     ActionButton { get { return actionButton; }set { actionButton = value; } }
    //ローリングを開始した時の初期位置
    private Vector3                 initVelocity = Vector3.zero;
    public Vector3                  InitVelocity { get { return initVelocity; } set { initVelocity = value; } }

    private float                   rollTimer = 0.0f;
    public float                    RollTimer { get { return rollTimer; }set { rollTimer = value; } }

    /// <summary>
    /// カメラ固定ボタン入力
    /// </summary>
    [SerializeField]
    private bool                    lockCamera = false;

    [SerializeField]
    private bool                    cameraLockEnabled = false;

    public bool                     IsCameraLockEnabled() {  return cameraLockEnabled; }

    /// <summary>
    ///切り替えボタン入力
    /// </summary>
    [SerializeField]
    private bool                    changeButton = false;
    public bool                     ChangeButton => changeButton;

    /// <summary>
    /// 道具ボタン入力
    /// </summary>
    [SerializeField]
    private bool                    toolButton = false;
    public bool                     ToolButton => toolButton;

    /// <summary>
    /// 攻撃ボタン入力
    /// </summary>
    [SerializeField]
    private bool                    attackButton = false;
    public bool                     AttackButton { get { return attackButton; } set { attackButton = value; } }

    //通常の攻撃カウント変数
    private byte                    threeAttackCount = 0;
    public byte                     ThreeAttackCount { get{ return threeAttackCount; }set{ threeAttackCount = value; } }

    [SerializeField]
    private bool                    attackHoldButton = false;
    public bool                     AttackHoldButton { get {return attackHoldButton; } set {attackHoldButton = value; } }

    /// <summary>
    /// 防御ボタン入力
    /// </summary>
    [SerializeField]
    private bool                    guardHoldButton = false;
    public bool                     GuardHoldButton => guardHoldButton;
    [SerializeField]
    private bool                    guardPushButton = false;
    public bool                     GuardPushButton => guardPushButton;

    /// <summary>
    /// 右手に設定する道具のクラス
    /// </summary>
    private RightHandInput          rightHandInput = null;

    /// <summary>
    /// 左手に設定する道具のクラス
    /// </summary>
    private LeftHandInput           leftHandInput = null;

    /// <summary>
    /// ステージギミックのボタン入力
    /// </summary>
    [SerializeField]
    private bool                    getButton = false;
    public bool                     IsGetButton() {  return getButton; }

    public void Initialize()
    {
        rightHandInput = new RightHandInput(controller);
        leftHandInput = new LeftHandInput(controller);
        GetUpInput();
    }

    private bool NoInputEnabled()
    {
        bool noaccele = controller.GetTimer().GetTimerNoAccele().IsEnabled();
        bool rollingflag = controller.CurrentState == StateTag.Rolling && !controller.GetMotion().MotionEndCheck();
        bool falldamageflag = controller.CurrentState == StateTag.Damage;
        bool jumpattack = controller.CurrentState == StateTag.JumpAttack;
        bool pushflag = controller.CurrentState == StateTag.Push&&(controller.PushTag == PushTag.Start|| controller.PushTag == PushTag.Pushing);
        if (noaccele || rollingflag||
            falldamageflag||jumpattack||
            pushflag)
        {
            return true;
        }
        return false;
    }

    public void UpdatePlayerInput()
    {
        ForcedRelease();
        //ダメージ入力
        DamageInput();
        if (!controller.DeathFlag)
        {
            //入力初期化
            InitializeInput();
            if (!controller.GetCameraController().IsFPSMode())
            {
                //落下入力
                FallInput();
                //押すことが可能なオブジェクトに対しての動作の入力
                PushInput();
                if (NoInputEnabled()) { return; }
                //待機入力
                IdleInput();
                //移動入力
                RunInput();
                //回転入力
                controller.GetRolling().Input();
            }
            else
            {
                //待機入力
                IdleInput();
            }
            //収納・納刀入力
            ModeChangeInput();
            //手に持っている道具の入力
            HoldToolInput();
        }
    }
    public void SystemInput()
    {
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
    public void UpdateGimicInput()
    {
        getButton = Input.GetButtonDown("Get");
    }

    private void ForcedRelease()
    {
        switch (controller.CurrentState)
        {
            case StateTag.Attack:
            case StateTag.JumpAttack:
            case StateTag.SpinAttack:
                break;
            default:
                if(controller.TripleAttack != TripleAttack.Null)
                {
                    controller.TripleAttack = TripleAttack.Null;
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
        upKey = InputManager.UpButton();
        downKey = InputManager.DownButton();
        leftKey = InputManager.LeftButton();
        rightKey = InputManager.RightButton();
        if (!controller.GetTimer().GetTimerRolling().IsEnabled()&&
            controller.CurrentState != StateTag.Null)
        {
            actionButton = InputManager.ActionButton();
        }
        lockCamera = InputManager.LockCameraButton();
        if (lockCamera)
        {
            cameraLockEnabled = !cameraLockEnabled;
        }
        changeButton = InputManager.ChangeButton();
        toolButton = InputManager.ToolButton();
        attackButton = InputManager.AttackButton();
        attackHoldButton = InputManager.AttackHoldButton();
        guardHoldButton = InputManager.GuardHoldButton();
        guardPushButton = InputManager.GuardPushButton();
    }

    public void GetUpInput()
    {
        controller.GetMotion().ForcedChangeMotion(StateTag.GetUp);
    }

    private void DamageInput()
    {
        controller.GetDamage().Input();
    }

    private void FallInput()
    {
        switch (controller.CurrentState)
        {
            case StateTag.Jump:
            case StateTag.JumpAttack:
            case StateTag.Rolling:
            case StateTag.WallJump:
            case StateTag.Grab:
            case StateTag.Null:
                return;
        }
        if (controller.GetObstacleCheck().IsGrabFlag()) { return; }
        if(controller.GetObstacleCheck().IsClimbFlag()) { return; }
        if(controller.GetObstacleCheck().IsWallJumpFlag()) { return; }
        if (!controller.Landing)
        {
            controller.GetMotion().ChangeMotion(StateTag.Fall);
        }
    }

    private void SetHorizontalAndVertical()
    {
        //入力自体を消す場合
        switch (controller.CurrentState)
        {
            case StateTag.Damage:
            case StateTag.Gurid:
                vertical = 0f;
                horizontal = 0f;
                return;
            case StateTag.Grab:
            case StateTag.ClimbWall:
            case StateTag.WallJump:
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

    private void IdleInput()
    {
        switch (controller.CurrentState)
        {
            case StateTag.GetUp:
            case StateTag.Grab:
            case StateTag.Rolling:
            case StateTag.Attack:
            case StateTag.JumpAttack:
            case StateTag.ReadySpinAttack:
            case StateTag.SpinAttack:
                return;
            case StateTag.ChangeMode:
                if (!controller.GetCameraController().IsFPSMode())
                {
                    return;
                }
                break;
        }
        bool stopstate = controller.CurrentState == StateTag.Idle&&guardHoldButton;
        if (stopstate) { return; }
        if (!controller.Landing) { return; }

        bool horidleNoEnabled = horizontal >= 1 || horizontal <= -1;
        bool verIdleNoEnabled = vertical >= 1 || vertical <= -1;

        if (horidleNoEnabled||verIdleNoEnabled) { return; }
        controller.GetMotion().ChangeMotion(StateTag.Idle);
    }

    private void RunInput()
    {
        switch (controller.CurrentState)
        {
            case StateTag.GetUp:
            case StateTag.Grab:
            case StateTag.ClimbWall:
            case StateTag.Rolling:
            case StateTag.Attack:
            case StateTag.ReadySpinAttack:
            case StateTag.SpinAttack:
                return;
        }
        if (horizontal == 0&& vertical == 0) { return; }
        if (!cameraLockEnabled)
        {
            currentDirection = DirectionTag.Null;
            controller.GetMotion().ChangeMotion(StateTag.Run);
        }
        else
        {
            DirectionRunInput();
        }
    }

    private void DirectionRunInput()
    {
        if (horizontal > 0)
        {
            currentDirection = DirectionTag.Right;
        }
        if(horizontal < 0) 
        {
            currentDirection = DirectionTag.Left;
        }
        if(vertical > 0)
        {
            currentDirection = DirectionTag.Up;
        }
        if(vertical < 0)
        {
            currentDirection = DirectionTag.Down;
        }
        controller.MoveInput = true;
        controller.GetMotion().ChangeMotion(StateTag.Run);
    }

    private void ModeChangeInput()
    {
        switch(controller.CurrentState)
        {
            case StateTag.Idle:
            case StateTag.ChangeMode:
                break;
            default:
                return;
        }
        if (controller.MoveInput) { return; }
        if (!controller.Landing) { return; }
        if (!changeButton) { return;}
        if (controller.GetToolController().CheckNullToolObject(controller.GetToolController().GetInventoryData().ToolItemList[(int)ToolInventoryController.ToolObjectTag.Sword])){ return; }
        if (controller.GetToolController().CurrentToolTag == ToolInventoryController.ToolObjectTag.CrossBow) { return; }
        controller.BattleMode = !controller.BattleMode;
        controller.GetMotion().ChangeMotion(StateTag.ChangeMode);
    }

    private void PushInput()
    {
        if (!controller.Landing) { return; }
        if(controller.PushTag == PushTag.Null) { return; }
        controller.GetMotion().ChangeMotion(StateTag.Push);
    }

    private void HoldToolInput()
    {
        //右手入力
        rightHandInput.Execute();
        //左手入力
        leftHandInput.Execute();
    }
}
