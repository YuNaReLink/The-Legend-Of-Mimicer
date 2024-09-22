using UnityEngine;

/// <summary>
/// �{�X�̎�_�I�u�W�F�N�g�ɃA�^�b�`����
/// �����蔻��̏������s��
/// </summary>
public class BossWeakPoint : MonoBehaviour
{
    private BossController controller = null;

    private void Awake()
    {
        controller = GetComponentInParent<BossController>();
        if(controller == null)
        {
            Debug.LogError("controller���A�^�b�`����Ă��܂���(BossController)");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        ToolController toolController = other.GetComponent<ToolController>();
        if(toolController == null) { return; }
        if(controller == null) { return; }
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
                controller.GetEffectController().CreateVFX((int)EffectTagList.CharacterEffectTag.Damage, other.transform.position, 1f, Quaternion.identity);
                controller.SetStunFlag(true);
                break;
            case CharacterTagList.StateTag.Damage:
                controller.GetEffectController().CreateVFX((int)EffectTagList.CharacterEffectTag.Damage, other.transform.position, 1f, Quaternion.identity);
                controller.GetBossDamageCommand().Attacker = other.gameObject;
                controller.GetBossDamageCommand().SetDamageFlag(true);
                break;
        }
    }
}
