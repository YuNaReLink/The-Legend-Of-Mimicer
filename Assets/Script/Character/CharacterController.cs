using CharacterTag;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    /// <summary>
    /// ��ԊǗ��ϐ�
    /// </summary>
    [Header("�L�����N�^�[�̏��")]
    //��Ԃ̃C���X�^���X
    [SerializeField]
    protected StateTag                      currentState = StateTag.Null;
    public StateTag                         CurrentState { get { return currentState; } set { currentState = value; } }
    [SerializeField]
    protected StateTag                      pastState = StateTag.Null;
    public StateTag                         PastState { get { return pastState; } set { pastState = value; } }

    /// <summary>
    /// �A�j���[�^�[�C���X�^���X
    /// </summary>
    protected Animator                      animator = null;
    public Animator                         GetAnimator() { return animator; }

    protected AnimatorOverrideController    animatorOverride;
    public AnimatorOverrideController       GetAnimatorOverrideController() { return animatorOverride; }

    protected MotionController              motion = null;
    public MotionController                 GetMotion() {  return motion; }

    protected virtual void                  SetMotionController(){ motion = new MotionController();}

    /// <summary>
    /// HP�֌W�̕ϐ�
    /// </summary>
    //�ő�HP
    [SerializeField]
    protected float                         maxHp = 0;
    public float                            GetMaxHP() {  return maxHp; }
    //��������HP
    [SerializeField]
    protected float                         hp = 0;
    public float                            HP { get { return hp; }set { hp = value; } }
    [SerializeField]
    protected bool                          death = false;
    public bool                             DeathFlag {  get { return death; } set { death = value; } }

    /// <summary>
    /// �ړ��֘A�̕ϐ�
    /// </summary>
    [SerializeField]
    protected Vector3                       velocity = Vector3.zero;
    public Vector3                          Velocity { get { return velocity; } set { velocity = value; } }

    /// <summary>
    /// �L�����N�^�[�̖h��̏�Ԃ����߂�eunm�N���X
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
    /// �����Ă��邩���Ȃ����𔻒肷����̂̏W�܂�
    /// </summary>
    [SerializeField]
    protected bool                          input = false;
    public bool                             MoveInput { get { return input; } set { input = value; } }

    /// <summary>
    /// ���n����
    /// </summary>
    protected GroundCheck                   groundCheck = null;
    [SerializeField]
    protected bool                          landing = false;
    public bool                             Landing { get { return landing; } set { landing = value; } }

    /// <summary>
    /// ���n���Ă�Ԏ擾������W
    /// </summary>
    protected Vector3                       landingPosition = Vector3.zero;

    public Vector3                          GetLandingPosition() {return landingPosition; }

    /// <summary>
    /// �W�����v�t���O
    /// </summary>
    [SerializeField]
    protected bool                          jumping = false;

    public bool                             Jumping { get { return jumping; } set { jumping = value; } }

    [SerializeField]
    protected float                         jumpPower = 0;

    /// <summary>
    /// �L�����N�^�[�̓������~������t���O
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
            Debug.LogError("Animator���A�^�b�`����Ă��܂���");
        }
        SetMotionController();
        characterCollider = GetComponent<Collider>();
        if(characterCollider == null)
        {
            Debug.Log("Collider���A�^�b�`����Ă��܂���");
        }
        characterRB = GetComponent<Rigidbody>();
        if(characterRB == null)
        {
            Debug.LogError("Rigidbody���A�^�b�`����Ă��܂���");
        }
        groundCheck = GetComponent<GroundCheck>();
        if (groundCheck == null)
        {
            transform.AddComponent<GroundCheck>();
            groundCheck = GetComponent<GroundCheck>();
            Debug.Log("GroundCheck���A�^�b�`����Ă��Ȃ������̂ŃA�^�b�`���܂���");
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
            Debug.LogError("Rrnderer���A�^�b�`����Ă��܂���");
        }
        knockBackCommand = new KnockBackCommand(this);
    }

    protected virtual void  Start()
    {
        //�X�^�[�g���̏�����
        velocity = Vector3.zero;
        input = false;
    }


    protected virtual void Update()
    {
        if (Time.timeScale <= 0) { return; }
        //���͂�����
        input = false;
    }
    /// <summary>
    /// ���͂��Ă邩�𔻒f�����邽�߂̊֐�
    /// </summary>
    protected virtual void UpdateMoveInput()
    {
        input = true;
    }
    /// <summary>
    /// �I�u�W�F�N�g���~�߂�֐�
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
    /// �I�u�W�F�N�g��Rigidbody�Ɉړ��ʂ�^����֐�
    /// </summary>
    protected void Move()
    {
        characterRB.velocity = new Vector3(velocity.x, characterRB.velocity.y, velocity.z);
    }
    /// <summary>
    /// �W�����v�̃p���[��������ăI�u�W�F�N�g��y������ɏグ��֐�
    /// </summary>
    /// <param name="_jumppower"></param>
    /// <param name="_maxJumpPower"></param>
    public virtual void JumpForce(float _jumppower)
    {
        characterRB.AddForce(transform.up * _jumppower, ForceMode.Impulse);
    }
    /// <summary>
    /// �I�u�W�F�N�g�̑O�����Ɉ����ł�����Ă����l����������������֐�
    /// </summary>
    /// <param name="_accele"></param>
    public virtual void ForwardAccele(float _accele)
    {
        characterRB.AddForce(transform.forward * _accele, ForceMode.Acceleration);
    }
    /// <summary>
    /// �L�����N�^�[�����S�������ɌĂяo�����֐�
    /// </summary>
    public virtual void Death()
    {
        if (death) { return; }
        motion.ForcedChangeMotion(StateTag.Die);
        death = true;
    }
    /// <summary>
    /// �L�����N�^�[��HP���񕜂�����֐�
    /// maxHP���傫���͂Ȃ�Ȃ�
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
