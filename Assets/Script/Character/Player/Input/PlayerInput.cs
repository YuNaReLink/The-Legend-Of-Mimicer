using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterTag;

public class PlayerInput : MonoBehaviour
{


    [SerializeField]
    private PlayerController controller;
    public void SetController(PlayerController _controller) {  controller = _controller; }

    /// <summary>
    /// �}�E�X����
    /// </summary>

    /// <summary>
    /// XY�ړ�����
    /// </summary>
    [SerializeField]
    private float horizontal = 0;

    public float Horizontal {  get { return horizontal; } set { horizontal = value; } }

    [SerializeField]
    private float vertical = 0;

    public float Vertical { get { return vertical; } set { vertical = value; } }

    [SerializeField]
    private bool upKey;
    public bool IsUpKey() { return upKey; }
    [SerializeField]
    private bool downKey;
    public bool IsDownKey() { return downKey; }
    [SerializeField]
    private bool leftKey;
    public bool IsLeftKey() { return leftKey; }
    [SerializeField]
    private bool rightKey;
    public bool IsRightKey() { return rightKey; }

    /// <summary>
    /// �v���C���[�����ڂ��Ă鎞�̕����𔻒f����ϐ�
    /// </summary>
    [SerializeField]
    private DirectionTag currentDirection;

    public DirectionTag CurrentDirection { get { return currentDirection; }set { currentDirection = value; } }

    [SerializeField]
    private DirectionTag pastDirection;

    public DirectionTag PastDirection { get { return pastDirection; } set { pastDirection = value; } }

    /// <summary>
    /// �A�N�V�����{�^������
    /// </summary>
    [SerializeField]
    private bool actionButton = false;
    public bool ActionButton { get { return actionButton; }set { actionButton = value; } }
    //���[�����O���J�n�������̏����ʒu
    [SerializeField]
    private Vector3 initVelocity = Vector3.zero;
    public Vector3 InitVelocity { get { return initVelocity; } set { initVelocity = value; } }
    [SerializeField]
    private float rollTimer = 0.0f;
    public float RollTimer { get { return rollTimer; }set { rollTimer = value; } }


    private float magnitudeSpeed = 0;
    public float MagnitudeSpeed { get { return magnitudeSpeed; } set { magnitudeSpeed = value; } }


    /// <summary>
    /// �J�����Œ�{�^������
    /// </summary>
    [SerializeField]
    private bool lockCamera = false;
    public bool LockCamera {  get { return lockCamera; } set { lockCamera = value; } }

    [SerializeField]
    private bool cameraLockEnabled = false;

    public bool IsCameraLockEnabled() {  return cameraLockEnabled; }

    /// <summary>
    ///�؂�ւ��{�^������
    /// </summary>
    [SerializeField]
    private bool changeButton = false;
    public bool ChangeButton { get {return changeButton; } set { changeButton = value; } }

    /// <summary>
    /// ����{�^������
    /// </summary>
    [SerializeField]
    private bool toolButton = false;
    public bool ToolButton { get { return toolButton; } set { toolButton = value; } }

    /// <summary>
    /// �U���{�^������
    /// </summary>
    [SerializeField]
    private bool attackButton = false;
    public bool AttackButton { get { return attackButton; } set { attackButton = value; } }

    //�ʏ�̍U���J�E���g�ϐ�
    private byte threeAttackCount = 0;
    public byte ThreeAttackCount { get{ return threeAttackCount; }set{ threeAttackCount = value; } }

    [SerializeField]
    private bool attackHoldButton = false;
    public bool AttackHoldButton { get {return attackHoldButton; } set {attackHoldButton = value; } }

    /// <summary>
    /// �h��{�^������
    /// </summary>
    [SerializeField]
    private bool guardHoldButton = false;
    public bool GuardHoldButton { get { return guardHoldButton; } set { guardHoldButton = value; } }

    /// <summary>
    /// �E��ɐݒ肷�铹��̃N���X
    /// </summary>
    private RightHandInput rightHandInput = null;

    /// <summary>
    /// ����ɐݒ肷�铹��̃N���X
    /// </summary>
    private LeftHandInput leftHandInput = null;

    /// <summary>
    /// �X�e�[�W�M�~�b�N�̃{�^������
    /// </summary>
    [Header("�M�~�b�N�֌W�̃L�[")]
    [SerializeField]
    private bool getButton = false;
    public bool IsGetButton() {  return getButton; }

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
                RollingInput();
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

    public void UpdateSound()
    {
        switch (controller.CurrentState)
        {
            case StateTag.Attack:
                AnimatorStateInfo animInfo = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
                if (attackButton&& controller.BattleMode)
                    controller.GetSoundController().PlaySESound((int)PlayerSoundController.PlayerSoundTag.FirstAttack);
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
    }

    public void GetUpInput()
    {
        controller.GetMotion().ChangeMotion(StateTag.GetUp);
    }

    private void DamageInput()
    {
        controller.GetDamage().Input();
    }

    private void FallInput()
    {
        bool stopstate = controller.CurrentState == StateTag.Jump || controller.CurrentState == StateTag.JumpAttack ||
            controller.CurrentState == StateTag.Rolling || controller.CurrentState == StateTag.Null;
        if (stopstate) { return; }
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
        bool stopinput = controller.CurrentState == StateTag.Damage|| controller.CurrentState == StateTag.Gurid;
        if (stopinput) {
            vertical = 0f;
            horizontal = 0f;
            return; 
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

    private void RollingInput()
    {
        switch (controller.CurrentState)
        {
            case StateTag.Rolling:
            case StateTag.Attack:
            case StateTag.JumpAttack:
            case StateTag.ReadySpinAttack:
            case StateTag.SpinAttack:
            case StateTag.GetUp:
                return;
        }
        if (!controller.Landing) { return; }
        if (controller.GetMotion().IsEndRollingMotionNameCheck()) { return; }
        if (!actionButton) { return; }
        if (!cameraLockEnabled) 
        {
            //���S�Ɏ~�܂��Ă����烊�^�[��
            if (vertical == 0&&horizontal == 0) { return; }
            currentDirection = DirectionTag.Up;
            //���[�����O�̈ړ����s�����߂̏����x����
            initVelocity = controller.CharacterRB.velocity;
            rollTimer = 0.0f;

            controller.GetTimer().GetTimerRolling().StartTimer(0.4f);
            controller.GetTimer().GetTimerNoAccele().StartTimer(0.4f);
            controller.GetMotion().ChangeMotion(StateTag.Rolling);
            //Shift�L�[�𖳌��ɂ���
            actionButton = false;
        }
        else
        {
            if (controller.BattleMode && (vertical > 0|| vertical == 0 && horizontal == 0)) { return; }
            if (vertical > 0 || vertical == 0 && horizontal == 0)
            {
                currentDirection = DirectionTag.Up;
            }
            DirectionRollingInput();
        }
    }

    private void DirectionRollingInput()
    {
        float rollingcount = 0.4f;
        float noaccele = 0.4f;
        if (horizontal > 0)
        {
            currentDirection = DirectionTag.Right;
        }
        if (horizontal < 0)
        {
            currentDirection = DirectionTag.Left;
        }
        if (vertical < 0&& horizontal == 0)
        {
            rollingcount = 0.5f;
            noaccele = 0.5f;
            currentDirection = DirectionTag.Down;
        }
        initVelocity = controller.CharacterRB.velocity;
        rollTimer = 0.0f;
        controller.GetTimer().GetTimerRolling().StartTimer(rollingcount);
        controller.GetTimer().GetTimerNoAccele().StartTimer(noaccele);
        controller.GetMotion().ChangeMotion(StateTag.Rolling);
        //Shift�L�[�𖳌��ɂ���
        actionButton = false;
    }

    private void ModeChangeInput()
    {
        switch(controller.CurrentState)
        {
            case StateTag.Idle:
                break;
            default:
                return;
        }
        if (controller.MoveInput) { return; }
        if (!controller.Landing) { return; }
        if (!changeButton) { return;}
        if(controller.GetToolController().CheckNullToolObject(controller.GetToolController().GetInventoryData().ToolItemList[(int)ToolInventoryController.ToolObjectTag.Sword])){ return; }
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
