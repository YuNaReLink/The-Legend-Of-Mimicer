using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����̂�����������Ă���L�����N�^�[�̏������ɏ������s���x�[�X�N���X
/// </summary>
public class ToolController : BaseAttackController
{
    [SerializeField]
    protected ItemData itemData = null;
    public ItemData GetItemData() { return itemData; }
}
