using UnityEngine;


/// <summary>
/// �X�e�[�W��̃v���C���[�̏�񂪕K�v�ȃM�~�b�N�I�u�W�F�N�g�ɃA�^�b�`��
/// �v���C���[�����̃M�~�b�N�I�u�W�F�N�g�ɐG��Ă��邩���ʁA�����擾����N���X
/// </summary>
public class HitPlayerExecute : MonoBehaviour
{
    [SerializeField]
    private bool playerHit = false;

    public bool PlayerHit { get { return playerHit; } set { playerHit = value; } }

    public void OnTriggerEnter(Collider other)
    {
        playerHit = false;
        if (other.tag != "Player") { return; }
        playerHit = true;
    }
}
