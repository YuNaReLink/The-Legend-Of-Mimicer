using UnityEngine;

public class BaseAttackController : MonoBehaviour
{
    public virtual ToolTag GetToolTag() { return ToolTag.Null; }

    [SerializeField]
    protected CharacterController controller = null;

    public void SetController(CharacterController _controller)
    {
        controller = _controller;
    }

    [SerializeField]
    protected Collider collider = null;

    [SerializeField]
    private WeaponStateData statusData = null;
    public WeaponStateData GetStatusData() { return statusData; }
    private void Awake()
    {
        controller = GetComponentInParent<CharacterController>();

        collider = GetComponent<Collider>();

    }
}
