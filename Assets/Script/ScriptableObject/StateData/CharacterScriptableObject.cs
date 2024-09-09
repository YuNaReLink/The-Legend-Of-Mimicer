using UnityEngine;


public class CharacterScriptableObject : ScriptableObject
{
    /// <summary>
    /// ƒ[ƒJƒ‹•Ï”
    /// </summary>
    [Header("Å‘åHP"), SerializeField]
    protected float maxHP;
    [Header("ˆÚ“®‰Á‘¬—Í"), SerializeField]
    protected float acceleration;
    [Header("ˆÚ“®Å‘å‘¬“x"), SerializeField]
    protected float maxSpeed;

    public float MaxHP { get { return maxHP; } }
    public float Acceleration { get { return acceleration; } }
    public float MaxSpeed { get { return maxSpeed; } }
}
