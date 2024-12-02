using UnityEngine;

/// <summary>
/// �v���C���[�̑S�Ă̏������s���Ă���N���X
/// </summary>
public class PlayerController : CharacterController
{
    //�O������A�^�b�`����ꗗ
    /// <summary>
    /// ScriptableObject�f�[�^
    /// �v���C���[�Ŏg���ϐ��̒l��ێ����Ă�
    /// </summary>
    [SerializeField]
    private PlayerScriptableObject          data;
    public PlayerScriptableObject           GetData() { return data; }
    /// <summary>
    /// �v���C���[�̓���̏������܂Ƃ߂��N���X
    /// </summary>
    private ToolInventoryController         toolInventory = null;
    public ToolInventoryController          GetToolController() { return toolInventory; }
    //Serializable��Controller���ɂ���N���X
    /// <summary>
    /// �v���C���[�̃L�[���͂��܂Ƃ߂��N���X
    /// </summary>
    [SerializeField]
    private PlayerInput                     keyInput;
    public PlayerInput                      GetKeyInput() { return keyInput; }
    /// <summary>
    /// �v���C���[�̕ǁA�R�Ƃ̔��菈�����܂Ƃ߂��N���X
    /// </summary>
    [SerializeField]
    private ObstacleAction                   obstacleAction = null;
    public ObstacleAction                    GetObstacleCheck() { return obstacleAction; }
    /// <summary>
    /// �v���C���[�̓���Ƃ͈Ⴄ�����i�̐ݒ���s���N���X
    /// </summary>
    [SerializeField]
    private PlayerDecorationController      decorationController = null;
    //��������N���X�̈ꗗ
    /// <summary>
    /// �v���C���[��Ԃ��Ǘ�����N���X
    /// </summary>
    private PlayerState                     state = null;
    /// <summary>
    /// �v���C���[�̍s�������s����N���X
    /// </summary>
    private PlayerCommands                  commands = null;
    public PlayerCommands                   GetCommands() { return commands; }
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
    /// �v���C���[�̊e�E�E���̓���̏����𐶐�����N���X
    /// </summary>
    private InterfaceBaseToolCommand        rightAction = null;
    public InterfaceBaseToolCommand         RightAction { get { return rightAction; } set { rightAction = value; } }
    private InterfaceBaseToolCommand        leftAction = null;
    public InterfaceBaseToolCommand         LeftAction { get { return leftAction; } set { leftAction = value; } }

    //�v���C���[�Ŏg���^�O
    /// <summary>
    /// �v���C���[�̍U���s������������N���X
    /// </summary>
    [SerializeField]
    private CharacterTagList.TripleAttack   tripleAttack = CharacterTagList.TripleAttack.Null;
    public CharacterTagList.TripleAttack    TripleAttack { get { return tripleAttack; } set {  tripleAttack = value; } }
    /// <summary>
    /// �v���C���[����������I�u�W�F�N�g�ɐG��Ă邩���肷��
    /// </summary>
    [SerializeField]
    private CharacterTagList.PushTag        pushTag = CharacterTagList.PushTag.Null;
    public CharacterTagList.PushTag         PushTag  => pushTag;
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


    private const float                     LittleSpeed = 0.2f;
    protected override void Awake()
    {
        base.Awake();
        InitializeAssign();
    }
    protected override void Start()
    {
        base.Start();

        obstacleAction.Setup(this);

        //�T�E���h�R���g���[���[��Awake���̏�����
        soundController.AwakeInitilaize();
        if (data != null)
        {
            characterStatus.SetMaxHP(data.MaxHP);
            characterStatus.HP = characterStatus.GetMaxHP();
        }
    }
    /// <summary>
    /// Awake�ōs���v���C���[�̏�����
    /// </summary>
    protected override void InitializeAssign()
    {
        base.InitializeAssign();
        toolInventory =         GetComponentInChildren<ToolInventoryController>();
        soundController =       GetComponent<SoundController>();
        state =                 new PlayerState(this);
        commands =              new PlayerCommands(this);
        rotation =              new PlayerRotation(this,transform.rotation);
        timer =                 new PlayerTimers();
        animatorOverride =      new AnimatorOverrideController(animator.runtimeAnimatorController);

        obstacleAction.Setup(this);
        keyInput.Setup(this);
        keyInput.Initialize();
        state.AwakeInitilaize();
        commands.AwakeInitilaize();
        if(toolInventory == null)
        {
            Debug.LogError("ToolInventoryController���A�^�b�`����Ă��܂���");
        }
        else
        {
            toolInventory.SetController(this);
            toolInventory.Initilaize();
        }
        decorationController.Setup(this);
        timer.InitializeAssignTimer();
        animator.runtimeAnimatorController = animatorOverride;
    }
    //���͏������s��
    protected override void Update()
    {
        keyInput.SystemInput();
        if (Time.timeScale <= 0) { return; }
        base.Update();
        decorationController.Update();
        //�^�C�}�[�̍X�V
        timer.TimerUpdate();
        //���n���̔���
        LandingCheck();
        //�v���C���[�̑̂�\���A��\���ɂ��鏈��
        BodyRendererUpdate();
        //�O������v���C���[���~�߂鏈�����Ăяo���ꂽ�烊�^�[��
        if (characterStatus.StopController) { return; }
        //��Q���Ƃ̓����蔻��
        obstacleAction.WallCheckInput();
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
    /// <summary>
    /// ���n�A�󒆂ōs���������s��
    /// </summary>
    private void LandingCheck()
    {
        characterStatus.Landing = groundCheck.CheckGroundStatus();
        //�p�V�t�B�b�N�}�e���A���ύX
        SetPhysicMaterial();
        if (obstacleAction.IsSavePosition() && characterStatus.Landing)
        {
            characterStatus.SetLandingPosition(transform.localPosition);
        }
        if (!characterStatus.Landing) { return; }
        obstacleAction.SetGrabCancel(false);
        if (!timer.GetTimerNoAccele().IsEnabled()&&
            !timer.GetTimerJumpAttackAccele().IsEnabled())
        {
            characterStatus.Jumping = false;
        }
        characterStatus.SetJumpPower(0);
    }

    /// <summary>
    /// �v���C���[�I�u�W�F�N�g��MeshRenderer�̕\����ύX����֐�
    /// </summary>
    private void BodyRendererUpdate()
    {
        foreach (var renderer in rendererData.RendererList)
        {
            if (renderer.enabled == !CameraController.Instance.IsFPSMode()) { continue; }
            renderer.enabled = !CameraController.Instance.IsFPSMode();
        }
    }
    //�s���������s��
    private void FixedUpdate()
    {
        if (characterStatus.StopController) { return; }
        MoveStateCheck();
        UpdateCommand();
    }
    /// <summary>
    /// �v���C���[�������������Ȃ�������ԂŌ��߂�֐�
    /// </summary>
    protected override void MoveStateCheck()
    {
        bool noMoveInput =  characterStatus.CurrentState == CharacterTagList.StateTag.Idle ||
                            characterStatus.CurrentState == CharacterTagList.StateTag.Grab ||
                            characterStatus.CurrentState == CharacterTagList.StateTag.ClimbWall ||
                            characterStatus.CurrentState == CharacterTagList.StateTag.Attack ||
                            characterStatus.CurrentState == CharacterTagList.StateTag.JumpAttack ||
                            characterStatus.CurrentState == CharacterTagList.StateTag.SpinAttack ||
                            characterStatus.CurrentState == CharacterTagList.StateTag.Gurid ||
                            characterStatus.CurrentState == CharacterTagList.StateTag.Damage ||
                            characterStatus.CurrentState == CharacterTagList.StateTag.Die ||
                            characterStatus.CurrentState == CharacterTagList.StateTag.GetUp;
        if (noMoveInput) { return; }
        if(characterStatus.CurrentState == CharacterTagList.StateTag.ReadySpinAttack&& keyInput.Horizontal == 0 &&
           keyInput.Vertical == 0) { return; }
        if(commands.GetFallDistanceCheck().FallDamage) { return; }
        characterStatus.MoveInput = true;
    }
    /// <summary>
    /// �J�����̕�������㉺���E���擾����֐�
    /// </summary>
    /// <param name="dir">
    /// �J�����̑O�A����Vector3
    /// </param>
    /// <returns></returns>
    public Vector3 GetCameraDirection(Vector3 dir)
    {
        return Vector3.Scale(dir, new Vector3(1, 0, 1)).normalized;
    }
    /// <summary>
    /// �v���C���[�̍s�������s���鏈��������֐�
    /// �v���C���[�̈ړ����~�A��]���s��
    /// </summary>
    private void UpdateCommand()
    {
        //MonoBehaviour���p�����Ă�N���X�̏���
        obstacleAction?.Execute();
        //����interface(InterfaceBaseToolCommand)����錾���Ă�N���X�̏���
        rightAction?.Execute();
        leftAction?.Execute();
        //����������InterfaceBaseCommand�����������N���X���܂Ƃ߂ď������Ă�N���X�̏���
        commands.DoUpdate();
        //�p������Character�Ő錾���Ă�N���X�̏���
        knockBackCommand?.Execute();
        //�ړ�����
        Accele(GetCameraDirection(Camera.main.transform.forward), GetCameraDirection(Camera.main.transform.right),
                                  data.MaxSpeed, data.Acceleration);
        //���͂��Ȃ������ꍇ��~����
        StopMove();
        //�ړ�RigidBody�ɓK�p
        Move();
        if(RotateStopFlag()) { return; }
        //�v���C���[���g�̉�]����
        transform.rotation = rotation.SelfRotation(this);
    }
    /// <summary>
    /// �v���C���[����]���邩����ԂŌ��߂�֐�
    /// </summary>
    /// <returns></returns>
    private bool RotateStopFlag()
    {
        bool noRotate = characterStatus.CurrentState == CharacterTagList.StateTag.Jump ||
                        characterStatus.CurrentState == CharacterTagList.StateTag.JumpAttack ||
                        characterStatus.CurrentState == CharacterTagList.StateTag.SpinAttack ||
                        characterStatus.CurrentState == CharacterTagList.StateTag.ReadySpinAttack ||
                        characterStatus.CurrentState == CharacterTagList.StateTag.Rolling ||
                        characterStatus.CurrentState == CharacterTagList.StateTag.Push ||
                        characterStatus.CurrentState == CharacterTagList.StateTag.Damage ||
                        characterStatus.CurrentState == CharacterTagList.StateTag.WallJump ||
                        characterStatus.CurrentState == CharacterTagList.StateTag.Grab ||
                        characterStatus.CurrentState == CharacterTagList.StateTag.ClimbWall ||
                        characterStatus.CurrentState == CharacterTagList.StateTag.Die ||
                        characterStatus.CurrentState == CharacterTagList.StateTag.GetUp;
        if (noRotate) { return true; }
        return false;
    }
    /// <summary>
    /// �ړ��������s���֐�
    /// </summary>
    /// <param name="forward">
    /// �J�������猩�đO�̐��l
    /// </param>
    /// <param name="right">
    /// �J�������猩�ĉE�̐��l
    /// </param>
    /// <param name="_maxspeed">
    /// �ő�X�s�[�h
    /// </param>
    /// <param name="_accele">
    /// ������
    /// </param>
    private void Accele(Vector3 forward, Vector3 right, float _maxspeed, float _accele)
    {
        if (timer.GetTimerNoAccele().IsEnabled()) { return; }
        Vector3 vel = characterStatus.Velocity;
        float h = keyInput.Horizontal;
        float v = keyInput.Vertical;
        if (characterStatus.CurrentState == CharacterTagList.StateTag.Jump||characterStatus.CurrentState == CharacterTagList.StateTag.Rolling)
        {

            vel = transform.forward * _accele;
        }
        else
        {
            vel = (h * right + v * forward) * SetSpeed(_accele);
        }
        // ���݂̑��x�̑傫�����v�Z
        float currentSpeed = vel.magnitude;
        // �������݂̑��x���ő呬�x�����Ȃ�΁A�����x��K�p����
        // ���݂̑��x���ő呬�x�ȏ�̏ꍇ�͑��x���ő呬�x�ɐ�������
        if (currentSpeed >= SetSpeed(_maxspeed))
        {
            vel = vel.normalized * SetSpeed(_maxspeed);
        }
        characterStatus.Velocity = vel;
    }
    public override void StopMove()
    {
        if (characterStatus.MoveInput) { return; }
        base.StopMove();
    }
    /// <summary>
    /// �ړ��Ŏg���X�s�[�h����Ԃɂ���ĕύX����֐�
    /// </summary>
    /// <param name="_speed">
    /// �ő�A�����X�s�[�h����
    /// </param>
    /// <returns></returns>
    private float SetSpeed(float _speed)
    {
        if(characterStatus.CurrentState == CharacterTagList.StateTag.ReadySpinAttack||
           characterStatus.CurrentState == CharacterTagList.StateTag.Push||
           CameraController.Instance.IsFPSMode())
        {
            _speed *= LittleSpeed;
        }
        return _speed;
    }
    /// <summary>
    /// �v���C���[�̎��S�����֐�
    /// </summary>
    public override void Death()
    {
        base.Death();
        motion.ForcedChangeMotion(CharacterTagList.StateTag.Die);
        effectController.CreateVFX((int)EffectTagList.CharacterEffectTag.Death, transform.position, 1f, Quaternion.identity);
    }
    /// <summary>
    /// ���C�t�񕜏����֐�
    /// </summary>
    /// <param name="count"></param>
    public override void RecoveryHelth(int count)
    {
        base.RecoveryHelth(count);
        soundController.PlaySESound((int)SoundTagList.PlayerSoundTag.GetHeart);
    }
    /// <summary>
    /// ����擾�������ɏ�������SE����
    /// </summary>
    /// <param name="count"></param>
    public void GetArrowSound(int count)
    {
        soundController.PlaySESound((int)SoundTagList.PlayerSoundTag.GetItem);
    }
    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.collider);
    }
    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other);
    }
    /// <summary>
    /// �R���C�_�[�ɑ��̃R���C�_�[�������������̏���
    /// </summary>
    /// <param name="other"></param>
    private void HandleCollision(Collider other)
    {
        if (characterStatus.DeathFlag) { return; }
        switch (other.tag)
        {
            case "Damage":
                DamageOrGuardCheck(other);
                break;
            default:
                commands.GetFallDistanceCheck().CollisionEnter();
                if (!commands.GetFallDistanceCheck().FallDamage) { return; }
                damageTag = CharacterTagList.DamageTag.Fall;
                break;
        }
    }
    /// <summary>
    /// �_���[�W���󂯂邩���肷��֐�
    /// </summary>
    /// <param name="other"></param>
    private void DamageOrGuardCheck(Collider other)
    {
        switch (characterStatus.GuardState)
        {
            case CharacterTagList.GuardState.Null:
                //�_���[�W�������̏���
                damageTag = CharacterTagList.DamageTag.NormalAttack;
                commands.Damage.Attacker = other.gameObject;
                break;
            case CharacterTagList.GuardState.Normal:
            case CharacterTagList.GuardState.Crouch:
                knockBackCommand.SetKnockBackFlag(true);
                knockBackCommand.SetAttacker(other.gameObject);
                soundController.PlaySESound((int)SoundTagList.PlayerSoundTag.Guard);
                break;
        }
    }
    /// <summary>
    /// ������ԃ^�O�̕ύX���s���֐�
    /// </summary>
    /// <param name="_pushTag"></param>
    private void SetPushState(CharacterTagList.PushTag _pushTag){pushTag = _pushTag;}
    private void OnCollisionStay(Collision collision)
    {
        if (characterStatus.DeathFlag) { return; }
        switch (collision.collider.tag)
        {
            case "Furniture":
                CharacterTagList.PushTag tag = pushTag;
                if ((keyInput.Vertical != 0 || keyInput.Horizontal != 0)&&
                    obstacleAction.CameraForwardWallCheck())
                {
                    if(pushTag == CharacterTagList.PushTag.Start)
                    {
                        tag = CharacterTagList.PushTag.Pushing;
                    }
                    else if(pushTag == CharacterTagList.PushTag.Null)
                    {
                        tag = CharacterTagList.PushTag.Start;
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
                commands.GetFallDistanceCheck().CollisionExit();
                break;
        }
    }
}
