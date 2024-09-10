using UnityEngine;
using System.Collections.Generic;


public class PlayerController : CharacterController
{
    /// <summary>
    /// ScriptableObject�f�[�^
    /// </summary>
    [SerializeField]
    private PlayerScriptableObject          data;
    public PlayerScriptableObject           GetData() { return data; }
    /// <summary>
    /// �J����������܂Ƃ߂��N���X
    /// </summary>
    [SerializeField]
    private CameraController                cameraController = null;
    public CameraController                 GetCameraController() { return cameraController; }
    /// <summary>
    /// �v���C���[�̃L�[���͂��܂Ƃ߂��N���X
    /// </summary>
    private PlayerInput                     keyInput = null;
    public PlayerInput                      GetKeyInput() { return keyInput; }

    private PlayerState                     state = null;
    /// <summary>
    /// �v���C���[�̕ǁA�R�Ƃ̔��菈�����܂Ƃ߂��N���X
    /// </summary>
    private ObstacleCheck                   obstacleCheck = null;
    public ObstacleCheck                    GetObstacleCheck() { return obstacleCheck; }
    /// <summary>
    /// �v���C���[�̗����������܂Ƃ߂��N���X
    /// </summary>
    private FallDistanceCheck               fallDistanceCheck = null;
    public FallDistanceCheck                GetFallDistanceCheck() {  return fallDistanceCheck; }
    /// <summary>
    /// �v���C���[�̓���̏������܂Ƃ߂��N���X
    /// </summary>
    private ToolInventoryController         toolInventory = null;
    public ToolInventoryController          GetToolController() { return toolInventory; }
    /// <summary>
    /// �v���C���[�̓���Ƃ͈Ⴄ�����i�̐ݒ���s���N���X
    /// </summary>
    private PlayerDecorationController      decorationController = null;
    /// <summary>
    /// �v���C���[�̉�]���܂Ƃ߂��N���X
    /// </summary>
    private PlayerRotation                  rotation = null;
    /// <summary>
    /// �v���C���[�Ŏg���^�C�}�[���܂Ƃ߂��N���X
    /// </summary>
    private PlayerTimers                    timer = null;
    public PlayerTimers                     GetTimer() {  return timer; }
    /// <summary>
    /// �v���C���[�̉���s������������N���X
    /// </summary>
    private RollingCommand                  rolling = null;
    public RollingCommand                   GetRolling() { return rolling; }
    [SerializeField]
    private AnimationCurve                  rollCurve = null;
    public AnimationCurve                   GetRollCurve() { return rollCurve; }
    /// <summary>
    /// �v���C���[�̍U���s������������N���X
    /// </summary>
    [SerializeField]
    private CharacterTagList.TripleAttack   tripleAttack = CharacterTagList.TripleAttack.Null;
    public CharacterTagList.TripleAttack    TripleAttack { get { return tripleAttack; } set {  tripleAttack = value; } }
    /// <summary>
    /// �v���C���[�̃_���[�W�������s���N���X
    /// </summary>
    private PlayerDamageCommand             damage = null;
    public PlayerDamageCommand              GetDamage() { return damage; }
    /// <summary>
    /// �v���C���[�̊e�E�E���̓���̏����𐶐�����N���X
    /// </summary>
    private InterfaceBaseToolCommand        rightAction = null;
    public InterfaceBaseToolCommand         RightAction { get { return rightAction; }set { rightAction = value; } }
    private InterfaceBaseToolCommand        leftAction = null;
    public InterfaceBaseToolCommand         LeftAction { get {return leftAction; }set { leftAction = value; } }
    /// <summary>
    /// �v���C���[����������I�u�W�F�N�g�ɐG��Ă邩���肷��
    /// </summary>
    [SerializeField]
    private CharacterTagList.PushTag        pushTag = CharacterTagList.PushTag.Null;
    public CharacterTagList.PushTag         PushTag  => pushTag;
    /// <summary>
    /// �p�V�t�B�b�N�}�e���A��
    /// </summary>
    [SerializeField]
    private List<PhysicMaterial>            physicMaterials = new List<PhysicMaterial>();
    /// <summary>
    /// �_���[�W�֌W�̕ϐ�
    /// </summary>
    [SerializeField]
    private CharacterTagList.DamageTag      damageTag = CharacterTagList.DamageTag.Null;
    public CharacterTagList.DamageTag       DamageTag {  get { return damageTag; } set { damageTag = value; } }
    [Header("�v���C���[�������\�������Ɏg�����[�V����Clip")]
    [SerializeField]
    private AnimationClip                   clip = null;
    public AnimationClip                    GetClip() {  return clip; }
    [SerializeField]
    private AnimationClip                   nullClip = null;
    public AnimationClip                    GetNullClip() { return nullClip; }
    /// <summary>
    /// �v���C���[�̃T�E���h�Ǘ��̃N���X
    /// </summary>
    private SoundController                 soundController = null;
    public SoundController                  GetSoundController() { return soundController; }
    protected override void                 SetMotionController(){motion = new PlayerMotion(this);}
    protected override void Awake()
    {
        base.Awake();
        InitializeAssign();
    }
    protected override void Start()
    {
        base.Start();
        //�T�E���h�R���g���[���[��Awake���̏�����
        soundController.AwakeInitilaize();
        if (data != null)
        {
            characterStatus.SetMaxHP(data.MaxHP);
            characterStatus.HP = characterStatus.GetMaxHP();
        }
    }

    protected override void InitializeAssign()
    {
        base.InitializeAssign();

        GameObject cameraObject = GameObject.FindWithTag("MainCamera");
        if(cameraObject != null)
        {
            cameraController = cameraObject.GetComponent<CameraController>();
        }

        obstacleCheck = GetComponent<ObstacleCheck>();
        obstacleCheck?.SetController(this);

        keyInput = GetComponent<PlayerInput>();
        if(keyInput == null)
        {
            Debug.LogError("PlayerInput���A�^�b�`����Ă��܂���");
        }
        keyInput?.SetController(this);
        keyInput?.Initialize();

        state = GetComponent<PlayerState>();
        if(state == null)
        {
            Debug.LogError("PlayerState���A�^�b�`����Ă��܂���");
        }
        state?.AwakeInitilaize();

        fallDistanceCheck = new FallDistanceCheck(this);
        fallDistanceCheck?.Initialize();

        toolInventory = GetComponentInChildren<ToolInventoryController>();
        if(toolInventory == null)
        {
            Debug.LogError("ToolInventoryController���A�^�b�`����Ă��܂���");
        }
        toolInventory?.SetController(this);
        toolInventory?.Initilaize();

        decorationController = GetComponent<PlayerDecorationController>();
        if(decorationController == null)
        {
            Debug.LogError("PlayerDecorationController���A�^�b�`����Ă��܂���");
        }
        decorationController?.SetController(this);

        rotation = new PlayerRotation(this,transform.rotation);

        timer = new PlayerTimers();
        timer?.InitializeAssignTimer();

        rolling = new RollingCommand(this);

        damage = new PlayerDamageCommand(this);

        animatorOverride = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverride;

        soundController = GetComponent<SoundController>();
    }

    //���͏������s��
    protected override void Update()
    {
        base.Update();

        keyInput.SystemInput();

        if (Time.timeScale <= 0) { return; }

        //�^�C�}�[�̍X�V
        timer.TimerUpdate();

        //���n���̔���
        LandingCheck();

        if(characterStatus.StopController) { return; }

        //��Q���Ƃ̓����蔻��
        obstacleCheck.WallCheckInput();

        //���͂̍X�V
        keyInput.UpdateInput();

        //�L�[�̓��͂ɂ�������Ԃɕω�
        state.StateUpdate();

        //����⏂�̈ʒu����Ԃɂ���ĕς���
        toolInventory.UpdateTool();

        //����̃��[�V���������̏����Ŏ~�߂���Đ������肷�郁�\�b�h
        motion.StopMotionCheck();

        //����̃��[�V�����I�����ɏ������s�����\�b�h
        motion.EndMotionNameCheck();
    }

    private void LandingCheck()
    {
        characterStatus.Landing = groundCheck.CheckGroundStatus();
        //�p�V�t�B�b�N�}�e���A���ύX
        SetPhysicMaterial();
        if (obstacleCheck.IsSavePosition() && characterStatus.Landing)
        {
            characterStatus.SetLandingPosition(transform.localPosition);
        }
        if (!characterStatus.Landing) { return; }
        obstacleCheck.CliffJumpFlag = false;
        obstacleCheck.SetGrabCancel(false);
        if (!timer.GetTimerNoAccele().IsEnabled()&&
            !timer.GetTimerJumpAttackAccele().IsEnabled())
        {
            characterStatus.Jumping = false;
        }
        characterStatus.SetJumpPower(0);
    }

    private void SetPhysicMaterial()
    {
        if (characterStatus.CurrentState != CharacterTagList.StateTag.Grab&&!characterStatus.Landing)
        {
            characterCollider.material = physicMaterials[(int)CharacterTagList.PhysicState.Jump];
        }
        else
        {
            characterCollider.material = physicMaterials[(int)CharacterTagList.PhysicState.Land];
        }
    }

    //�s���������s��
    private void FixedUpdate()
    {
        if (characterStatus.StopController) { return; }
        MoveStateCheck();
        UpdateCommand();
    }

    protected override void MoveStateCheck()
    {
        switch (characterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Idle:
            case CharacterTagList.StateTag.Grab:
            case CharacterTagList.StateTag.ClimbWall:
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.JumpAttack:
            case CharacterTagList.StateTag.SpinAttack:
            case CharacterTagList.StateTag.Gurid:
            case CharacterTagList.StateTag.Damage:
            case CharacterTagList.StateTag.Die:
            case CharacterTagList.StateTag.GetUp:
                return;
        }
        if(characterStatus.CurrentState == CharacterTagList.StateTag.ReadySpinAttack&& keyInput.Horizontal == 0 && keyInput.Vertical == 0) { return; }
        if(fallDistanceCheck.FallDamage) { return; }
        characterStatus.MoveInput = true;
    }

    public Vector3 GetCameraDirection(Vector3 dir)
    {
        return Vector3.Scale(dir, new Vector3(1, 0, 1)).normalized;
    }

    private void UpdateCommand()
    {
        //�J�����ɑ΂��đO���擾
        Vector3 cameraForward = GetCameraDirection(Camera.main.transform.forward);
        //�J�����ɑ΂��ĉE���擾
        Vector3 cameraRight = GetCameraDirection(Camera.main.transform.right);
        obstacleCheck?.Execute();
        rightAction?.Execute();
        leftAction?.Execute();
        //InterfaceBaseCommand,InterfaceBaseInput���������Ă�N���X��
        fallDistanceCheck?.Execute();
        rolling?.Execute();
        damage?.Execute();
        knockBackCommand?.Execute();
        //�ړ�����
        if (!timer.GetTimerNoAccele().IsEnabled())
        {
            Accele(cameraForward, cameraRight, data.MaxSpeed, data.Acceleration);
        }
        //���͂��Ȃ������ꍇ��~����
        if (!characterStatus.MoveInput)
        {
            StopMove();
        }
        //�ړ�RigidBody�ɓK�p
        Move();
        if(RotateStopFlag()) { return; }
        //�v���C���[���g�̉�]����
        transform.rotation = rotation.SelfRotation(this);
    }

    private bool RotateStopFlag()
    {
        switch (characterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Jump:
            case CharacterTagList.StateTag.JumpAttack:
            case CharacterTagList.StateTag.SpinAttack:
            case CharacterTagList.StateTag.ReadySpinAttack:
            case CharacterTagList.StateTag.Rolling:
            case CharacterTagList.StateTag.Push:
            case CharacterTagList.StateTag.Damage:
            case CharacterTagList.StateTag.WallJump:
            case CharacterTagList.StateTag.Grab:
            case CharacterTagList.StateTag.ClimbWall:
            case CharacterTagList.StateTag.Die:
            case CharacterTagList.StateTag.GetUp:
                return true;
        }
        return false;
    }

    private void Accele(Vector3 forward, Vector3 right, float _maxspeed, float _accele)
    {
        Vector3 vel = characterStatus.Velocity;
        float h = keyInput.Horizontal;
        float v = keyInput.Vertical;
        if (characterStatus.CurrentState == CharacterTagList.StateTag.Jump||characterStatus.CurrentState == CharacterTagList.StateTag.Rolling)
        {
            vel += transform.forward * _accele;
        }
        else
        {
            vel += (h * right + v * forward) * SetAccele(_accele);
        }
        // ���݂̑��x�̑傫�����v�Z
        float currentSpeed = vel.magnitude;
        // �������݂̑��x���ő呬�x�����Ȃ�΁A�����x��K�p����
        // ���݂̑��x���ő呬�x�ȏ�̏ꍇ�͑��x���ő呬�x�ɐ�������
        if (currentSpeed >= SetMaxSpeed(_maxspeed))
        {
            vel = vel.normalized * SetMaxSpeed(_maxspeed);
        }
        characterStatus.Velocity = vel;
    }

    private float SetAccele(float _accele)
    {
        if(characterStatus.CurrentState == CharacterTagList.StateTag.ReadySpinAttack||
           characterStatus.CurrentState == CharacterTagList.StateTag.Push||
           cameraController.IsFPSMode())
        {
            _accele *= 0.2f;
        }
        return _accele;
    }

    private float SetMaxSpeed(float _maxSpeed)
    {
        if (characterStatus.CurrentState == CharacterTagList.StateTag.ReadySpinAttack||
            characterStatus.CurrentState == CharacterTagList.StateTag.Push ||
            cameraController.IsFPSMode())
        {
            _maxSpeed *= 0.2f;
        }
        return _maxSpeed;
    }

    public override void Death()
    {
        base.Death();
        motion.ForcedChangeMotion(CharacterTagList.StateTag.Die);
        effectController.CreateVFX((int)EffectTagList.CharacterEffectTag.Death, transform.position, 1f, Quaternion.identity);
    }
    public override void RecoveryHelth(int count)
    {
        base.RecoveryHelth(count);
        soundController.PlaySESound((int)SoundTagList.PlayerSoundTag.GetHeart);
    }
    public void GetArrow(int count)
    {
        soundController.PlaySESound((int)SoundTagList.PlayerSoundTag.GetItem);
    }

    private void SetPushState(CharacterTagList.PushTag _pushTag){pushTag = _pushTag;}

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.collider);
    }
    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other);
    }

    private void HandleCollision(Collider other)
    {
        if (characterStatus.DeathFlag) { return; }
        switch (other.tag)
        {
            case "Damage":
                DamageOrGuardCheck(other);
                break;
            default:
                fallDistanceCheck.CollisionEnter();
                if (!fallDistanceCheck.FallDamage) { return; }
                damageTag = CharacterTagList.DamageTag.Fall;
                break;
        }
    }

    private void DamageOrGuardCheck(Collider other)
    {
        switch (characterStatus.GuardState)
        {
            case CharacterTagList.GuardState.Null:
                //�_���[�W�������̏���
                damageTag = CharacterTagList.DamageTag.NormalAttack;
                damage.Attacker = other.gameObject;
                break;
            case CharacterTagList.GuardState.Normal:
            case CharacterTagList.GuardState.Crouch:
                knockBackCommand.SetKnockBackFlag(true);
                knockBackCommand.SetAttacker(other.gameObject);
                soundController.PlaySESound((int)SoundTagList.PlayerSoundTag.Guard);
                break;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (characterStatus.DeathFlag) { return; }
        switch (collision.collider.tag)
        {
            case "Furniture":
                CharacterTagList.PushTag tag = pushTag;
                if ((keyInput.Vertical != 0 || keyInput.Horizontal != 0)&&
                    obstacleCheck.CameraForwardWallCheck())
                {
                    switch (pushTag)
                    {
                        case CharacterTagList.PushTag.Start:
                            tag = CharacterTagList.PushTag.Pushing;
                            break;
                        case CharacterTagList.PushTag.Null:
                            tag = CharacterTagList.PushTag.Start;
                            break;
                    }
                }
                else
                {
                    tag = CharacterTagList.PushTag.Null;
                }
                SetPushState(tag);
                break;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (characterStatus.DeathFlag) { return; }
        switch (collision.collider.tag)
        {
            case "Furniture":
                SetPushState(CharacterTagList.PushTag.Null);
                break;
            default:
                fallDistanceCheck.CollisionExit();
                break;
        }
    }

}
