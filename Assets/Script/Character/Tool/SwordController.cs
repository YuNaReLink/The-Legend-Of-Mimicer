using UnityEngine;

/// <summary>
/// �����g�ɃA�^�b�`����N���X
/// �G�t�F�N�g�ⓖ���蔻��͌����̂����䂷��l����
/// </summary>
public class SwordController : ToolController
{
    public override float           AttackPower => base.AttackPower * ratioPower;

    private float                   ratioPower = 1f;

    public override void SetController(CharacterController _controller)
    {
        base.SetController(_controller);
        player = controller.GetComponent<PlayerController>();
    }
    private PlayerController        player = null;

    public override ToolTag GetToolTag() { return ToolTag.Sword; }
    private SwordEffectController   effect = null;

    private CharacterTagList.StateTag[] StateArray = new CharacterTagList.StateTag[]
    {
        CharacterTagList.StateTag.Attack,
        CharacterTagList.StateTag.JumpAttack,
        CharacterTagList.StateTag.SpinAttack,
    };

    /// <summary>
    /// �R���C�_�[�̒��S�A�T�C�Y����ʂ��邽�߂�State
    /// </summary>
    private enum ColliderType
    {
        Normal,
        SpinAttack
    }
    private BoxCollider boxCollider = null;
    /// <summary>
    /// ��Ԃɂ���Č��̃R���C�_�[�̒��S�ƃT�C�Y��ύX����l��ێ������z��
    /// </summary>
    private Vector3[] colliderCenter = new Vector3[]
    {
        new Vector3(0,1.1f,0),
        new Vector3(0,1.9f,0)
    };
    private Vector3[] colliderScale = new Vector3[]
    {
        new Vector3(0.6f,1.8f,0.2f),
        new Vector3(1,3.5f,1)
    };
    protected override void Awake()
    {
        base.Awake();
        if (collider != null)
        {
            boxCollider = collider.GetComponent<BoxCollider>();
        }
        else
        {
            Debug.Log("Collider���A�^�b�`����Ă��Ȃ�");
        }
        effect = GetComponent<SwordEffectController>();
    }
    private void Start()
    {
        //�G�t�F�N�g�̏�����
        effect.StopTrail();
    }

    void Update()
    {
        ActiveCheck();
        SetAttackPower();
        SetColliderSize();
    }

    /// <summary>
    /// ���̃R���C�_�[��L���ɂ��邩���߂�֐�
    /// </summary>
    private void ActiveCheck()
    {
        if (controller == null) { return; }
        if (collider == null) { return; }
        CharacterTagList.StateTag state = controller.CharacterStatus.CurrentState;
        if (MotionTimeCheck(state))
        {
            effect.PlayTrail();
            collider.enabled = true;
        }
        else
        {
            effect.StopTrail();
            collider.enabled = false;
        }
    }

    private const float JumpattackStartCount = 0.3f;
    private const float JumpattackEndCount = 0.5f;

    private const float SpinAttackStartCount = 0.3f;
    /// <summary>
    /// ���[�V�����̖��O���猻�݂̃��[�V�������擾����
    /// ���[�V�����̍Đ����ԂŃR���C�_�[��L���ɂ��邩�����߂�
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    private bool MotionTimeCheck(CharacterTagList.StateTag tag)
    {
        AnimatorStateInfo animInfo = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
        PlayerController player = controller.GetComponent<PlayerController>();
        switch (tag)
        {
            case CharacterTagList.StateTag.Attack:
                if (TripleAttackCheck(animInfo))
                {
                    return true;
                }
                break;
            case CharacterTagList.StateTag.JumpAttack:
                if (animInfo.normalizedTime >= JumpattackStartCount &&
                    animInfo.normalizedTime < JumpattackEndCount)
                {
                    return true;
                }
                break;
            case CharacterTagList.StateTag.SpinAttack:
                if (animInfo.normalizedTime < SpinAttackStartCount)
                {
                    return true;
                }
                break;
            default:
                return false;
        }

        return false;
    }

    private const float FirstAttackStartTime = 0.3f;
    private const float FirstAttackEndTime = 0.7f;

    private const float SecondAttackStartTime = 0.1f;
    private const float SecondAttackEndTime = 0.4f;

    private const float ThreeAttackStartTime = 0.5f;
    private const float ThreeAttackEndTime = 0.7f;
    /// <summary>
    /// �O�i�U�����̃R���C�_�[��L���ɂ���Ԋu�����߂�֐�
    /// </summary>
    /// <param name="animInfo">
    /// �L�����N�^�[�̃A�j���[�^�[�����������
    /// </param>
    /// <returns></returns>
    private bool TripleAttackCheck(AnimatorStateInfo animInfo)
    {
        PlayerController player = controller.GetComponent<PlayerController>();
        switch (player.TripleAttack)
        {
            case CharacterTagList.TripleAttack.First:
                if (animInfo.normalizedTime >= FirstAttackStartTime &&
                    animInfo.normalizedTime < FirstAttackEndTime)
                {
                    return true;
                }
                break;
            case CharacterTagList.TripleAttack.Second:
                if (animInfo.normalizedTime >= SecondAttackStartTime &&
                    animInfo.normalizedTime < SecondAttackEndTime)
                {
                    return true;
                }
                break;
            case CharacterTagList.TripleAttack.Third:
                if (animInfo.normalizedTime >= ThreeAttackStartTime &&
                    animInfo.normalizedTime < ThreeAttackEndTime)
                {
                    return true;
                }
                break;
        }
        return false;
    }
    /// <summary>
    /// ��������U����Ԃ���_���[�W�ʂ����߂�֐�
    /// </summary>
    private void SetAttackPower()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
                ratioPower = statusData.BaseDamagePower;
                break;
            case CharacterTagList.StateTag.JumpAttack:
                ratioPower = player.GetData().StrongAttackPowerRatio1;
                break;
            case CharacterTagList.StateTag.SpinAttack:
                ratioPower = player.GetData().StrongAttackPowerRatio2;
                break;
        }
    }
    /// <summary>
    /// ��Ԃɂ���Č��̃R���C�_�[�̃T�C�Y��ύX����֐�
    /// </summary>
    private void SetColliderSize()
    {
        if (controller == null) { return; }
        if (collider == null) { return; }
        if(boxCollider == null) { return; }
        if(controller.CharacterStatus.CurrentState == controller.CharacterStatus.PastState) { return; }
        CharacterTagList.StateTag state = controller.CharacterStatus.CurrentState;
        Vector3 center = boxCollider.center;
        Vector3 size = boxCollider.size;
        switch (state)
        {
            case CharacterTagList.StateTag.SpinAttack:
                center = colliderCenter[(int)ColliderType.SpinAttack];
                size  = colliderScale[(int)ColliderType.SpinAttack];
                break;
            default:
                center = colliderCenter[(int)ColliderType.Normal];
                size  = colliderScale[(int)ColliderType.Normal];
                break;
        }
        boxCollider.center = center;
        boxCollider.size = size;
    }

}
