using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����̂�����������Ă���L�����N�^�[�̏������ɏ������s���x�[�X�N���X
/// </summary>
public class ToolController : MonoBehaviour
{
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
    public WeaponStateData GetStatusData() {  return statusData; }
    [SerializeField]
    private ItemData itemData = null;
    public ItemData GetItemData() { return itemData; }
}
