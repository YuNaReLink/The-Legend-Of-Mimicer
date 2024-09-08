using UnityEngine;

public class KnockBackCommand : InterfaceBaseCommand
{
    private CharacterController controller = null;
    public KnockBackCommand(CharacterController _controller)
    {
        controller = _controller;
    }
    private bool knockBackFlag = false;
    public bool KnockBackFlag { get {  return knockBackFlag; } set { knockBackFlag = value; } }

    private GameObject attacker = null;
    public GameObject Attacker { get { return attacker; } set { attacker = value; } }

    public void Execute()
    {
        if (!knockBackFlag) { return; }
        if (attacker == null) { return; }
        BaseAttackController tool = attacker.GetComponent<BaseAttackController>();
        if (tool == null) { return; }
        WeaponStateData data = tool.GetStatusData();
        if (data == null) { return; }
        Knockback(controller.transform.position, attacker.transform.position,
            data.KnockBackPower, controller.CharacterRB);
        knockBackFlag = false;
    }

    public void Knockback(Vector3 origin, Vector3 attacker, float power, Rigidbody rb)
    {
        Vector3 direction = origin - attacker;
        rb.AddForce(direction * power, ForceMode.VelocityChange);
    }
}
