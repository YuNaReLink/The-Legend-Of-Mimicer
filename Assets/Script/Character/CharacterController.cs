using CharacterTag;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    /// <summary>
    /// 状態管理変数
    /// </summary>
    [Header("キャラクターの状態")]
    //状態のインスタンス
    [SerializeField]
    protected StateTag currentState = StateTag.Null;
    public StateTag CurrentState { get { return currentState; } set { currentState = value; } }
    [SerializeField]
    protected StateTag pastState = StateTag.Null;
    public StateTag PastState { get { return pastState; } set { pastState = value; } }

    /// <summary>
    /// アニメーターインスタンス
    /// </summary>
    [SerializeField]
    protected Animator animator;


    public Animator GetAnimator() { return animator; }

    protected AnimatorOverrideController animatorOverride;
    public AnimatorOverrideController GetAnimatorOverrideController() { return animatorOverride; }

    /// <summary>
    /// HP関係の変数
    /// </summary>
    //最大HP
    [SerializeField]
    protected float maxHp = 0;
    public float GetMaxHP() {  return maxHp; }
    //増減するHP
    [SerializeField]
    protected float hp = 0;
    public float HP { get { return hp; }set { hp = value; } }
    [SerializeField]
    protected bool death = false;
    public bool DeathFlag {  get { return death; } set { death = value; } }

    /// <summary>
    /// 移動関連の変数
    /// </summary>
    [SerializeField]
    protected Vector3 velocity = Vector3.zero;
    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }

    /// <summary>
    /// プレイヤーの盾の行動を処理するクラス
    /// </summary>
    [SerializeField]
    public GuardState guardState = GuardState.Null;
    public GuardState GuardState { get { return guardState; } set { guardState = value; } }

    ///<summary>
    ///Collider
    ///</summary>
    [SerializeField]
    protected Collider characterCollider;
    public Collider GetCharacterCollider() { return characterCollider; }
    [SerializeField]
    protected Rigidbody characterRB = new Rigidbody();
    public Rigidbody CharacterRB { get { return characterRB; } set { characterRB = value; } }
    
    /// <summary>
    /// 動いているかいないかを判定するものの集まり
    /// </summary>
    [SerializeField]
    protected bool input = false;


    public bool MoveInput { get { return input; } set { input = value; } }

    /// <summary>
    /// 着地判定
    /// </summary>
    [SerializeField]
    protected GroundCheck groundCheck = null;
    [SerializeField]
    protected bool landing = false;

    public bool Landing { get { return landing; } set { landing = value; } }

    /// <summary>
    /// ジャンプフラグ
    /// </summary>
    [SerializeField]
    protected bool jumping = false;

    public bool Jumping { get { return jumping; } set { jumping = value; } }

    [SerializeField]
    protected float jumpPower = 0;

    /// <summary>
    /// キャラクターの動きを停止させるフラグ
    /// </summary>
    [SerializeField]
    protected bool stopController = false;
    public bool StopController { get { return stopController; } set { stopController = value; } }

    [SerializeField]
    protected VFXScriptableObject vfxObjects = null;

    protected virtual void Awake()
    {
        
    }

    protected virtual void  Start()
    {
        velocity = Vector3.zero;
        input = false;
    }

    protected virtual void InitializeAssign()
    {
        animator = GetComponent<Animator>();

        characterCollider = GetComponent<Collider>();
        characterRB = GetComponent<Rigidbody>();
        groundCheck = GetComponent<GroundCheck>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //入力を解除
        input = false;
    }

    protected void Move()
    {
        characterRB.velocity = new Vector3(velocity.x, characterRB.velocity.y, velocity.z);
    }

    public virtual void AcceleJumpForce(float _jumppower,float _maxJumpPower)
    {
        if(jumpPower >= _maxJumpPower) { return; }
        characterRB.AddForce(transform.up * _jumppower, ForceMode.Acceleration);
        jumpPower += _jumppower;
    }

    public virtual void JumpForce(float _jumppower)
    {
        characterRB.AddForce(transform.up * _jumppower, ForceMode.Impulse);
    }

    public virtual void ForwardAccele(float _accele)
    {
        characterRB.AddForce(transform.forward * _accele, ForceMode.Acceleration);
    }


    public void Death()
    {
        Instantiate(vfxObjects.GetVFXArray()[(int)VFXScriptableObject.VFXTag.Die], transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        if(gameObject == PlayerCameraController.LockObject)
        {
            PlayerCameraController.LockObject = null;
        }
    }
}
