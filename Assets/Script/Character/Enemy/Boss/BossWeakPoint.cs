using UnityEngine;

/// <summary>
/// ボスの弱点オブジェクトにアタッチして
/// 当たり判定の処理を行う
/// </summary>
public class BossWeakPoint : MonoBehaviour
{
    private BossController controller = null;

    private void Awake()
    {
        controller = GetComponentInParent<BossController>();
        if(controller == null)
        {
            Debug.LogError("controllerがアタッチされていません(BossController)");
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
