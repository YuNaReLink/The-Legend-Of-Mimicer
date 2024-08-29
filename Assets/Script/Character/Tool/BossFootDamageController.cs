using UnityEngine;

public class BossFootDamageController : ToolController
{
    public override ToolTag GetToolTag() { return ToolTag.Other; }
    private void Awake()
    {
        controller = GetComponentInParent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("controllerがアタッチされていません");
        }
        collider = GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("colliderがアタッチされていません");
        }
    }
    private void Start()
    {
        collider.enabled = false;
    }

    void Update()
    {
        SetTrigger();
    }

    private bool CheckAttackState()
    {
        switch (controller.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
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
