using UnityEngine;
using CharacterTagList;

public class PlayerInput : MonoBehaviour
{
    private PlayerController        controller = null;
    public void                     SetController(PlayerController _controller) {  controller = _controller; }

    /// <summary>
    /// XY�ړ�����
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
    /// �v���C���[�����ڂ��Ă鎞�̕����𔻒f����ϐ�
    /// </summary>
    [SerializeField]
    private DirectionTag            currentDirection;

    public DirectionTag             CurrentDirection { get { return currentDirection; }set { currentDirection = value; } }

    [SerializeField]
    private DirectionTag            pastDirection;

    public DirectionTag             PastDirection { get { return pastDirection; } set { pastDirection = value; } }

    /// <summary>
    /// �A�N�V�����{�^������
    /// </summary>
    [SerializeField]
    private bool                    actionButton = false;
    public bool                     ActionButton { get { return actionButton; }set { actionButton = value; } }
    //���[�����O���J�n�������̏����ʒu
    private Vector3                 initVelocity = Vector3.zero;
    public Vector3                  InitVelocity { get { return initVelocity; } set { initVelocity = value; } }

    private float                   rollTimer = 0.0f;
    public float                    RollTimer { get { return rollTimer; }set { rollTimer = value; } }

    /// <summary>
    /// �J�����Œ�{�^������
    /// </summary>
    [SerializeField]
    private bool                    lockCamera = false;

    [SerializeField]
    private bool                    cameraLockEnabled = false;

    public bool                     IsCameraLockEnabled() {  return cameraLockEnabled; }

    /// <summary>
    ///�؂�ւ��{�^������
    /// </summary>
    [SerializeField]
    private bool                    changeButton = false;
    public bool                     ChangeButton => changeButton;

    /// <summary>
    /// ����{�^������
    /// </summary>
    [SerializeField]
    private bool                    toolButton = false;
    public bool                     ToolButton => toolButton;

    /// <summary>
    /// �U���{�^������
    /// </summary>
    [SerializeField]
    private bool                    attackButton = false;
    public bool                     AttackButton { get { return attackButton; } set { attackButton = value; } }

    //�ʏ�̍U���J�E���g�ϐ�
    private byte                    threeAttackCount = 0;
    public byte                     ThreeAttackCount { get{ return threeAttackCount; }set{ threeAttackCount = value; } }

    [SerializeField]
    private bool                    attackHoldButton = false;
    public bool                     AttackHoldButton { get {return attackHoldButton; } set {attackHoldButton = value; } }

    /// <summary>
    /// �h��{�^������
    /// </summary>
    [SerializeField]
    private bool                    guardHoldButton = false;
    public bool                     GuardHoldButton => guardHoldButton;
    [SerializeField]
    private bool                    guardPushButton = false;
    public bool                     GuardPushButton => guardPushButton;

    /// <summary>
    /// �E��ɐݒ肷�铹��̃N���X
    /// </summary>
    private RightHandInput          rightHandInput = null;

    /// <summary>
    /// ����ɐݒ肷�铹��̃N���X
    /// </summary>
    private LeftHandInput           leftHandInput = null;

    /// <summary>
    /// �X�e�[�W�M�~�b�N�̃{�^������
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
        //�_���[�W����
        DamageInput();
        if (!controller.DeathFlag)
        {
            //���͏�����
            InitializeInput();
            if (!controller.GetCameraController().IsFPSMode())
            {
                //��������
                FallInput();
                //�������Ƃ��\�ȃI�u�W�F�N�g�ɑ΂��Ă̓���̓���
                PushInput();
                if (NoInputEnabled()) { return; }
                //�ҋ@����
                IdleInput();
                //�ړ�����
                RunInput();
                //��]����
                controller.GetRolling().Input();
            }
            else
            {
                //�ҋ@����
                IdleInput();
            }
            //���[�E�[������
            ModeChangeInput();
            //��Ɏ����Ă��铹��̓���
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
    //���̓L�[�̏�����
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
        //���͎��̂������ꍇ
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
        //���͂��̂܂܂Ŏ~�߂�ꍇ
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
        //�E�����
        rightHandInput.Execute();
        //�������
        leftHandInput.Execute();
    }
}
