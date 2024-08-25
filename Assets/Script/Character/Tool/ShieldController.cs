using CharacterTag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : ToolController
{
    public override ToolTag GetToolTag() { return ToolTag.Shield; }
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        SetTrigger();
    }

    private bool CheckGuridState()
    {
        switch (controller.GuardState)
        {
            case GuardState.Normal:
            case GuardState.Crouch:
                return true;
        }
        return false;
    }

    private void SetTrigger()
    {
        if (controller == null) { return; }
        if (collider == null) { return; }
        if (CheckGuridState())
        {
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }
    }
}
