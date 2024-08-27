using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create WeaponStateData")]
public class WeaponStateData : ScriptableObject
{
    [SerializeField]
    private float baseDamagePower;
    [SerializeField]
    private float knockBackPower;

    public float BaseDamagePower { get { return baseDamagePower; } }
    public float KnockBackPower { get {return knockBackPower; } }
}
