using UnityEditor;
using UnityEngine;

public class SearchArea : MonoBehaviour
{
    [SerializeField]
    private EnemyController     controller = null;
    [SerializeField]
    private SphereCollider      searchArea = null;
    [SerializeField]
    private float               searchAngle = 180f;

    private Ray                 ray;
    private RaycastHit          hit;
    //Ray���΂�����
    private Vector3             direction = Vector3.zero;
    //Ray���΂�����
    [SerializeField]
    private float               distance = 0;

    private void Awake()
    {
        controller = GetComponentInParent<EnemyController>();
        searchArea = GetComponent<SphereCollider>();
    }
    private void OnTriggerStay(Collider other)
    {
        HandleCollision(other);
    }
    private void HandleCollision(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if(playerController == null) { return; }
        //Ray���΂��������v�Z
        Vector3 temp =                      other.transform.position - transform.position;
        direction =                         temp.normalized;
        //Ray���΂�
        ray =                               new Ray(transform.position, direction);
        // Ray���V�[����ɕ`��
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);  
        //��l���̕���
        var playerDirection =               other.transform.position - transform.position;
        //�G�̑O������̎�l���̕���
        var angle =                         Vector3.Angle(transform.forward, playerDirection);
        //�T�[�`����p�x���������甭��
        if (angle <= searchAngle)
        {
            // �v���C���[�ƓG�̊Ԃɏ�Q�����Ȃ����`�F�b�N
            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    controller.Target = playerController;
                    Debug.Log("�v���C���[�������Ă�");
                }
            }
        }
        else
        {
            Debug.Log("�v���C���[�������Ă��Ȃ�");
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
