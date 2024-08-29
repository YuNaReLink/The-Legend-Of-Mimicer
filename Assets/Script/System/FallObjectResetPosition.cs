using UnityEngine;

/// <summary>
/// �v���C���[���X�e�[�W���痎�������̏������s���N���X
/// �v���C���[���X�e�[�W���痎������Ō�ɒ��n���Ă����Ƃ����
/// �ʒu��ݒ肷�鏈�����s��
/// </summary>
public class FallObjectResetPosition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") { return; }
        ResetPosition(other);
    }

    private void ResetPosition(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        controller.GetKeyInput().GetUpInput();
        controller.CharacterRB.velocity = controller.StopMoveVelocity();
        controller.Velocity = controller.StopMoveVelocity();
        other.transform.position = controller.GetLandingPosition();
        controller.GetKeyInput().GetUpInput();
    }
}
