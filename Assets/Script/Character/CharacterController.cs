using CharacterTag;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    /// <summary>
    /// 状態管理変数
    /// </summary>
    [Header("キャラクターの状態")]
    //状態のインスタンス
    [SerializeField]
    protected StateTag                      currentState = StateTag.Null;
    public StateTag                         CurrentState { get { return currentState; } set { currentState = value; } }
    [SerializeField]
    protected StateTag                      pastState = StateTag.Null;
    public StateTag                         PastState { get { return pastState; } set { pastState = value; } }

    /// <summary>
    /// アニメーターインスタンス
    /// </summary>
    protected Animator                      animator = null;
    public Animator                         GetAnimator() { return animator; }

    protected AnimatorOverrideController    animatorOverride;
    public AnimatorOverrideController       GetAnimatorOverrideController() { return animatorOverride; }

    protected MotionController              motion = null;
    public MotionController                 GetMotion() {  return motion; }

    protected virtual void                  SetMotionController(){ motion = new MotionController();}

    /// <summary>
    /// HP関係の変数
    /// </summary>
    //最大HP
    [SerializeField]
    protected float                         maxHp = 0;
    public float                            GetMaxHP() {  return maxHp; }
    //増減するHP
    [SerializeField]
    protected float                         hp = 0;
    public float                            HP { get { return hp; }set { hp = value; } }
    [SerializeField]
    protected bool                          death = false;
    public bool                             DeathFlag {  get { return death; } set { death = value; } }

    /// <summary>
    /// 移動関連の変数
    /// </summary>
    [SerializeField]
    protected Vector3                       velocity = Vector3.zero;
    public Vector3                          Velocity { get { return velocity; } set { velocity = value; } }

    /// <summary>
    /// キャラクターの防御の状態を決めるeunmクラス
    /// </summary>
    [SerializeField]
    protected GuardState                    guardState = GuardState.Null;
    public GuardState                       GuardState { get { return guardState; } set { guardState = value; } }

    ///<summary>
    ///Collider
    ///</summary>
    protected Collider                      characterCollider;
    public Collider                         GetCharacterCollider() { return characterCollider; }
    protected Rigidbody                     characterRB;
    public Rigidbody                        CharacterRB { get { return characterRB; } set { characterRB = value; } }
    
    /// <summary>
    /// 動いているかいないかを判定するものの集まり
    /// </summary>
    [SerializeField]
    protected bool                          input = false;
    public bool                             MoveInput { get { return input; } set { input = value; } }

    /// <summary>
    /// 着地判定
    /// </summary>
    protected GroundCheck                   groundCheck = null;
    [SerializeField]
    protected bool                          landing = false;
    public bool                             Landing { get { return landing; } set { landing = value; } }

    /// <summary>
    /// 着地してる間取得する座標
    /// </summary>
    protected Vector3                       landingPosition = Vector3.zero;

    public Vector3                          GetLandingPosition() {return landingPosition; }

    /// <summary>
    /// ジャンプフラグ
    /// </summary>
    [SerializeField]
    protected bool                          jumping = false;

    public bool                             Jumping { get { return jumping; } set { jumping = value; } }

    [SerializeField]
    protected float                         jumpPower = 0;

    /// <summary>
    /// キャラクターの動きを停止させるフラグ
    /// </summary>
    [SerializeField]
    protected bool                          stopController = false;
    public bool                             StopController { get { return stopController; } set { stopController = value; } }

    protected EffectController                 vfxController = null;
    public EffectController                    GetVFXController() { return vfxController; }

    protected RendererData                  rendererData = null;
    public RendererData                     GetRendererData() { return rendererData; }

    protected KnockBackCommand              knockBackCommand = null;
    public KnockBackCommand                 GetKnockBackCommand() { return knockBackCommand; }
    public GameObject SelfObject() { return gameObject; }
    public bool IsActiveObject() { return gameObject.activeSelf; }
    public void SetActiveObject(bool enabled) { gameObject.SetActive(enabled); }
    protected virtual void Awake(){}
    protected virtual void InitializeAssign()
    {
        animator = GetComponent<Animator>();
        if(animator == null)
        {
            Debug.LogError("Animatorがアタッチされていません");
        }
        SetMotionController();
        characterCollider = GetComponent<Collider>();
        if(characterCollider == null)
        {
            Debug.Log("Colliderがアタッチされていません");
        }
        characterRB = GetComponent<Rigidbody>();
        if(characterRB == null)
        {
            Debug.LogError("Rigidbodyがアタッチされていません");
        }
        groundCheck = GetComponent<GroundCheck>();
        if (groundCheck == null)
        {
            transform.AddComponent<GroundCheck>();
            groundCheck = GetComponent<GroundCheck>();
            Debug.Log("GroundCheckがアタッチされていなかったのでアタッチしました");
        }
        vfxController = GetComponent<EffectController>();
        if(vfxController == null)
        {
            Debug.LogError("EffectController");
        }
        rendererData = GetComponentInChildren<RendererData>();
        if (rendererData != null)
        {
            rendererData.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("Rrndererがアタッチされていません");
        }
        knockBackCommand = new KnockBackCommand(this);
    }

    protected virtual void  Start()
    {
        //スタート時の初期化
        velocity = Vector3.zero;
        input = false;
    }


    protected virtual void Update()
    {
        if (Time.timeScale <= 0) { return; }
        //入力を解除
        input = false;
    }
    /// <summary>
    /// 入力してるかを判断させるための関数
    /// </summary>
    protected virtual void UpdateMoveInput()
    {
        input = true;
    }
    /// <summary>
    /// オブジェクトを止める関数
    /// </summary>
    public void StopMove()
    {
        velocity = StopMoveVelocity();
        characterRB.velocity = velocity;
    }
    public Vector3 StopMoveVelocity()
    {
        return new Vector3(0, characterRB.velocity.y, 0);
    }
    /// <summary>
    /// オブジェクトのRigidbodyに移動量を与える関数
    /// </summary>
    protected void Move()
    {
        characterRB.velocity = new Vector3(velocity.x, characterRB.velocity.y, velocity.z);
    }
    /// <summary>
    /// ジャンプのパワーをもらってオブジェクトをy上方向に上げる関数
    /// </summary>
    /// <param name="_jumppower"></param>
    /// <param name="_maxJumpPower"></param>
    public virtual void JumpForce(float _jumppower)
    {
        characterRB.AddForce(transform.up * _jumppower, ForceMode.Impulse);
    }
    /// <summary>
    /// オブジェクトの前方向に引数でもらってきた値分だけ加速させる関数
    /// </summary>
    /// <param name="_accele"></param>
    public virtual void ForwardAccele(float _accele)
    {
        characterRB.AddForce(transform.forward * _accele, ForceMode.Acceleration);
    }
    /// <summary>
    /// キャラクターが死亡した時に呼び出す基底関数
    /// </summary>
    public virtual void Death()
    {
        if (death) { return; }
        motion.ForcedChangeMotion(StateTag.Die);
        death = true;
    }
    /// <summary>
    /// キャラクターのHPを回復させる関数
    /// maxHPより大きくはならない
    /// </summary>
    /// <param name="count"></param>
    public virtual void RecoveryHelth(int count)
    {
        hp+=count;
        if(hp > maxHp)
        {
            hp = maxHp;
        }
    }
}
