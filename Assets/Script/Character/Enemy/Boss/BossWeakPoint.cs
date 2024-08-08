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
                controller.StunFlag = true;
                break;
        }
    }
}
