using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    /// <summary>
    /// �����J�����Ƃ�
    /// �v���C���[���ړ����邽�߂̍��W��ێ�����I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private GameObject          movePosition;
    /// <summary>
    /// movePosition�I�u�W�F�N�g�����
    /// ���Α��Ɉړ������邩���肷��N���X
    /// </summary>
    [SerializeField]
    private HitPlayerExecute    hitExecute;
    /// <summary>
    /// �h�A�̃R���C�_�[��ێ�����ϐ�
    /// </summary>
    [SerializeField]
    private Collider            doorCollider;
    /// <summary>
    /// �v���C���[�������ňړ����鎞�̑��x
    /// </summary>
    [SerializeField]
    private float               playerSpeed = 3f;

    /// <summary>
    /// �����J�����߂̃t���O
    /// </summary>
    [SerializeField]
    private bool                open = false;
    /// <summary>
    /// �v���C���[���ړ����邽�߂̃t���O
    /// </summary>
    [SerializeField]
    private bool                movePlayer = false;
    /// <summary>
    /// �����܂邽�߂̃t���O
    /// </summary>
    [SerializeField]
    private bool                close = false;
    /// <summary>
    /// �ŏ��̔��̈ʒu��ۑ�����Vector3
    /// </summary>
    [SerializeField]
    private Vector3             baseDoorPos = Vector3.zero;
    /// <summary>
    /// �����������̓������W
    /// </summary>
    [SerializeField]
    private Vector3             moveDoorPos = new Vector3(0, 7, 0);
    /// <summary>
    /// ���̊J���x
    /// </summary>
    [SerializeField]
    private float               openSpeed = 10f;
    /// <summary>
    /// �����J���Ă���܂�܂ł̃t���O
    /// </summary>
    [SerializeField]
    private bool                start = false;
    /// <summary>
    /// �L�[���͂̔�����s���N���X
    /// </summary>
    [SerializeField]
    private TriggerCheck        triggerCheck = null;
    private void Awake()
    {
        if (movePosition != null)
        {
            hitExecute = movePosition.GetComponent<HitPlayerExecute>();
        }
    }
    private void Start()
    {

        open = false;
        movePlayer = false;
        close = false;
        start = false;

        baseDoorPos = transform.localPosition;
    }

    private void Update()
    {
        //0�Ԗ�:�L�[���͂ō쓮
        KeyInput();

        //1�Ԗ�:�h�A�J�̏�����
        InitilaizeOpenDoor();

        //2�Ԗ�:�h�A����ɂ�����
        Open();

        MovePlayer();

        //6�Ԗ�:�h�A��߁A�I�������ʒu���C���A���̑��̐ݒ���s��
        Close();
    }

    private void KeyInput()
    {
        if (!open && !movePlayer && !close)
        {
            if (InputManager.GetItemButton() && triggerCheck.IsHitPlayer())
            {
                start = true;
                triggerCheck.SetTriggerTag(false);
            }
        }
    }

    private void InitilaizeOpenDoor()
    {
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
                triggerCheck.GetController().GetMotion().ChangeMotion(CharacterTagList.StateTag.Idle);
                start = false;
                return;
            }
        }
    }

    private void Open()
    {
        if (!open) { return; }
        MoveDoor();
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

    private void MovePlayer()
    {
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
            triggerCheck.GetController().GetMotion().ChangeMotion(CharacterTagList.StateTag.Run);
            //5�Ԗ�:�h�A��߂�t���O��ݒ�
            if (triggerCheck.GetPlayer().transform.position == movePosition.transform.position)
            {
                movePlayer = false;
                close = true;
            }
        }
    }

    private void Close()
    {
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
                triggerCheck.SetTriggerTag(true);
            }
        }
    }
}
