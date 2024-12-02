using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�A�G�̐���N���X�̌��N���X
/// </summary>
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    protected CharacterStatus               characterStatus;
    public CharacterStatus                  CharacterStatus => characterStatus;
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
    ///<summary>
    ///Collider
    ///</summary>
    protected Collider                      characterCollider;
    public Collider                         GetCharacterCollider() { return characterCollider; }
    protected Rigidbody                     characterRB;
    public Rigidbody                        CharacterRB { get { return characterRB; } set { characterRB = value; } }
    /// <summary>
    /// ���n����
    /// </summary>
    [SerializeField]
    protected GroundCheck                   groundCheck;
    /// <summary>
    /// �G�t�F�N�g���Ǘ�����N���X
    /// </summary>
    protected EffectController              effectController = null;
    public EffectController                 GetEffectController() { return effectController; }
    /// <summary>
    /// �I�u�W�F�N�g��MeshRenderer���擾�A�ێ�����N���X
    /// </summary>
    protected RendererData                  rendererData = null;
    public RendererData                     GetRendererData() { return rendererData; }
    /// <summary>
    /// �I�u�W�F�N�g�̃J���[��ύX����N���X
    /// </summary>
    protected RendererEffect                rendererEffect = null;
    public RendererEffect                   GetRendererEffect() { return rendererEffect; }
    /// <summary>
    /// �m�b�N�o�b�N�̏������s���N���X
    /// </summary>
    protected KnockBackCommand              knockBackCommand = null;
    public KnockBackCommand                 GetKnockBackCommand() { return knockBackCommand; }
    /// <summary>
    /// �L�����N�^�[���퓬��Ԃ���������Ȃ���
    /// </summary>
    [SerializeField]
    private bool                            battleMode = false;
    public bool                             BattleMode { get { return battleMode; } set { battleMode = value; } }
    /// <summary>
    /// �I�u�W�F�N�g�̕\���A��\����ݒ肷��֐�3�I
    /// </summary>
    /// <returns></returns>
    public GameObject                       SelfObject() { return gameObject; }
    public bool                             IsActiveObject() { return gameObject.activeSelf; }
    public void                             SetActiveObject(bool enabled) { gameObject.SetActive(enabled); }

    /// <summary>
    /// �p�V�t�B�b�N�}�e���A��
    /// </summary>
    [SerializeField]
    protected List<PhysicMaterial> physicMaterials = new List<PhysicMaterial>();
    protected virtual void Awake(){}
    protected virtual void InitializeAssign()
    {
        animator =          GetComponent<Animator>();
        characterCollider = GetComponent<Collider>();
        characterRB =       GetComponent<Rigidbody>();
        effectController =  GetComponent<EffectController>();
        rendererData =      GetComponentInChildren<RendererData>();
        rendererEffect =    new RendererEffect(this);
        knockBackCommand =  new KnockBackCommand(this);

        if(animator == null)
        {
            Debug.LogError("Animator���A�^�b�`����Ă��܂���");
        }

        SetMotionController();

        if(characterCollider == null)
        {
            Debug.Log("Collider���A�^�b�`����Ă��܂���");
        }

        if(characterRB == null)
        {
            Debug.LogError("Rigidbody���A�^�b�`����Ă��܂���");
        }

        groundCheck.SetTransform(transform);

        if(effectController == null)
        {
            Debug.LogError("EffectController");
        }

        if (rendererData != null)
        {
            rendererData.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("Renderer���A�^�b�`����Ă��܂���");
        }
    }

    protected virtual void  Start()
    {
        //�X�^�[�g���̏�����
        characterStatus.Velocity = Vector3.zero;
        characterStatus.MoveInput = false;
        battleMode = false;
    }
    /// <summary>
    /// PhysicMaterial�𒅒n�̗L���ŕύX����֐�
    /// </summary>
    protected void SetPhysicMaterial()
    {
        if (characterStatus.CurrentState != CharacterTagList.StateTag.Grab && !characterStatus.Landing)
        {
            characterCollider.material = physicMaterials[(int)CharacterTagList.PhysicState.Jump];
        }
        else
        {
            characterCollider.material = physicMaterials[(int)CharacterTagList.PhysicState.Land];
        }
    }
    protected virtual void Update()
    {
        //���͂�����
        characterStatus.MoveInput = false;
        rendererEffect.ColorChange();
    }
    /// <summary>
    /// ���͂��Ă邩�𔻒f�����邽�߂̊֐�
    /// </summary>
    protected virtual void MoveStateCheck()
    {
        characterStatus.MoveInput = true;
    }
    /// <summary>
    /// �I�u�W�F�N�g���~�߂�֐�
    /// </summary>
    public virtual void StopMove()
    {
        characterStatus.Velocity = StopMoveVelocity();
        characterRB.velocity = characterStatus.Velocity;
    }
    public Vector3 StopMoveVelocity()
    {
        Vector3 stopVelocity = new Vector3(0, characterRB.velocity.y, 0);
        if (characterStatus.CurrentState == CharacterTagList.StateTag.Grab||
            characterStatus.CurrentState == CharacterTagList.StateTag.ClimbWall)
        {
            stopVelocity = Vector3.zero;
        }

        return stopVelocity;
    }
    /// <summary>
    /// �I�u�W�F�N�g��Rigidbody�Ɉړ��ʂ�^����֐�
    /// </summary>
    protected void Move()
    {
        characterRB.velocity = new Vector3(characterStatus.Velocity.x, characterRB.velocity.y, characterStatus.Velocity.z);
    }
    /// <summary>
    /// �W�����v�̃p���[��������ăI�u�W�F�N�g��y������ɏグ��֐�
    /// </summary>
    /// <param name="_jumppower"></param>
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
        if (characterStatus.DeathFlag) { return; }
        characterStatus.DeathFlag = true;
    }
    /// <summary>
    /// �L�����N�^�[��HP���񕜂�����֐�
    /// maxHP���傫���͂Ȃ�Ȃ�
    /// </summary>
    /// <param name="count"></param>
    public virtual void RecoveryHelth(int count)
    {
        characterStatus.HP +=count;
        if(characterStatus.HP > characterStatus.GetMaxHP())
        {
            characterStatus.HP = characterStatus.GetMaxHP();
        }
    }
}
