using UnityEngine;

/// <summary>
/// �G�̍��G�g���K�[�ɃA�^�b�`���v���C���[�𔭌����鏈�����s���N���X
/// </summary>
public class TargetSearchArea : MonoBehaviour
{
    private EnemyController     controller = null;
    
    [SerializeField]
    public float                searchAngle = 130f;

    private const string targetTagName = "Player";

    private void Awake()
    {
        controller = GetComponentInParent<EnemyController>();
        
        if(controller == null)
        {
            Debug.LogError("EnemyController���A�^�b�`����Ă��܂���");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //���E�͈͓̔��̓����蔻��
        if (other.gameObject.tag != targetTagName) { return; }
        //���E�̊p�x���Ɏ��܂��Ă��邩
        Vector3 posDelta = other.transform.position - transform.position;
        float target_angle = Vector3.Angle(transform.forward, posDelta);
        //target_angle��angle�Ɏ��܂��Ă��邩�ǂ���
        if (target_angle < searchAngle) 
        {
            if (Physics.Raycast(transform.position, posDelta, out RaycastHit hit)) //Ray���g�p����target�ɓ������Ă��邩����
            {
                if (hit.collider == other)
                {
                    if (controller.Target == null)
                    {
                        controller.Target = other.GetComponent<PlayerController>();
                    }
                }
                else
                {
                    controller.Target = null;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController == null) { return; }
        //�G�̏�Ԃ�ύX
        controller.Target = null;
    }
}
