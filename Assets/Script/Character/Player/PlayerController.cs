using UnityEngine;
using System.Collections.Generic;


public class PlayerController : CharacterController
{
    /// <summary>
    /// ScriptableObjectデータ
    /// </summary>
    [SerializeField]
    private PlayerScriptableObject          data;
    public PlayerScriptableObject           GetData() { return data; }
    /// <summary>
    /// カメラ制御をまとめたクラス
    /// </summary>
    [SerializeField]
    private CameraController                cameraController = null;
    public CameraController                 GetCameraController() { return cameraController; }
    /// <summary>
    /// プレイヤーのキー入力をまとめたクラス
    /// </summary>
    private PlayerInput                     keyInput = null;
    public PlayerInput                      GetKeyInput() { return keyInput; }

    private PlayerState                     state = null;
    /// <summary>
    /// プレイヤーの壁、崖との判定処理をまとめたクラス
    /// </summary>
    private ObstacleCheck                   obstacleCheck = null;
    public ObstacleCheck                    GetObstacleCheck() { return obstacleCheck; }
    /// <summary>
    /// プレイヤーの落下処理をまとめたクラス
    /// </summary>
    private FallDistanceCheck               fallDistanceCheck = null;
    public FallDistanceCheck                GetFallDistanceCheck() {  return fallDistanceCheck; }
    /// <summary>
    /// プレイヤーの道具の処理をまとめたクラス
    /// </summary>
    private ToolInventoryController         toolInventory = null;
    public ToolInventoryController          GetToolController() { return toolInventory; }
    /// <summary>
    /// プレイヤーの道具とは違う装飾品の設定を行うクラス
    /// </summary>
    private PlayerDecorationController      decorationController = null;
    /// <summary>
    /// プレイヤーの回転をまとめたクラス
    /// </summary>
    private PlayerRotation                  rotation = null;
    /// <summary>
    /// プレイヤーで使うタイマーをまとめたクラス
    /// </summary>
    private PlayerTimers                    timer = null;
    public PlayerTimers                     GetTimer() {  return timer; }
    /// <summary>
    /// プレイヤーの回避行動を処理するクラス
    /// </summary>
    private RollingCommand                  rolling = null;
    public RollingCommand                   GetRolling() { return rolling; }
    [SerializeField]
    private AnimationCurve                  rollCurve = null;
    public AnimationCurve                   GetRollCurve() { return rollCurve; }
    /// <summary>
    /// プレイヤーの攻撃行動を処理するクラス
    /// </summary>
    [SerializeField]
    private CharacterTagList.TripleAttack   tripleAttack = CharacterTagList.TripleAttack.Null;
    public CharacterTagList.TripleAttack    TripleAttack { get { return tripleAttack; } set {  tripleAttack = value; } }
    /// <summary>
    /// プレイヤーのダメージ処理を行うクラス
    /// </summary>
    private PlayerDamageCommand             damage = null;
    public PlayerDamageCommand              GetDamage() { return damage; }
    /// <summary>
    /// プレイヤーの各右・左の道具の処理を生成するクラス
    /// </summary>
    private InterfaceBaseToolCommand        rightAction = null;
    public InterfaceBaseToolCommand         RightAction { get { return rightAction; }set { rightAction = value; } }
    private InterfaceBaseToolCommand        leftAction = null;
    public InterfaceBaseToolCommand         LeftAction { get {return leftAction; }set { leftAction = value; } }
    /// <summary>
    /// プレイヤーが動かせるオブジェクトに触れてるか判定する
    /// </summary>
    [SerializeField]
    private CharacterTagList.PushTag        pushTag = CharacterTagList.PushTag.Null;
    public CharacterTagList.PushTag         PushTag  => pushTag;
    /// <summary>
    /// パシフィックマテリアル
    /// </summary>
    [SerializeField]
    private List<PhysicMaterial>            physicMaterials = new List<PhysicMaterial>();
    /// <summary>
    /// ダメージ関係の変数
    /// </summary>
    [SerializeField]
    private CharacterTagList.DamageTag      damageTag = CharacterTagList.DamageTag.Null;
    public CharacterTagList.DamageTag       DamageTag {  get { return damageTag; } set { damageTag = value; } }
    [Header("プレイヤーが盾を構えた時に使うモーションClip")]
    [SerializeField]
    private AnimationClip                   clip = null;
    public AnimationClip                    GetClip() {  return clip; }
    [SerializeField]
    private AnimationClip                   nullClip = null;
    public AnimationClip                    GetNullClip() { return nullClip; }
    /// <summary>
    /// プレイヤーのサウンド管理のクラス
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
        //サウンドコントローラーのAwake時の初期化
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
            Debug.LogError("PlayerInputがアタッチされていません");
        }
        keyInput?.SetController(this);
        keyInput?.Initialize();

        state = GetComponent<PlayerState>();
        if(state == null)
        {
            Debug.LogError("PlayerStateがアタッチされていません");
        }
        state?.AwakeInitilaize();

        fallDistanceCheck = new FallDistanceCheck(this);
        fallDistanceCheck?.Initialize();

        toolInventory = GetComponentInChildren<ToolInventoryController>();
        if(toolInventory == null)
        {
            Debug.LogError("ToolInventoryControllerがアタッチされていません");
        }
        toolInventory?.SetController(this);
        toolInventory?.Initilaize();

        decorationController = GetComponent<PlayerDecorationController>();
        if(decorationController == null)
        {
            Debug.LogError("PlayerDecorationControllerがアタッチされていません");
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

    //入力処理を行う
    protected override void Update()
    {
        base.Update();

        keyInput.SystemInput();

        if (Time.timeScale <= 0) { return; }

        //タイマーの更新
        timer.TimerUpdate();

        //着地時の判定
        LandingCheck();

        if(characterStatus.StopController) { return; }

        //障害物との当たり判定
        obstacleCheck.WallCheckInput();

        //入力の更新
        keyInput.UpdateInput();

        //キーの入力にあった状態に変化
        state.StateUpdate();

        //武器や盾の位置を状態によって変える
        toolInventory.UpdateTool();

        //特定のモーションを特定の条件で止めたり再生したりするメソッド
        motion.StopMotionCheck();

        //特定のモーション終了時に処理を行うメソッド
        motion.EndMotionNameCheck();
    }

    private void LandingCheck()
    {
        characterStatus.Landing = groundCheck.CheckGroundStatus();
        //パシフィックマテリアル変更
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

    //行動処理を行う
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
        //カメラに対して前を取得
        Vector3 cameraForward = GetCameraDirection(Camera.main.transform.forward);
        //カメラに対して右を取得
        Vector3 cameraRight = GetCameraDirection(Camera.main.transform.right);
        obstacleCheck?.Execute();
        rightAction?.Execute();
        leftAction?.Execute();
        //InterfaceBaseCommand,InterfaceBaseInputを実装してるクラス↓
        fallDistanceCheck?.Execute();
        rolling?.Execute();
        damage?.Execute();
        knockBackCommand?.Execute();
        //移動処理
        if (!timer.GetTimerNoAccele().IsEnabled())
        {
            Accele(cameraForward, cameraRight, data.MaxSpeed, data.Acceleration);
        }
        //入力がなかった場合停止処理
        if (!characterStatus.MoveInput)
        {
            StopMove();
        }
        //移動RigidBodyに適用
        Move();
        if(RotateStopFlag()) { return; }
        //プレイヤー自身の回転処理
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
        // 現在の速度の大きさを計算
        float currentSpeed = vel.magnitude;
        // もし現在の速度が最大速度未満ならば、加速度を適用する
        // 現在の速度が最大速度以上の場合は速度を最大速度に制限する
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
                //ダメージ発生時の処理
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
