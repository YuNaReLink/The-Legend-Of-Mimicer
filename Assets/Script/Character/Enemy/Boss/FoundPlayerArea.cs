using UnityEngine;

/// <summary>
/// �{�X���v���C���[�̏����擾����I�u�W�F�N�g�ɃA�^�b�`��
/// �v���C���[�������蔻��Ƀq�b�g�������ɏ������s��
/// </summary>
public class FoundPlayerArea : MonoBehaviour
{
    private BossController controller = null;

    private void Awake()
    {
        controller = GetComponentInParent<BossController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") { return; }
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if(player == null) { return; }
        controller.Target = player;
        if(GameSceneSystemController.Instance != null)
        {
            GameSceneSystemController.Instance.BossBattleStart = true;
        }
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag != "Player") { return; }
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player == null) { return; }
        controller.Target = player;
        if(GameSceneSystemController.Instance != null)
        {
            GameSceneSystemController.Instance.BossBattleStart = true;
        }
        Destroy(gameObject);
    }

}
