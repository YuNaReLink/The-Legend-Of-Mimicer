using CharacterTag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : ToolController
{
    [SerializeField]
    private SwordEffectController effect = null;

    private void Start()
    {
        collider = GetComponent<Collider>();
        effect = GetComponent<SwordEffectController>();
        effect.SetController(controller);
        effect.StopEffect();
    }

    void Update()
    {
        effect.UpdateEffect();
        SetTrigger();
    }

    private bool CheckAttackState()
    {
        StateTag state = controller.CurrentState;
        switch (state)
        {
            case StateTag.Attack:
            case StateTag.JumpAttack:
            case StateTag.SpinAttack:
                return true;
        }
        return false;
    }

    private void SetTrigger()
    {
        if (controller == null) { return; }
        if (collider == null) { return; }
        if (CheckAttackState())
        {
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }
    }

}
