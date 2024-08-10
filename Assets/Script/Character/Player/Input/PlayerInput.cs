using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterTag;

public class PlayerInput : MonoBehaviour
{


    [SerializeField]
    private PlayerController controller;


    public void SetController(PlayerController _controller) {  controller = _controller; }
    //private PlayerMotion motion;
    //
    //public PlayerMotion GetMotion() { return motion; }

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
    /// Shift�L�[����
    /// </summary>
    [SerializeField]
    private bool shiftKey = false;
    public bool ShiftKey { get { return shiftKey; }set { shiftKey = value; } }
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
    /// C�L�[����
    /// </summary>
    [SerializeField]
    private bool cKey = false;
    public bool CKey {  get { return cKey; } set { cKey = value; } }

    [SerializeField]
    private bool cKeyEnabled = false;

    public bool IsCKeyEnabled() {  return cKeyEnabled; }

    /// <summary>
    ///Q�L�[����
    /// </summary>
    [SerializeField]
    private bool qKey = false;
    public bool QKey { get {return qKey; } set { qKey = value; } }

    /// <summary>
    /// E�L�[����
    /// </summary>
    [SerializeField]
    private bool eKey = false;
    public bool EKey { get { return eKey; } set { eKey = value; } }

    /// <summary>
    /// ���N���b�N����
    /// </summary>
    [SerializeField]
    private bool leftMouseDownClick = false;
    public bool LeftMouseDownClick { get { return leftMouseDownClick; } set { leftMouseDownClick = value; } }
    //�ʏ�̍U���J�E���g�ϐ�
    private byte threeAttackCount = 0;
    public byte ThreeAttackCount { get{ return threeAttackCount; }set{ threeAttackCount = value; } }

    [SerializeField]
    private bool leftMouseClick = false;
    public bool LeftMouseClick { get {return leftMouseClick; } set {leftMouseClick = value; } }

    /// <summary>
    /// �E�N���b�N����
    /// </summary>
    [SerializeField]
    private bool rightMouseClick = false;
    public bool RightMouseClick { get { return rightMouseClick; } set { rightMouseClick = value; } }

    /// <summary>
    /// �E��ɐݒ肷�铹��̃N���X
    /// </summary>
    private RightHandInput rightHandInput = null;

    /// <summary>
    /// ����ɐݒ肷�铹��̃N���X
    /// </summary>
    private LeftHandInput leftHandInput = null;

    /// <summary>
    /// �X�e�[�W�M�~�b�N�֌W�̃L�[����
    /// </summary>
    [Header("�M�~�b�N�֌W�̃L�[")]
    [SerializeField]
    private bool fKey = false;
    public bool IsFKey() {  return fKey; }

    public void Initialize()
    {

        rightHandInput = new RightHandInput(controller);
        leftHandInput = new LeftHandInput(controller);
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
            //���[�E�[������
            ModeChangeInput();
            //��Ɏ����Ă��铹��̓���
            HoldToolInput();
        }
    }

    public void UpdateGimicInput()
    {
        fKey = InputManager.PushFKey();
    }

    //���̓L�[�̏�����
    private void InitializeInput()
    {
        SetHorizontalAndVertical();

        upKey = InputManager.PushWKey();
        downKey = InputManager.PushSKey();
        leftKey = InputManager.PushAKey();
        rightKey = InputManager.PushDKey();

        if (!controller.GetTimer().GetTimerRolling().IsEnabled()&&
            controller.CurrentState != StateTag.Null)
        {
            shiftKey = InputManager.PushShiftKey();
        }
        
        
        cKey = InputManager.PushCKey();

        if (cKey)
        {
            cKeyEnabled = !cKeyEnabled;
        }

        qKey = InputManager.PushQKey();

        eKey = InputManager.PushEKey();

        leftMouseDownClick = InputManager.PushMouseLeft();
        leftMouseClick = InputManager.HoldMouseLeft();

        rightMouseClick = InputManager.HoldMouseRight();
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
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void IdleInput()
    {
        switch (controller.CurrentState)
        {
            case StateTag.Grab:
            case StateTag.Rolling:
            case StateTag.Attack:
            case StateTag.JumpAttack:
            case StateTag.ReadySpinAttack:
            case StateTag.SpinAttack:
                return;
        }
        bool stopstate = controller.CurrentState == StateTag.Idle&&rightMouseClick;
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
            case StateTag.Grab:
            case StateTag.ClimbWall:
            case StateTag.Rolling:
            case StateTag.Attack:
            case StateTag.ReadySpinAttack:
            case StateTag.SpinAttack:
                return;
        }
        if (horizontal == 0&& vertical == 0) { return; }
        if (!cKeyEnabled)
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
        bool stopstate = controller.CurrentState == StateTag.Rolling || controller.CurrentState == StateTag.Rolling||
            controller.CurrentState == StateTag.Attack||controller.CurrentState == StateTag.JumpAttack||
            controller.CurrentState == StateTag.ReadySpinAttack||controller.CurrentState == StateTag.SpinAttack;
        if (stopstate) { return; }
        if (!controller.Landing) { return; }
        if (controller.GetMotion().IsEndRollingMotionNameCheck()) { return; }
        if (!shiftKey) { return; }
        if (!cKeyEnabled) 
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
            shiftKey = false;
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
        shiftKey = false;
    }

    private void ModeChangeInput()
    {
        if (controller.MoveInput) { return; }
        if (!controller.Landing) { return; }
        if (!qKey) { return;}
        if(controller.GetToolController().CurrentToolTag == PlayerToolController.ToolObjectTag.Bow) { return; }
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
        rightHandInput.Execute();

        leftHandInput.Execute();
    }
}
