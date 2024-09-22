using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create PlayerScriptableObject")]
public class PlayerScriptableObject : CharacterScriptableObject
{
    /// <summary>
    /// ローカル変数
    /// </summary>
    [Header("減速力"), SerializeField]
    private float deceleration;
    [Header("停止最低速度"), SerializeField]
    private float minSpeed;
    [Header("移動中の前転加速力"), SerializeField]
    private float rollingUpDynamicAcceleration;
    [Header("停止中の前転加速力"), SerializeField]
    private float rollingUpStaticAcceleration;
    [Header("バク転加速力"), SerializeField]
    private float rollingDownAcceleration;
    [Header("左側転加速力"), SerializeField]
    private float rollingLeftAcceleration;
    [Header("右側転加速力"), SerializeField]
    private float rollingRightAcceleration;
    [Header("バク転ジャンプ力"), SerializeField]
    private float rollingJumpPower;
    [Header("壁ジャンプ力"), SerializeField]
    private float wallJumpPower;
    [Header("段差ジャンプ力"), SerializeField]
    private float lowStepJumpPower;
    [Header("強攻撃1の倍率"), SerializeField]
    private float strongAttackPowerRatio1;
    [Header("強攻撃2の倍率"), SerializeField]
    private float strongAttackPowerRatio2;
    [Header("三段攻撃三段目の前進力"), SerializeField]
    private float thirdAttackForwardPower;
    [Header("ジャンプ攻撃時のジャンプ力"), SerializeField]
    private float jumpPowerOfJumpAttack;
    [Header("ジャンプ攻撃時の前進力"), SerializeField]
    private float forwardPowerOfJumpAttack;
    [Header("回転攻撃時の前進力"), SerializeField]
    private float forwardPowerOfSpinAttack;

    /// <summary>
    /// 参照変数
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
