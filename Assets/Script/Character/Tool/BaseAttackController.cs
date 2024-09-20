using UnityEngine;

/// <summary>
/// �v���C���[�A�G��������ɃA�^�b�`����N���X���p������
/// �N���X
/// </summary>
public class BaseAttackController : MonoBehaviour
{
    [SerializeField]
    protected float                 attackPower = 0;
    public virtual float            AttackPower => attackPower;

    public virtual ToolTag          GetToolTag() { return ToolTag.Null; }

    [SerializeField]
    protected CharacterController   controller = null;

    public virtual void SetController(CharacterController _controller)
    {
        controller = _controller;
    }

    [SerializeField]
    protected Collider              collider = null;

    [SerializeField]
    protected WeaponStateData       statusData = null;
    public WeaponStateData          GetStatusData() { return statusData; }
    protected virtual void Awake()
    {
        controller = GetComponentInParent<CharacterController>();

        collider = GetComponent<Collider>();

        if (statusData != null)
        {
            attackPower = statusData.BaseDamagePower;
        }
    }
}
