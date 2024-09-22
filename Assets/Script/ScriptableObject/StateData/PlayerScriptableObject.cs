using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create PlayerScriptableObject")]
public class PlayerScriptableObject : CharacterScriptableObject
{
    /// <summary>
    /// ���[�J���ϐ�
    /// </summary>
    [Header("������"), SerializeField]
    private float deceleration;
    [Header("��~�Œᑬ�x"), SerializeField]
    private float minSpeed;
    [Header("�ړ����̑O�]������"), SerializeField]
    private float rollingUpDynamicAcceleration;
    [Header("��~���̑O�]������"), SerializeField]
    private float rollingUpStaticAcceleration;
    [Header("�o�N�]������"), SerializeField]
    private float rollingDownAcceleration;
    [Header("�����]������"), SerializeField]
    private float rollingLeftAcceleration;
    [Header("�E���]������"), SerializeField]
    private float rollingRightAcceleration;
    [Header("�o�N�]�W�����v��"), SerializeField]
    private float rollingJumpPower;
    [Header("�ǃW�����v��"), SerializeField]
    private float wallJumpPower;
    [Header("�i���W�����v��"), SerializeField]
    private float lowStepJumpPower;
    [Header("���U��1�̔{��"), SerializeField]
    private float strongAttackPowerRatio1;
    [Header("���U��2�̔{��"), SerializeField]
    private float strongAttackPowerRatio2;
    [Header("�O�i�U���O�i�ڂ̑O�i��"), SerializeField]
    private float thirdAttackForwardPower;
    [Header("�W�����v�U�����̃W�����v��"), SerializeField]
    private float jumpPowerOfJumpAttack;
    [Header("�W�����v�U�����̑O�i��"), SerializeField]
    private float forwardPowerOfJumpAttack;
    [Header("��]�U�����̑O�i��"), SerializeField]
    private float forwardPowerOfSpinAttack;

    /// <summary>
    /// �Q�ƕϐ�
    /// </summary>
    public float Deceleration { get { return deceleration; } }
    public float MinSpeed { get { return minSpeed; } }
    public float RollingUPDynamicAcceleration { get { return rollingUpDynamicAcceleration; } }
    public float RollingUPStaticAcceleration { get { return rollingUpStaticAcceleration; } }
    public float RollingDOWNAcceleration { get { return rollingDownAcceleration; } }
    public float RollingLEFTAcceleration { get { return rollingLeftAcceleration; } }
    public float RollingRIGHTAcceleration { get { return rollingRightAcceleration; } }
    public float RollingJumpPower {  get { return rollingJumpPower; } }
    public float WallJumpPower {  get { return wallJumpPower; } }
    public float LowStepJumpPower {  get { return lowStepJumpPower; } }
    public float StrongAttackPowerRatio1 {  get { return strongAttackPowerRatio1; } }
    public float StrongAttackPowerRatio2 {  get { return strongAttackPowerRatio2; } }
    public float ThirdAttackForwardPower { get { return thirdAttackForwardPower; } }
    public float JumpPowerOfJumpAttack {  get { return jumpPowerOfJumpAttack; } }
    public float ForwardPowerOfJumpAttack { get { return forwardPowerOfJumpAttack; } }
    public float ForwardPowerOfSpinAttack {  get { return forwardPowerOfSpinAttack; } }
}
