using UnityEngine;

/// <summary>
/// プレイヤーの全ての処理を行っているクラス
/// </summary>
public class PlayerController : CharacterController
{
    //外部からアタッチする一覧
    /// <summary>
    /// ScriptableObjectデータ
    /// プレイヤーで使う変数の値を保持してる
    /// </summary>
    [SerializeField]
    private PlayerScriptableObject          data;
    public PlayerScriptableObject           GetData() { return data; }
    /// <summary>
    /// プレイヤーの道具の処理をまとめたクラス
    /// </summary>
    private ToolInventoryController         toolInventory = null;
    public ToolInventoryController          GetToolController() { return toolInventory; }
    //SerializableでController内にあるクラス
    /// <summary>
    /// プレイヤーのキー入力をまとめたクラス
    /// </summary>
    [SerializeField]
    private PlayerInput                     keyInput;
    public PlayerInput                      GetKeyInput() { return keyInput; }
    /// <summary>
    /// プレイヤーの壁、崖との判定処理をまとめたクラス
    /// </summary>
    [SerializeField]
    private ObstacleAction                   obstacleAction = null;
    public ObstacleAction                    GetObstacleCheck() { return obstacleAction; }
    /// <summary>
    /// プレイヤーの道具とは違う装飾品の設定を行うクラス
    /// </summary>
    [SerializeField]
    private PlayerDecorationController      decorationController = null;
    //生成するクラスの一覧
    /// <summary>
    /// プレイヤー状態を管理するクラス
    /// </summary>
    private PlayerState                     state = null;
    /// <summary>
    /// プレイヤーの行動を実行するクラス
    /// </summary>
    private PlayerCommands                  commands = null;
    public PlayerCommands                   GetCommands() { return commands; }
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
    /// プレイヤーの各右・左の道具の処理を生成するクラス
    /// </summary>
    private InterfaceBaseToolCommand        rightAction = null;
    public InterfaceBaseToolCommand         RightAction { get { return rightAction; } set { rightAction = value; } }
    private InterfaceBaseToolCommand        leftAction = null;
    public InterfaceBaseToolCommand         LeftAction { get { return leftAction; } set { leftAction = value; } }

    //プレイヤーで使うタグ
    /// <summary>
    /// プレイヤーの攻撃行動を処理するクラス
    /// </summary>
    [SerializeField]
    private CharacterTagList.TripleAttack   tripleAttack = CharacterTagList.TripleAttack.Null;
    public CharacterTagList.TripleAttack    TripleAttack { get { return tripleAttack; } set {  tripleAttack = value; } }
    /// <summary>
    /// プレイヤーが動かせるオブジェクトに触れてるか判定する
    /// </summary>
    [SerializeField]
    private CharacterTagList.PushTag        pushTag = CharacterTagList.PushTag.Null;
    public CharacterTagList.PushTag         PushTag  => pushTag;
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

        //サウンドコントローラーのAwake時の初期化
        soundController.AwakeInitilaize();
        if (data != null)
        {
            characterStatus.SetMaxHP(data.MaxHP);
            characterStatus.HP = characterStatus.GetMaxHP();
        }
    }
    /// <summary>
    /// Awakeで行うプレイヤーの初期化
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
            Debug.LogError("ToolInventoryControllerがアタッチされていません");
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
    //入力処理を行う
    protected override void Update()
    {
        keyInput.SystemInput();
        if (Time.timeScale <= 0) { return; }
        base.Update();
        decorationController.Update();
        //タイマーの更新
        timer.TimerUpdate();
        //着地時の判定
        LandingCheck();
        //プレイヤーの体を表示、非表示にする処理
        BodyRendererUpdate();
        //外部からプレイヤーを止める処理が呼び出されたらリターン
        if (characterStatus.StopController) { return; }
        //障害物との当たり判定
        obstacleAction.WallCheckInput();
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
    /// <summary>
    /// 着地、空中で行う処理を行う
    /// </summary>
    private void LandingCheck()
    {
        characterStatus.Landing = groundCheck.CheckGroundStatus();
        //パシフィックマテリアル変更
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
    /// プレイヤーオブジェクトのMeshRendererの表示を変更する関数
    /// </summary>
    private void BodyRendererUpdate()
    {
        foreach (var renderer in rendererData.RendererList)
        {
            if (renderer.enabled == !CameraController.Instance.IsFPSMode()) { continue; }
            renderer.enabled = !CameraController.Instance.IsFPSMode();
        }
    }
    //行動処理を行う
    private void FixedUpdate()
    {
        if (characterStatus.StopController) { return; }
        MoveStateCheck();
        UpdateCommand();
    }
    /// <summary>
    /// プレイヤーが動くか動かないかを状態で決める関数
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
    /// カメラの方向から上下左右を取得する関数
    /// </summary>
    /// <param name="dir">
    /// カメラの前、横のVector3
    /// </param>
    /// <returns></returns>
    public Vector3 GetCameraDirection(Vector3 dir)
    {
        return Vector3.Scale(dir, new Vector3(1, 0, 1)).normalized;
    }
    /// <summary>
    /// プレイヤーの行動を実行する処理をする関数
    /// プレイヤーの移動や停止、回転を行う
    /// </summary>
    private void UpdateCommand()
    {
        //MonoBehaviourを継承してるクラスの処理
        obstacleAction?.Execute();
        //同じinterface(InterfaceBaseToolCommand)から宣言してるクラスの処理
        rightAction?.Execute();
        leftAction?.Execute();
        //いくつかあるInterfaceBaseCommandを実装したクラスをまとめて処理してるクラスの処理
        commands.DoUpdate();
        //継承元のCharacterで宣言してるクラスの処理
        knockBackCommand?.Execute();
        //移動処理
        Accele(GetCameraDirection(Camera.main.transform.forward), GetCameraDirection(Camera.main.transform.right),
                                  data.MaxSpeed, data.Acceleration);
        //入力がなかった場合停止処理
        StopMove();
        //移動RigidBodyに適用
        Move();
        if(RotateStopFlag()) { return; }
        //プレイヤー自身の回転処理
        transform.rotation = rotation.SelfRotation(this);
    }
    /// <summary>
    /// プレイヤーが回転するかを状態で決める関数
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
    /// 移動処理を行う関数
    /// </summary>
    /// <param name="forward">
    /// カメラから見て前の数値
    /// </param>
    /// <param name="right">
    /// カメラから見て右の数値
    /// </param>
    /// <param name="_maxspeed">
    /// 最大スピード
    /// </param>
    /// <param name="_accele">
    /// 加速力
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
        // 現在の速度の大きさを計算
        float currentSpeed = vel.magnitude;
        // もし現在の速度が最大速度未満ならば、加速度を適用する
        // 現在の速度が最大速度以上の場合は速度を最大速度に制限する
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
    /// 移動で使うスピードを状態によって変更する関数
    /// </summary>
    /// <param name="_speed">
    /// 最大、加速スピードを代入
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
    /// プレイヤーの死亡処理関数
    /// </summary>
    public override void Death()
    {
        base.Death();
        motion.ForcedChangeMotion(CharacterTagList.StateTag.Die);
        effectController.CreateVFX((int)EffectTagList.CharacterEffectTag.Death, transform.position, 1f, Quaternion.identity);
    }
    /// <summary>
    /// ライフ回復処理関数
    /// </summary>
    /// <param name="count"></param>
    public override void RecoveryHelth(int count)
    {
        base.RecoveryHelth(count);
        soundController.PlaySESound((int)SoundTagList.PlayerSoundTag.GetHeart);
    }
    /// <summary>
    /// 矢を取得した時に処理するSE処理
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
    /// コライダーに他のコライダーが当たった時の処理
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
    /// ダメージを受けるか判定する関数
    /// </summary>
    /// <param name="other"></param>
    private void DamageOrGuardCheck(Collider other)
    {
        switch (characterStatus.GuardState)
        {
            case CharacterTagList.GuardState.Null:
                //ダメージ発生時の処理
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
    /// 押す状態タグの変更を行う関数
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
