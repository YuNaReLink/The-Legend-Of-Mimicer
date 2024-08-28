using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject movePosition;
    [SerializeField]
    private HitPlayerExecute hitExecute;
    [SerializeField]
    private Collider doorCollider;

    [SerializeField]
    private float playerSpeed = 3f;

    //�J���ɕK�v�ȃt���O
    [SerializeField]
    private bool open = false;
    [SerializeField]
    private bool movePlayer = false;
    [SerializeField]
    private bool close = false;

    [SerializeField]
    private Vector3 baseDoorPos = Vector3.zero;

    [SerializeField]
    private Vector3 moveDoorPos = new Vector3(0, 7, 0);

    [SerializeField]
    private float openSpeed = 10f;

    //���O�I�u�W�F�N�g
    //[SerializeField]
    //private LookObject lookObject;

    [SerializeField]
    private bool start = false;

    [SerializeField]
    private TriggerCheck triggerCheck = null;
    private void Start()
    {
        if (movePosition != null)
        {
            hitExecute = movePosition.GetComponent<HitPlayerExecute>();
        }

        //lookObject = GetComponentInChildren<LookObject>();

        open = false;
        movePlayer = false;
        close = false;
        start = false;

        baseDoorPos = transform.localPosition;
    }

    private void Update()
    {
        //0�Ԗ�:�L�[���͂ō쓮
        if (!open && !movePlayer && !close)
        {
            if (InputManager.GetItemButton()&& triggerCheck.IsHitPlayer())
            {
                start = true;
            }
        }

        //1�Ԗ�:�h�A�J�̏�����
        bool startOpenTheDoor = !open && !movePlayer && !close;
        if (startOpenTheDoor)
        {
            if (start)
            {
                doorCollider.isTrigger = true;
                open = true;
                hitExecute.enabled = false;
                triggerCheck.GetController().StopController = true;
                triggerCheck.GetController().StopMove();
                triggerCheck.GetController().GetMotion().ChangeMotion(CharacterTag.StateTag.Idle);
                start = false;
                return;
            }
        }


        //2�Ԗ�:�h�A����ɂ�����
        if (open)
        {
            MoveDoor();
        }

        if (!movePlayer)
        {
            if (hitExecute.PlayerHit)
            {
                Vector3 changePos = movePosition.transform.localPosition;
                changePos.z *= -1;
                movePosition.transform.localPosition = changePos;
                hitExecute.PlayerHit = false;
            }
        }
        else
        {
            //4�Ԗ�:�v���C���[�����̈ʒu�ɋ����I�Ɉړ�
            Vector3 lookPos = movePosition.transform.position;
            lookPos.y = triggerCheck.GetPlayer().transform.position.y;
            triggerCheck.GetPlayer().transform.LookAt(lookPos);
            triggerCheck.GetPlayer().transform.position = Vector3.MoveTowards(triggerCheck.GetPlayer().transform.position, movePosition.transform.position, playerSpeed * Time.deltaTime);
            triggerCheck.GetController().GetMotion().ChangeMotion(CharacterTag.StateTag.Run);
            //5�Ԗ�:�h�A��߂�t���O��ݒ�
            if (triggerCheck.GetPlayer().transform.position == movePosition.transform.position)
            {
                movePlayer = false;
                close = true;
            }
        }


        //6�Ԗ�:�h�A��߁A�I�������ʒu���C���A���̑��̐ݒ���s��
        if (close)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, baseDoorPos, openSpeed * Time.deltaTime);
            if (transform.localPosition.y <= baseDoorPos.y)
            {
                doorCollider.isTrigger = false;
                transform.localPosition = new Vector3(0, baseDoorPos.y, 0);
                close = false;
                triggerCheck.GetController().StopController = false;
                hitExecute.enabled = true;
            }
        }
    }

    private void MoveDoor()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, moveDoorPos, openSpeed * Time.deltaTime);
        //3�Ԗ�:�h�A���w�肵���ʒu�܂ŏオ�����炻�̈ʒu����
        Vector3 doorpos = transform.localPosition;
        if (doorpos.y >= moveDoorPos.y)
        {
            transform.localPosition = new Vector3(0, moveDoorPos.y, 0);
            open = false;
            movePlayer = true;
        }
    }
}
