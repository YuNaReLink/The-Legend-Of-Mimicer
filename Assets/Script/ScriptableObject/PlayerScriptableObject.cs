using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerScriptableObject : CharacterScriptableObject
{
    /// <summary>
    /// ÉçÅ[ÉJÉãïœêî
    /// </summary>
    [SerializeField]
    private float deceleration;
    [SerializeField]
    private float minSpeed;
    [SerializeField]
    private float rollingUpDynamicAcceleration;
    [SerializeField]
    private float rollingUpStaticAcceleration;
    [SerializeField]
    private float rollingDownAcceleration;
    [SerializeField]
    private float rollingLeftAcceleration;
    [SerializeField]
    private float rollingRightAcceleration;

    /// <summary>
    /// éQè∆ïœêî
    /// </summary>
    public float Deceleration { get { return deceleration; } }
    public float MinSpeed { get { return minSpeed; } }

    public float RollingUPDynamicAcceleration { get { return rollingUpDynamicAcceleration; } }
    public float RollingUPStaticAcceleration { get { return rollingUpStaticAcceleration; } }
    public float RollingDOWNAcceleration { get { return rollingDownAcceleration; } }
    public float RollingLEFTAcceleration { get { return rollingLeftAcceleration; } }
    public float RollingRIGHTAcceleration { get { return rollingRightAcceleration; } }
}
