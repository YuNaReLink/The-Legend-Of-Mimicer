using UnityEngine;

[System.Serializable]
public class CharacterStatus
{
    /// <summary>
    /// ��ԊǗ��ϐ�
    /// </summary>
    [Header("�L�����N�^�[�̏��")]
    //��Ԃ̃C���X�^���X
    [SerializeField]
    protected CharacterTagList.StateTag     currentState = CharacterTagList.StateTag.Null;
    public CharacterTagList.StateTag        CurrentState { get { return currentState; } set { currentState = value; } }
    [SerializeField]
    protected CharacterTagList.StateTag     pastState = CharacterTagList.StateTag.Null;
    public CharacterTagList.StateTag        PastState { get { return pastState; } set { pastState = value; } }

    /// <summary>
    /// HP�֌W�̕ϐ�
    /// </summary>
    //�ő�HP
    [SerializeField]
    protected float                         maxHp = 0;
    public float                            GetMaxHP() { return maxHp; }
    public void                             SetMaxHP(float _maxHp) { maxHp = _maxHp; }
    //��������HP
    [SerializeField]
    protected float                         hp = 0;
    public float                            HP { get { return hp; } set { hp = value; } }
    [SerializeField]
    protected bool                          death = false;
    public bool                             DeathFlag { get { return death; } set { death = value; } }

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
    protected CharacterTagList.GuardState   guardState = CharacterTagList.GuardState.Null;
    public CharacterTagList.GuardState      GuardState { get { return guardState; } set { guardState = value; } }

    /// <summary>
    /// �����Ă��邩���Ȃ����𔻒肷��t���O
    /// </summary>
    [SerializeField]
    protected bool                          input = false;
    public bool                             MoveInput { get { return input; } set { input = value; } }

    [SerializeField]
    protected bool                          landing = false;
    public bool                             Landing { get { return landing; } set { landing = value; } }
    /// <summary>
    /// ���n���Ă�Ԏ擾������W
    /// </summary>
    protected Vector3                       landingPosition = Vector3.zero;

    public Vector3                          GetLandingPosition() { return landingPosition; }
    public void                             SetLandingPosition(Vector3 _landingPosition) {  landingPosition = _landingPosition;}


    /// <summary>
    /// �W�����v�t���O
    /// </summary>
    [SerializeField]
    protected bool                          jumping = false;

    public bool                             Jumping { get { return jumping; } set { jumping = value; } }

    [SerializeField]
    protected float                         jumpPower = 0;
    public float                            JumpPower => jumpPower;
    public void                             SetJumpPower(float _jumpPower) { jumpPower = _jumpPower;}

    /// <summary>
    /// �L�����N�^�[�̓������~������t���O
    /// </summary>
    [SerializeField]
    protected bool                          stopController = false;
    public bool                             StopController { get { return stopController; } set { stopController = value; } }
}
