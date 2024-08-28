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
    [SerializeField]
    private AnimationCurve                  rollCurve = null;
    public AnimationCurve                   GetRollCurve() { return rollCurve; }
    /// <summary>
    /// プレイヤーの攻撃行動を処理するクラス
    /// </summary>
    [SerializeField]
    private CharacterTag.TripleAttack       tripleAttack = CharacterTag.TripleAttack.Null;
    public CharacterTag.TripleAttack        TripleAttack { get { return tripleAttack; } set {  tripleAttack = value; } }
    /// <summary>
    /// プレイヤーのダメージ処理を行うクラス
    /// </summary>
    private PlayerDamageCommand             damage = null;
    public PlayerDamageCommand              GetDamage() { return damage; }
    /// <summary>
    /// プレイヤーの各右・左の道具の処理を生成するクラス
    /// </summary>
    private BaseToolAction                  rightAction = null;
    public BaseToolAction                   RightAction { get { return rightAction; }set { rightAction = value; } }
    private BaseToolAction                  leftAction = null;
    public BaseToolAction                   LeftAction { get {return leftAction; }set { leftAction = value; } }
    /// <summary>
    /// プレイヤーが戦闘状態かそうじゃないか
    /// </summary>
    [SerializeField]
    private bool                            battleMode = false;
    public bool                             BattleMode { get { return battleMode; }set { battleMode = value; } }
    /// <summary>
    /// プレイヤーが動かせるオブジェクトに触れてるか判定する
    /// </summary>
    [SerializeField]
    private CharacterTag.PushTag            pushTag = CharacterTag.PushTag.Null;
    public CharacterTag.PushTag             PushTag  => pushTag;
    /// <summary>
    /// パシフィックマテリアル
    /// </summary>
    [SerializeField]
    private List<PhysicMaterial>            physicMaterials = new List<PhysicMaterial>();
    /// <summary>
    /// ダメージ関係の変数
    /// </summary>
    [SerializeField]
    private CharacterTag.DamageTag          damageTag = CharacterTag.DamageTag.Null;
    public CharacterTag.DamageTag           DamageTag {  get { return damageTag; } set { damageTag = value; } }
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
    private PlayerSoundController           soundController = null;
    public PlayerSoundController            GetSoundController() { return soundController; }
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
            maxHp = data.MaxHP;
            hp = maxHp;
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
        keyInput.SetController(this);
        keyInput?.Initialize();


        fallDistanceCheck = new FallDistanceCheck(this);
        fallDistanceCheck?.Initialize();

        toolInventory = GetComponentInChildren<ToolInventoryController>();
        toolInventory?.SetController(this);
        toolInventory?.Initilaize();

        decorationController = GetComponent<PlayerDecorationController>();
        decorationController?.SetController(this);

        rotation = new PlayerRotation(this,transform.rotation);
        timer = new PlayerTimers();
        timer?.InitializeAssignTimer();

        rolling = new RollingCommand(this);

        damage = new PlayerDamageCommand(this);

        animatorOverride = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverride;

        soundController = GetComponent<PlayerSoundController>();
    }

    //入力処理を行う
    protected override void Update()
    {
        keyInput.SystemInput();
        if (Time.timeScale <= 0) { return; }
        //着地時の判定
        LandingCheck();
        if (obstacleCheck.IsSavePosition()&&landing)
        {
            landingPosition = transform.localPosition;
        }
        base.Update();

        if (stopController) { return; }
        //タイマーの更新
        timer.TimerUpdate();
        
        //障害物との当たり判定
        obstacleCheck.WallCheckInput();
        //入力の更新
        keyInput.UpdatePlayerInput();
        keyInput.UpdateGimicInput();


        //武器や盾の位置を状態によって変える
        toolInventory.UpdateTool();

        //特定のモーションを特定の条件で止めたり再生したりするメソッド
        motion.StopMotionCheck();
        //特定のモーション終了時に処理を行うメソッド
        motion.EndMotionNameCheck();
    }

    private void LandingCheck()
    {
        landing = groundCheck.CheckGroundStatus();

        //パシフィックマテリアル変更
        SetPhysicMaterial();

        if (!landing) { return; }

        obstacleCheck.CliffJumpFlag = false;
        obstacleCheck.GrabCancel = false;

        if (!timer.GetTimerRolling().IsEnabled()&&
            !timer.GetTimerNoAccele().IsEnabled()&&
            !timer.GetTimerJumpAttackAccele().IsEnabled())
        {
            jumping = false;
        }

        jumpPower = 0;
    }

    private void SetPhysicMaterial()
    {
        if (currentState != CharacterTag.StateTag.Grab &&  !landing)
        {
            characterCollider.material = physicMaterials[(int)CharacterTag.PhysicState.Jump];
        }
        else
        {
            characterCollider.material = physicMaterials[(int)CharacterTag.PhysicState.Land];
        }
    }

    //行動処理を行う
    private void FixedUpdate()
    {
        if (stopController) { return; }
        UpdateMoveInput();
        UpdateCommand();
    }

    protected override void UpdateMoveInput()
    {
        switch (currentState)
        {
            case CharacterTag.StateTag.GetUp:
            case CharacterTag.StateTag.Idle:
            case CharacterTag.StateTag.Grab:
            case CharacterTag.StateTag.ClimbWall:
            case CharacterTag.StateTag.Attack:
            case CharacterTag.StateTag.JumpAttack:
            case CharacterTag.StateTag.SpinAttack:
            case CharacterTag.StateTag.Gurid:
            case CharacterTag.StateTag.Damage:
            case CharacterTag.StateTag.Die:
                return;
        }
        if(currentState == CharacterTag.StateTag.ReadySpinAttack&& keyInput.Horizontal == 0 && keyInput.Vertical == 0) { return; }
        if(fallDistanceCheck.FallDamage || cameraController.IsFPSMode()) { return; }

        input = true;
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
        if (obstacleCheck != null)
        {
            obstacleCheck.Execute();
        }
        if(fallDistanceCheck != null)
        {
            fallDistanceCheck.Execute();
        }
        if (rolling != null)
        {
            rolling.Execute();
        }
        if(rightAction != null)
        {
            rightAction.Execute();
        }
        if(leftAction != null)
        {
            leftAction.Execute();
        }
        if(damage != null)
        {
            damage.Execute();
        }
        if(knockBackCommand != null)
        {
            knockBackCommand.Execute();
        }
        //移動処理
        if (!timer.GetTimerRolling().IsEnabled())
        {
            Accele(cameraForward, cameraRight, data.MaxSpeed, data.Acceleration);
        }
        //入力がなかった場合停止処理
        if (!input)
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
        switch (currentState)
        {
            case CharacterTag.StateTag.JumpAttack:
            case CharacterTag.StateTag.SpinAttack:
            case CharacterTag.StateTag.ReadySpinAttack:
            case CharacterTag.StateTag.Rolling:
            case CharacterTag.StateTag.Push:
            case CharacterTag.StateTag.Damage:
            case CharacterTag.StateTag.WallJump:
            case CharacterTag.StateTag.Grab:
            case CharacterTag.StateTag.ClimbWall:
            case CharacterTag.StateTag.Die:
            case CharacterTag.StateTag.GetUp:
                return true;
        }
        return false;
    }

    private void Accele(Vector3 forward, Vector3 right, float _maxspeed, float _accele)
    {
        Vector3 vel = velocity;
        float h = keyInput.Horizontal;
        float v = keyInput.Vertical;
        if (currentState == CharacterTag.StateTag.Jump||currentState == CharacterTag.StateTag.Rolling)
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
        velocity = vel;
    }

    private float SetAccele(float _accele)
    {
        if(currentState == CharacterTag.StateTag.ReadySpinAttack)
        {
            _accele *= 0.2f;
        }

        return _accele;
    }

    private float SetMaxSpeed(float _maxSpeed)
    {
        if (currentState == CharacterTag.StateTag.ReadySpinAttack)
        {
            _maxSpeed *= 0.2f;
        }
        return _maxSpeed;
    }

    public override void Death()
    {
        base.Death();
        vfxController.CreateVFX((int)EffectTagList.CharacterEffectTag.Death, transform.position, 1f, Quaternion.identity);
    }
    public override void RecoveryHelth(int count)
    {
        base.RecoveryHelth(count);
        soundController.PlaySESound((int)PlayerSoundController.PlayerSoundTag.GetHeart);
    }
    public void GetArrow(int count)
    {
        toolInventory.GetQuiver().AddArrow(count);
        soundController.PlaySESound((int)PlayerSoundController.PlayerSoundTag.GetItem);
    }

    private void SetPushState(CharacterTag.PushTag _pushTag){pushTag = _pushTag;}

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
        if (death) { return; }
        switch (other.tag)
        {
            case "Furniture":
                SetPushState(CharacterTag.PushTag.Start);
                break;
            case "Damage":
                DamageOrGuardCheck(other);
                break;
            default:
                fallDistanceCheck.CollisionEnter();
                if (!fallDistanceCheck.FallDamage) { return; }
                damageTag = CharacterTag.DamageTag.Fall;
                break;
        }
    }

    private void DamageOrGuardCheck(Collider other)
    {
        switch (guardState)
        {
            case CharacterTag.GuardState.Null:
                //ダメージ発生時の処理
                damageTag = CharacterTag.DamageTag.NormalAttack;
                damage.Attacker = other.gameObject;
                break;
            case CharacterTag.GuardState.Normal:
            case CharacterTag.GuardState.Crouch:
                knockBackCommand.KnockBackFlag = true;
                knockBackCommand.Attacker = other.gameObject;
                soundController.PlaySESound((int)PlayerSoundController.PlayerSoundTag.Guard);
                break;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (death) { return; }
        switch (collision.collider.tag)
        {
            case "Furniture":
                CharacterTag.PushTag tag = pushTag;
                if (keyInput.Vertical != 0 || keyInput.Horizontal != 0)
                {
                    switch (pushTag)
                    {
                        case CharacterTag.PushTag.Start:
                            tag = CharacterTag.PushTag.Pushing;
                            break;
                        case CharacterTag.PushTag.Null:
                            tag = CharacterTag.PushTag.Start;
                            break;
                    }
                }
                else
                {
                    tag = CharacterTag.PushTag.Null;
                }
                SetPushState(tag);
                break;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (death) { return; }
        switch (collision.collider.tag)
        {
            case "Furniture":
                SetPushState(CharacterTag.PushTag.Null);
                break;
            default:
                fallDistanceCheck.CollisionExit();
                break;
        }
    }

}
