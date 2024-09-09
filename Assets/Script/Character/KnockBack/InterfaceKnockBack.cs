using UnityEngine;

public interface InterfaceKnockBack
{
    public void Knockback(Vector3 origin, Vector3 attacker, float power, Rigidbody rb);
}
