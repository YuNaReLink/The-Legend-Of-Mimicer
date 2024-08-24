using CharacterTag;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    /// <summary>
    /// ��ԊǗ��ϐ�
    /// </summary>
    [Header("�L�����N�^�[�̏��")]
    //��Ԃ̃C���X�^���X
    [SerializeField]
    protected StateTag currentState = StateTag.Null;
    public StateTag CurrentState { get { return currentState; } set { currentState = value; } }
    [SerializeField]
    protected StateTag pastState = StateTag.Null;
    public StateTag PastState { get { return pastState; } set { pastState = value; } }

    /// <summary>
    /// �A�j���[�^�[�C���X�^���X
    /// </summary>
    [SerializeField]
    protected Animator animator;
    public Animator GetAnimator() { return animator; }

    protected AnimatorOverrideController animatorOverride;
    public AnimatorOverrideController GetAnimatorOverrideController() { return animatorOverride; }

    protected MotionController motion = null;
    public MotionController GetMotion() {  return motion; }

    protected virtual void SetMotionController(){motion = new MotionController();}

    /// <summary>
    /// HP�֌W�̕ϐ�
    /// </summary>
    //�ő�HP
    [SerializeField]
    protected float maxHp = 0;
    public float GetMaxHP() {  return maxHp; }
    //��������HP
    [SerializeField]
    protected float hp = 0;
    public float HP { get { return hp; }set { hp = value; } }
    [SerializeField]
    protected bool death = false;
    public bool DeathFlag {  get { return death; } set { death = value; } }

    /// <summary>
    /// �ړ��֘A�̕ϐ�
    /// </summary>
    [SerializeField]
    protected Vector3 velocity = Vector3.zero;
    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }

    /// <summary>
    /// �L�����N�^�[�̖h��̏�Ԃ����߂�eunm�N���X
    /// </summary>
    [SerializeField]
    protected GuardState guardState = GuardState.Null;
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
    /// �����Ă��邩���Ȃ����𔻒肷����̂̏W�܂�
    /// </summary>
    [SerializeField]
    protected bool input = false;
    public bool MoveInput { get { return input; } set { input = value; } }

    //�v���C���[�̌��݂̈ʒu
    protected Vector3 currentPos;
    public Vector3 CurrentPos { get { return currentPos; }set { currentPos = value; } }
    //�v���C���[�̉ߋ��̈ʒu
    protected Vector3 pastPos;
    public Vector3 PastPos { get { return pastPos; }set { pastPos = value; } }

    /// <summary>
    /// ���n����
    /// </summary>
    [SerializeField]
    protected GroundCheck groundCheck = null;
    [SerializeField]
    protected bool landing = false;

    public bool Landing { get { return landing; } set { landing = value; } }

    /// <summary>
    /// ���n���Ă�Ԏ擾������W
    /// </summary>
    protected Vector3 landingPosition = Vector3.zero;

    public Vector3 GetLandingPosition() {return landingPosition; }

    /// <summary>
    /// �W�����v�t���O
    /// </summary>
    [SerializeField]
    protected bool jumping = false;

    public bool Jumping { get { return jumping; } set { jumping = value; } }

    [SerializeField]
    protected float jumpPower = 0;

    /// <summary>
    /// �L�����N�^�[�̓������~������t���O
    /// </summary>
    [SerializeField]
    protected bool stopController = false;
    public bool StopController { get { return stopController; } set { stopController = value; } }

    [SerializeField]
    protected VFXController vfxController = null;
    public VFXController GetVFXController() { return vfxController; }

    protected RendererData rendererData = null;
    public RendererData GetRendererData() { return rendererData; }

    protected virtual void Awake(){}

    protected virtual void  Start()
    {
        velocity = Vector3.zero;
        input = false;
    }

    protected virtual void InitializeAssign()
    {
        animator = GetComponent<Animator>();
        SetMotionController();
        characterCollider = GetComponent<Collider>();
        characterRB = GetComponent<Rigidbody>();
        groundCheck = GetComponent<GroundCheck>();
        vfxController = GetComponent<VFXController>();
        rendererData = GetComponentInChildren<RendererData>();
        if (rendererData != null)
        {
            rendererData.AwakeInitilaize();
        }
    }

    protected virtual void Update()
    {
        if (Time.timeScale <= 0) { return; }
        //���͂�����
        input = false;
        currentPos = transform.position;
    }

    protected void LateUpdate()
    {
        pastPos = currentPos;
    }

    protected virtual void UpdateMoveInput()
    {
        input = true;
    }

    public void Decele()
    {
        velocity = StopMoveVelocity();
        characterRB.velocity = StopRigidBodyVelocity();
    }

    protected void Move()
    {
        characterRB.velocity = new Vector3(velocity.x, characterRB.velocity.y, velocity.z);
    }
    public Vector3 StopMoveVelocity()
    {
        return new Vector3(0, characterRB.velocity.y, 0);
    }
    public Vector3 StopRigidBodyVelocity()
    {
        return new Vector3(0, characterRB.velocity.y, 0);
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

    public void Knockback(Vector3 attacker, float power)
    {
        Vector3 direction = transform.position - attacker;
        characterRB.AddForce(direction * power, ForceMode.VelocityChange);
    }

    public virtual void Death()
    {
        if (death) { return; }
        motion.ChangeMotion(StateTag.Die);
        death = true;
    }
}
