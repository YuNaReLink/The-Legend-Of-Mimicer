using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create PlayerScriptableObject")]
public class PlayerScriptableObject : CharacterScriptableObject
{
    /// <summary>
    /// ƒ[ƒJƒ‹•Ï”
    /// </summary>
    [Header("Œ¸‘¬—Í"), SerializeField]
    private float deceleration;
    [Header("’â~Å’á‘¬“x"), SerializeField]
    private float minSpeed;
    [Header("ˆÚ“®’†‚Ì‘O“]‰Á‘¬—Í"), SerializeField]
    private float rollingUpDynamicAcceleration;
    [Header("’â~’†‚Ì‘O“]‰Á‘¬—Í"), SerializeField]
    private float rollingUpStaticAcceleration;
    [Header("ƒoƒN“]‰Á‘¬—Í"), SerializeField]
    private float rollingDownAcceleration;
    [Header("¶‘¤“]‰Á‘¬—Í"), SerializeField]
    private float rollingLeftAcceleration;
    [Header("‰E‘¤“]‰Á‘¬—Í"), SerializeField]
    private float rollingRightAcceleration;
    [Header("ƒoƒN“]ƒWƒƒƒ“ƒv—Í"), SerializeField]
    private float rollingJumpPower;
    [Header("•ÇƒWƒƒƒ“ƒv—Í"), SerializeField]
    private float wallJumpPower;
    [Header("’i·ƒWƒƒƒ“ƒv—Í"), SerializeField]
    private float lowStepJumpPower;
    [Header("‹­UŒ‚1‚Ì”{—¦"), SerializeField]
    private float strongAttackPowerRatio1;
    [Header("‹­UŒ‚2‚Ì”{—¦"), SerializeField]
    private float strongAttackPowerRatio2;

    /// <summary>
    /// QÆ•Ï”
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
}
