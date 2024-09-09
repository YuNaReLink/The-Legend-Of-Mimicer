using UnityEngine;

public class ShieldController : ToolController
{
    public override ToolTag GetToolTag() { return ToolTag.Shield; }
    
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
        switch (controller.CharacterStatus.GuardState)
        {
            case CharacterTagList.GuardState.Normal:
            case CharacterTagList.GuardState.Crouch:
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
