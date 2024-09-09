using UnityEngine;

/// <summary>
/// �v���C���[�̎��E�N���X
/// ���E�ɂ���I�u�W�F�N�g�ɃA�^�b�`����
/// �^�[�Q�b�g�Ƀ��b�N�I�����鏈��������
/// </summary>
public class FocusArea : MonoBehaviour
{
    [SerializeField]
    private PlayerController    controller = null;
    [SerializeField]
    private SphereCollider      areaCollider = null;

    private Ray ray;
    private RaycastHit          hit;
    //Ray���΂�����
    private Vector3             direction = Vector3.zero;
    //Ray���΂�����
    [SerializeField]
    private float               distance = 10f;

    [SerializeField]
    private float               searchAngle = 130f;

    void Start()
    {
        areaCollider = GetComponent<SphereCollider>();
        if (areaCollider == null)
        {
            Debug.Log("areaCollider���A�^�b�`����܂���ł���");
        }
    }
    private void Update()
    {
        if(CameraController.LockObject == null) { return; }
        if (!CameraController.LockObject.activeSelf)
        {
            CameraController.LockObject = null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Target") { return; }
        if (!GameSceneSystemController.Instance.BattleStart)
        {
            GameSceneSystemController.Instance.BattleStart = true;
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
        if (CameraController.LockObject != null) { return; }
        CameraController.FocusFlag = true;
        CameraController.LockObject = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Target") { return; }
        RemoveEnemyList(other);
        if (GameSceneSystemController.Instance.BattleStart)
        {
            GameSceneSystemController.Instance.BattleStart = false;
        }
    }
    private void RemoveEnemyList(Collider other)
    {
        if (CameraController.LockObject == null) { return; }
        if (CameraController.LockObject != other.gameObject) { return; }
        CameraController.FocusFlag = false;
        CameraController.LockObject = null;
    }
}
