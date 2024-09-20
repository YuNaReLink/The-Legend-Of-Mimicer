using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController                        controller = null;
    public void SetController(PlayerController _controller)
    {
        controller = _controller; 
    }
    /// <summary>
    /// XY�ړ�����
    /// </summary>
    [SerializeField]
    private float                                   horizontal = 0;
    public float                                    Horizontal => horizontal;
    [SerializeField]
    private float                                   vertical = 0;
    public float                                    Vertical => vertical;
    /// <summary>
    /// �v���C���[�����ڂ��Ă鎞�̕����𔻒f����ϐ�
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
    /// �A�N�V�����{�^������
    /// </summary>
    [SerializeField]
    private bool                                    actionButton = false;
    public bool                                     ActionButton 
                                                    { get { return actionButton; }set { actionButton = value; } }
    //���[�����O���J�n�������̏����ʒu
    private Vector3                                 initVelocity = Vector3.zero;
    public Vector3                                  InitVelocity 
                                                    { get { return initVelocity; } set { initVelocity = value; } }
    private float                                   rollTimer = 0.0f;
    public float                                    RollTimer 
                                                    { get { return rollTimer; }set { rollTimer = value; } }
    /// <summary>
    /// �J�����Œ�{�^������
    /// </summary>
    [SerializeField]
    private bool                                    lockCamera = false;
    [SerializeField]
    private bool                                    cameraLockEnabled = false;
    public bool                                     IsCameraLockEnabled() {  return cameraLockEnabled; }
    /// <summary>
    ///�؂�ւ��{�^������
    /// </summary>
    [SerializeField]
    private bool                                    changeButton = false;
    public bool                                     ChangeButton => changeButton;
    /// <summary>
    /// ����{�^������
    /// </summary>
    [SerializeField]
    private bool                                    toolButton = false;
    public bool                                     ToolButton => toolButton;
    /// <summary>
    /// �U���{�^������
    /// </summary>
    [SerializeField]
    private bool                                    attackButton = false;
    public bool                                     AttackButton 
                                                    { get { return attackButton; } set { attackButton = value; } }
    //�ʏ�̍U���J�E���g�ϐ�
    private byte                                    threeAttackCount = 0;
    public byte                                     ThreeAttackCount 
                                                    { get{ return threeAttackCount; }set{ threeAttackCount = value; } }
    [SerializeField]
    private bool                                    attackHoldButton = false;
    public bool                                     AttackHoldButton 
                                                    { get {return attackHoldButton; } set {attackHoldButton = value; } }
    /// <summary>
    /// �h��{�^������
    /// </summary>
    [SerializeField]
    private bool                                    guardHoldButton = false;
    public bool                                     GuardHoldButton => guardHoldButton;
    [SerializeField]
    private bool                                    guardPushButton = false;
    public bool                                     GuardPushButton => guardPushButton;
    /// <summary>
    /// �X�e�[�W�M�~�b�N�̃{�^������
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
        //���͏�����
        InitializeInput();
        ForcedRelease();
    }

    /// <summary>
    /// �v���C���[�ł͂Ȃ��Q�[���S�̂Ɋ֌W������͂��s���֐�
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
    //���̓L�[�̏�����
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
        //���͎��̂������ꍇ
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
        //���͂��̂܂܂Ŏ~�߂�ꍇ
        if (controller.GetObstacleCheck().CliffJumpFlag){return;}
        horizontal = InputManager.HorizontalInput();
        vertical = InputManager.VerticalInput();
    }
}
