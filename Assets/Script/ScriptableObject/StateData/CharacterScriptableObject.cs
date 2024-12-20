using UnityEngine;


public class CharacterScriptableObject : ScriptableObject
{
    /// <summary>
    /// ローカル変数
    /// </summary>
    [Header("最大HP"), SerializeField]
    protected float maxHP;
    [Header("移動加速力"), SerializeField]
    protected float acceleration;
    [Header("移動最大速度"), SerializeField]
    protected float maxSpeed;

    public float MaxHP { get { return maxHP; } }
    public float Acceleration { get { return acceleration; } }
    public float MaxSpeed { get { return maxSpeed; } }
}
