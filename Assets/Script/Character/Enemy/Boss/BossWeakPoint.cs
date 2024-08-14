using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakPoint : MonoBehaviour
{
    private BossController controller = null;

    private void Awake()
    {
        controller = GetComponentInParent<BossController>();
        if(controller == null)
        {
            Debug.LogError("controllerがアタッチされていません(BossController)");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Attack") { return; }
        if(controller == null) { return; }
        switch (controller.CurrentState)
        {
            case CharacterTag.StateTag.Attack:
                controller.GetVFXController().CreateVFX(VFXScriptableObject.VFXTag.Damage, other.transform.position, 1f, Quaternion.identity);
                controller.StunFlag = true;
                break;
            case CharacterTag.StateTag.Damage:
                controller.GetVFXController().CreateVFX(VFXScriptableObject.VFXTag.Damage, other.transform.position, 1f, Quaternion.identity);
                controller.GetDamage().Attacker = other.gameObject;
                controller.GetDamage().DamageFlag = true;
                break;
        }
    }
}
