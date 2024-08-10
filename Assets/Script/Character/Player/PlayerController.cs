using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Windows;
using CharacterTag;
using System.Collections.Generic;


public class PlayerController : CharacterController
{
    /// <summary>
    /// ScriptableObjectデータ
    /// </summary>
    [SerializeField]
    private PlayerScriptableObject data;
    public PlayerScriptableObject GetData() { return data; }

    [SerializeField]
    private GameObject cameraObject = null;
    public GameObject GetCameraObject() {  return cameraObject; }
    [SerializeField]
    private PlayerCameraController cameraScript = null;
    public PlayerCameraController GetCameraScript() { return cameraScript; }

    [SerializeField]
    private PlayerInput keyInput = null;

    public PlayerInput GetKeyInput() { return keyInput; }

    [SerializeField]
    private ObstacleCheck obstacleCheck = null;

    public ObstacleCheck GetObstacleCheck() { return obstacleCheck; }

    private FallDistanceCheck fallDistanceCheck = null;

    public FallDistanceCheck GetFallDistanceCheck() {  return fallDistanceCheck; }

    [SerializeField]
    private PlayerToolController toolController = null;
    public PlayerToolController GetToolController() { return toolController; }

    [SerializeField]
    private PlayerDecorationController decorationController = null;

    private PlayerRotation rotation = null;

    private PlayerTimers timer = null;
    public PlayerTimers GetTimer() {  return timer; }

    /// <summary>
    /// プレイヤーの回避行動を処理するクラス
    /// </summary>
    private RollingCommand rolling;
    [SerializeField]
    private AnimationCurve rollCurve;
    public AnimationCurve GetRollCurve() { return rollCurve; }
    /// <summary>
    /// プレイヤーの攻撃行動を処理するクラス
    /// </summary>
    [SerializeField]
    private TripleAttack tripleAttack = TripleAttack.Null;

    public TripleAttack TripleAttack { get { return tripleAttack; } set {  tripleAttack = value; } }

    /// <summary>
    /// プレイヤーのダメージ処理を行うクラス
    /// </summary>
    private PlayerDamageCommand damage = null;
    public PlayerDamageCommand GetDamage() { return damage; }

    /// <summary>
    /// プレイヤーの各右・左の道具の処理を生成するクラス
    /// </summary>
    [SerializeField]
    private BaseToolCommand rightCommand = null;

    public BaseToolCommand RightCommand { get { return rightCommand; }set { rightCommand = value; } }

    [SerializeField]
    private BaseToolCommand leftCommand = null;
    public BaseToolCommand LeftCommand { get {return leftCommand; }set { leftCommand = value; } }


    /// <summary>
    /// プレイヤーが戦闘状態かそうじゃないか
    /// </summary>
    private bool battleMode = false;
    public bool BattleMode { get { return battleMode; }set { battleMode = value; } }

    /// <summary>
    /// プレイヤーが動かせるオブジェクトに触れてるか判定する
    /// </summary>
    [SerializeField]
    private PushTag pushTag = PushTag.Null;
    public PushTag PushTag { get { return pushTag; } set { pushTag = value; } }

    /// <summary>
    /// パシフィックマテリアル
    /// </summary>
    [SerializeField]
    private List<PhysicMaterial> physicMaterials = new List<PhysicMaterial>();

    /// <summary>
    /// ダメージ関係の変数
    /// </summary>
    [SerializeField]
    private DamageTag damageTag = DamageTag.Null;

    public DamageTag DamageTag {  get { return damageTag; } set { damageTag = value; } }

    [Header("プレイヤーが盾を構えた時に使うモーションClip")]
    [SerializeField]
    private AnimationClip clip = null;
    public AnimationClip GetClip() {  return clip; }
    [SerializeField]
    private AnimationClip nullClip = null;
    public AnimationClip GetNullClip() { return nullClip; }


    /// <summary>
    /// プレイヤーのステージとのギミックのフラグをまとめてるクラス
    /// </summary>
    private PlayerGimicController gimicController = null;
    public PlayerGimicController GimicController() {  return gimicController; }

    protected override void Awake()
    {
        base.Awake();
        InitializeAssign();
    }

    protected override void SetMotionController()
    {
        motion = new PlayerMotion(this);
    }

    protected override void Start()
    {
        base.Start();

        if (data != null)
        {
            maxHp = data.MaxHP;
            hp = maxHp;
        }
    }

    protected override void InitializeAssign()
    {
        base.InitializeAssign();

        cameraObject = GameObject.FindWithTag("MainCamera");
        cameraScript = cameraObject.GetComponent<PlayerCameraController>();

        keyInput = GetComponent<PlayerInput>();
        keyInput.SetController(this);
        keyInput?.Initialize();

        obstacleCheck = GetComponent<ObstacleCheck>();
        obstacleCheck?.SetController(this);

        fallDistanceCheck = new FallDistanceCheck(this);
        fallDistanceCheck?.Initialize();

        toolController = GetComponent<PlayerToolController>();
        toolController?.SetController(this);
        toolController?.Initilaize();

        decorationController = GetComponent<PlayerDecorationController>();
        decorationController?.SetController(this);

        rotation = new PlayerRotation();
        timer = new PlayerTimers();
        timer?.InitializeAssignTimer();

        rolling = new RollingCommand(this);

        damage = new PlayerDamageCommand(this);

        animatorOverride = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverride;


        gimicController = new PlayerGimicController();
    }

    //入力処理を行う
    protected override void Update()
    {
        base.Update();

        if (stopController) { return; }
        //タイマーの更新
        timer.TimerUpdate();
        
        //着地時の判定
        LandingCheck();
        //障害物との当たり判定
        obstacleCheck.WallCheckInput();
        //入力の更新
        keyInput.UpdatePlayerInput();
        keyInput.UpdateGimicInput();


        //武器や盾の位置を状態によって変える
        toolController.UpdateProps();

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
        if (currentState != StateTag.Grab &&  !landing)
        {
            characterCollider.material = physicMaterials[(int)PhysicState.Jump];
        }
        else
        {
            characterCollider.material = physicMaterials[(int)PhysicState.Land];
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
            case StateTag.Idle:
            case StateTag.Grab:
            case StateTag.ClimbWall:
            case StateTag.Attack:
            case StateTag.JumpAttack:
            case StateTag.SpinAttack:
            case StateTag.Gurid:
            case StateTag.Damage:
            case StateTag.Die:
                return;
        }
        if(currentState == StateTag.ReadySpinAttack&& keyInput.Horizontal == 0 && keyInput.Vertical == 0) { return; }
        if(fallDistanceCheck.FallDamage || cameraScript.IsFPSMode()) { return; }

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

        if(rightCommand != null)
        {
            rightCommand.Execute();
        }
        if(leftCommand != null)
        {
            leftCommand.Execute();
        }
        if(damage != null)
        {
            damage.Execute();
        }
        //移動処理
        if (!timer.GetTimerRolling().IsEnabled())
        {
            Accele(cameraForward, cameraRight, data.MaxSpeed, data.Acceleration);
        }
        //入力がなかった場合停止処理
        if (!input)
        {
            Decele();
        }
        //移動RigidBodyに適用
        Move();
        //プレイヤー自身の回転処理
        if (!input) { return; }
        if(currentState == StateTag.ReadySpinAttack) { return; }
        rotation.MathPlayerPos(transform);
        if (rotation.GetCameraVelocity() == Vector3.zero) { return; }
        if (obstacleCheck.IsMoveDirectionWallHitFlag()) { return; }
        if(currentState == StateTag.Push) { return; }
        if(currentState == StateTag.Damage) { return; }
        transform.rotation = rotation.SelfRotation(this);
    }

    private void Accele(Vector3 forward, Vector3 right, float _maxspeed, float _accele)
    {
        Vector3 vel = velocity;
        float h = keyInput.Horizontal;
        float v = keyInput.Vertical;
        if (currentState == StateTag.Jump||currentState == StateTag.Rolling)
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
        if(currentState == StateTag.ReadySpinAttack)
        {
            _accele *= 0.2f;
        }

        return _accele;
    }

    private float SetMaxSpeed(float _maxSpeed)
    {
        if (currentState == StateTag.ReadySpinAttack)
        {
            _maxSpeed *= 0.2f;
        }
        return _maxSpeed;
    }

    public void Decele()
    {
        velocity = StopMoveVelocity();
        characterRB.velocity =  StopRigidBodyVelocity();
    }

    public override void Death()
    {
        base.Death();
    }

    private void SetPushState(PushTag _pushTag){pushTag = _pushTag;}

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
                SetPushState(PushTag.Start);
                break;
            case "Damage":
                DamageOrGuardCheck(other);
                break;
            default:
                fallDistanceCheck.CollisionEnter();
                if (!fallDistanceCheck.FallDamage) { return; }
                damageTag = DamageTag.Fall;
                break;
        }
    }

    private void DamageOrGuardCheck(Collider other)
    {
        switch (guardState)
        {
            case GuardState.Null:
                //ダメージ発生時の処理
                damageTag = DamageTag.NormalAttack;
                damage.Attacker = other.gameObject;
                break;
            case GuardState.Normal:
            case GuardState.Crouch:
                break;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (death) { return; }
        switch (collision.collider.tag)
        {
            case "Furniture":
                PushTag tag = pushTag;
                if (keyInput.Vertical != 0 || keyInput.Horizontal != 0)
                {
                    switch (pushTag)
                    {
                        case PushTag.Start:
                            tag = PushTag.Pushing;
                            break;
                        case PushTag.Null:
                            tag = PushTag.Start;
                            break;
                    }
                }
                else
                {
                    tag = PushTag.Null;
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
                SetPushState(PushTag.Null);
                break;
            default:
                fallDistanceCheck.CollisionExit();
                break;
        }
    }

}
