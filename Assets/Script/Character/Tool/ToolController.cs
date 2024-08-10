using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道具自体が道具を持っているキャラクターの情報を元に処理を行うベースクラス
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
