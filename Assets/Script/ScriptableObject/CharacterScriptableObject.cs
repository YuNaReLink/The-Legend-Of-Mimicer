using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterScriptableObject : ScriptableObject
{
    /// <summary>
    /// ƒ[ƒJƒ‹•Ï”
    /// </summary>
    [SerializeField]
    protected float maxHP;
    [SerializeField]
    protected float acceleration;
    [SerializeField]
    protected float maxSpeed;

    public float MaxHP { get { return maxHP; } }
    public float Acceleration { get { return acceleration; } }
    public float MaxSpeed { get { return maxSpeed; } }
}
