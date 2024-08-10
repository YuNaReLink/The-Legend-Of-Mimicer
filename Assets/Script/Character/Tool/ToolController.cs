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
    protected float damageData = 1f;
    public float GetDamage() {  return damageData; }
}
