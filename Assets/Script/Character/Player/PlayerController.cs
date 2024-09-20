using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// �v���C���[�̑S�Ă̏������s���Ă���N���X
/// </summary>
public class PlayerController : CharacterController
{
    /// <summary>
    /// ScriptableObject�f�[�^
    /// �v���C���[�Ŏg���ϐ��̒l��ێ����Ă�
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
    /// �v���C���[�̕ǁA�R�Ƃ̔��菈�����܂Ƃ߂��N���X
    /// </summary>
    private ObstacleAction                   obstacleAction = null;
    public ObstacleAction                    GetObstacleCheck() { return obstacleAction; }
    
    
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
    /// �v���C���[�̍U���s������������N���X
    /// </summary>
    [SerializeField]
    private CharacterTagList.TripleAttack   tripleAttack = CharacterTagList.TripleAttack.Null;
    public CharacterTagList.TripleAttack    TripleAttack { get { return tripleAttack; } set {  tripleAttack = value; } }
    
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
        obstacleAction =        GetComponent<ObstacleAction>();
        keyInput =              GetComponent<PlayerInput>();
        toolInventory =         GetComponentInChildren<ToolInventoryController>();
        decorationController =  GetComponent<PlayerDecorationController>();
        soundController =       GetComponent<SoundController>();
        state =                 new PlayerState(this);
        commands =              new PlayerCommands(this);
        rotation =              new PlayerRotation(this,transform.rotation);
        timer =                 new PlayerTimers();
        animatorOverride =      new AnimatorOverrideController(animator.runtimeAnimatorController);

        if(obstacleAction == null)
        {
            Debug.LogError("ObstacleCheck���A�^�b�`����Ă��܂���");
        }
        else
        {
            obstacleAction.SetController(this);
        }

        if(keyInput == null)
        {
            Debug.LogError("PlayerInput���A�^�b�`����Ă��܂���");
        }
        else
        {
            keyInput.SetController(this);
            keyInput.Initialize();
        }

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

        if(decorationController == null)
        {
            Debug.LogError("PlayerDecorationController���A�^�b�`����Ă��܂���");
        }
        else
        {
            decorationController.SetController(this);
        }

        timer.InitializeAssignTimer();

        animator.runtimeAnimatorController = animatorOverride;
    }

    //���͏������s��
    protected override void Update()
    {
        keyInput.SystemInput();
        if (Time.timeScale <= 0) { return; }
        base.Update();
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
        obstacleAction.CliffJumpFlag = false;
        obstacleAction.SetGrabCancel(false);
        if (!timer.GetTimerNoAccele().IsEnabled()&&
            !timer.GetTimerJumpAttackAccele().IsEnabled())
        {
            characterStatus.Jumping = false;
        }
        characterStatus.SetJumpPower(0);
    }
    /// <summary>
    /// PhysicMaterial�𒅒n�̗L���ŕύX����֐�
    /// </summary>
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
    /// <summary>
    /// �v���C���[�I�u�W�F�N�g��MeshRenderer�̕\����ύX����֐�
    /// </summary>
    private void BodyRendererUpdate()
    {
        foreach (var renderer in rendererData.RendererList)
        {
            if (renderer.enabled == !cameraController.IsFPSMode()) { continue; }
            renderer.enabled = !cameraController.IsFPSMode();
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
        if(characterStatus.CurrentState == CharacterTagList.StateTag.ReadySpinAttack&& keyInput.Horizontal == 0 && keyInput.Vertical == 0) { return; }
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
        if (!timer.GetTimerNoAccele().IsEnabled())
        {
            Accele(GetCameraDirection(Camera.main.transform.forward), GetCameraDirection(Camera.main.transform.right),
                   data.MaxSpeed, data.Acceleration);
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
            vel += (h * right + v * forward) * SetSpeed(_accele);
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

    private const float LittleSpeed = 0.2f;
    private float SetSpeed(float _speed)
    {
        if(characterStatus.CurrentState == CharacterTagList.StateTag.ReadySpinAttack||
           characterStatus.CurrentState == CharacterTagList.StateTag.Push||
           cameraController.IsFPSMode())
        {
            _speed *= LittleSpeed;
        }
        return _speed;
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
