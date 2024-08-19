using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchArea : MonoBehaviour
{
    [SerializeField]
    private EnemyController controller = null;
    [SerializeField]
    private SphereCollider  searchCollider = null;
    [SerializeField]
    private float searchAngle = 180f;

    private Ray ray;
    private RaycastHit hit;
    //Ray���΂�����
    private Vector3 direction;
    //Ray���΂�����
    [SerializeField]
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        searchCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        HandleCollision(other);
    }
    private void OnCollisionStay(Collision collision)
    {
        HandleCollision(collision.collider);
    }

    private void HandleCollision(Collider other)
    {
        if (other.tag != "Player") { return; }
        //Ray���΂��������v�Z
        Vector3 temp = other.transform.position - transform.position;
        direction = temp.normalized;
        //Ray���΂�
        ray = new Ray(transform.position, direction);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);  // Ray���V�[����ɕ`��
        //��l���̕���
        var playerDirection = other.transform.position - transform.position;
        //�G�̑O������̎�l���̕���
        var angle = Vector3.Angle(transform.forward, playerDirection);
        //�T�[�`����p�x���������甭��
        if (angle <= searchAngle)
        {
            // �v���C���[�ƓG�̊Ԃɏ�Q�����Ȃ����`�F�b�N
            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    controller.Target = other.GetComponent<PlayerController>();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") { return; }
        //�G�̏�Ԃ�ύX
        controller.Target = null;
    }
}
