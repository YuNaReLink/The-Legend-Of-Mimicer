using UnityEngine;

[CreateAssetMenu]
public class WeaponStateData : ScriptableObject
{
    [SerializeField]
    private float baseDamagePower;
    [SerializeField]
    private float knockBackPower;

    public float BaseDamagePower { get { return baseDamagePower; } }
    public float KnockBackPower { get {return knockBackPower; } }
}
