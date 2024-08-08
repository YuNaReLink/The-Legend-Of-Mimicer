using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusArea : MonoBehaviour
{
    [SerializeField]
    private PlayerController controller;
    [SerializeField]
    private SphereCollider areaCollider;

    private Ray ray;
    private RaycastHit hit;
    //Ray���΂�����
    private Vector3 direction;
    //Ray���΂�����
    [SerializeField]
    private float distance = 10f;

    [SerializeField]
    private float searchAngle = 130f;

    // Start is called before the first frame update
    void Start()
    {
        areaCollider = GetComponent<SphereCollider>();
        if (areaCollider == null)
        {
            Debug.Log("areaCollider���A�^�b�`����܂���ł���(Enemy)");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Target") { return; }
        //Ray���΂��������v�Z
        Vector3 temp = other.transform.position - transform.position;
        direction = temp.normalized;
        //Ray���΂�
        ray = new Ray(transform.position, direction);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);  // Ray���V�[����ɕ`��
        //��l���̕���
        var playerDirection = other.transform.position - transform.position;
        //�v���C���[�̑O������̎�l���̕���
        var angle = Vector3.Angle(transform.forward, playerDirection);
        //�T�[�`����p�x���������甭��
        // Ray���ŏ��ɓ����������̂𒲂ׂ�
        if(angle > searchAngle) { return; }
        RaycastHit[] hits = Physics.RaycastAll(ray.origin,ray.direction * distance);
        foreach(var hit in hits)
        {
            if (hit.collider.CompareTag("Target"))
            {
                CheckSameEnemy(other);
            }
        }
    }

    private void CheckSameEnemy(Collider other)
    {
        if (PlayerCameraController.LockObject != null) { return; }
        PlayerCameraController.FocusFlag = true;
        PlayerCameraController.LockObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Target")
        {
            RemoveEnemyList(other);
        }
    }

    private void RemoveEnemyList(Collider other)
    {
        if (PlayerCameraController.LockObject == null) { return; }
        if (PlayerCameraController.LockObject == other.gameObject)
        {
            PlayerCameraController.FocusFlag = false;
            PlayerCameraController.LockObject = null;
        }
    }
}
