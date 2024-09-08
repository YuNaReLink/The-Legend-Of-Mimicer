using UnityEngine;

public class TargetSearchArea : MonoBehaviour
{
    [SerializeField]
    private EnemyController controller = null;
    public float angle = 130f;

    private void Awake()
    {
        controller = GetComponentInParent<EnemyController>();
    }

    private void OnTriggerStay(Collider other)
    {
        //���E�͈͓̔��̓����蔻��
        if (other.gameObject.tag != "Player") { return; }
        //���E�̊p�x���Ɏ��܂��Ă��邩
        Vector3 posDelta = other.transform.position - transform.position;
        float target_angle = Vector3.Angle(transform.forward, posDelta);
        //target_angle��angle�Ɏ��܂��Ă��邩�ǂ���
        if (target_angle < angle) 
        {
            if (Physics.Raycast(transform.position, posDelta, out RaycastHit hit)) //Ray���g�p����target�ɓ������Ă��邩����
            {
                if (hit.collider == other)
                {
                    if (controller.Target == null)
                    {
                        controller.Target = other.GetComponent<PlayerController>();
                    }
                    Debug.Log("range of view");
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
