using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("�J�������Ǐ]����^�[�Q�b�g")]
    [SerializeField]
    private GameObject target;
    [Header("�^�[�Q�b�g���v���C���[�̎��ɃA�^�b�`�����C���X�^���X�錾")]
    [SerializeField]
    private PlayerController player;

    [Header("�J�����ƃ^�[�Q�b�g�̏�������")]
    [SerializeField]
    private float distance_base = 10.0f;
    [SerializeField]
    private float maxDistanceBase = 10.0f;
    [Header("�J������X�ړ��X�s�[�h")]
    [SerializeField]
    private float mouseXSpeed = 3.0f;
    [Header("�J������Y�ړ��X�s�[�h")]
    [SerializeField]
    private float mouseYSpeed = 1.5f;

    /// <summary>
    /// �J�����̉�]�ʂ�ێ��������
    /// </summary>
    [SerializeField]
    private float rotation_hor;
    [SerializeField]
    private float rotation_ver;

    [SerializeField]
    private float resetCameraSpeed = 5.0f;

    private Vector3 targettrack;
    [Header("=================")]
    [SerializeField]
    private float desiredDistanceBehindPlayer = 3.0f;
    [SerializeField]
    private float focusCameraPosX = 1.5f;
    [SerializeField]
    private float focusCameraPosY = 2.0f;

    [SerializeField]
    private Vector3 initCameraRotation = new Vector3(0, 0.2f, -5);
    // �J�����ƃv���C���[�̋���
    [SerializeField]
    private float baseDistance = 2;
    // �J�����̍���
    [SerializeField]
    private float height = 1.0f;
    // �J�����̓����̊��炩��
    [SerializeField]
    private float damping = 10.0f;

    [SerializeField]
    private float neckHeight = 0;

    [SerializeField]
    private float testHor = 0;

    //���ڂ��邽�߂̃t���O
    private static bool focusFlag = false;

    public static bool FocusFlag { get { return focusFlag; } set { focusFlag = value; } }

    //���ڂ�����W��ێ��������
    private static GameObject lockObject;

    public static GameObject LockObject { get { return lockObject; }set { lockObject = value; } }

    [SerializeField]
    private bool fpsMode = false;
    public bool IsFPSMode() {  return fpsMode; }

    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    private void Start()
    {
        target = GameObject.FindWithTag("Player");

        rotation_hor = 0f;
        rotation_ver = 0f;
        targettrack = Vector3.zero;
    }

    private void Update()
    {
        SetCameraMode();

        if (fpsMode)
        {
            Vector3 fpsPos = player.gameObject.transform.position;
            fpsPos.y = player.gameObject.transform.position.y + neckHeight;
            transform.position = fpsPos;
            // ���݂̉�]�p�x���擾
            Vector3 currentRotation = transform.eulerAngles;

            // z���̉�]��0�ɐݒ�
            currentRotation.z = 0;

            // ��]�p�x��ݒ�
            transform.eulerAngles = currentRotation;
        }
        else
        {
            //�J�������Ǐ]���Ă�Ώۂ��`�F�b�N
            CheckTarget();

            //�J�����ƃv���C���[�Ƃ̋�����1.0f�ɐݒ�
            distance_base -= Input.mouseScrollDelta.y * 0.5f;
            if (distance_base < 1.0f)
            {
                distance_base = 1.0f;
            }
            else if (distance_base > maxDistanceBase)
            {
                distance_base = maxDistanceBase;
            }

            if(Mathf.Abs(rotation_hor) >= 360)
            {
                rotation_hor = 0;
            }
        }

    }

    private void SetCameraMode()
    {
        if (InputManager.PushEKey())
        {
            fpsMode = true;
        }
        if (InputManager.PushQKey()||InputManager.PushMouseLeft())
        {
            fpsMode = false;
        }
    }

    private void CheckTarget()
    {
        if(player != null) { return; }
        if(target.tag == "Player")
        {
            player = target.GetComponent<PlayerController>();
        }
    }

    void FixedUpdate()
    {
        if (fpsMode)
        {
            cameracon();
        }
        else
        {
            MouseInputCamera();
        }
    }

    private void MouseInputCamera()
    {
        if (target == null||player == null)
        {
            return;
        }
        if(lockObject == null)
        {
            focusFlag = false;
            player.GetKeyInput().CKey = false;
        }
        if (focusFlag&&player.GetKeyInput().IsCKeyEnabled())
        {
            Vector3 directionToEnemy = lockObject.transform.position - transform.position;
            // �v���C���[�̃��[�J�����W�n�ł̃J�����̃I�t�Z�b�g
            Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
            Vector3 offset = new Vector3(focusCameraPosX, focusCameraPosY, -desiredDistanceBehindPlayer);
            // �v���C���[�̉�]�ɍ��킹�ă��[�J�����W�n�̃I�t�Z�b�g��ϊ�
            Vector3 rotatedOffset = player.transform.rotation * offset;
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
            // ���[���h���W�n�ɕϊ����ăJ�����̈ʒu�ɓK�p
            transform.position = Vector3.Lerp(transform.position, player.transform.position + rotatedOffset, Time.deltaTime * 2.0f);
            return;
        }
        else if (!focusFlag && player.GetKeyInput().IsCKeyEnabled())
        {
            float playerRotationY = player.transform.rotation.eulerAngles.y;
            float playerRotationX = player.transform.rotation.eulerAngles.x;
            rotation_hor = Mathf.Lerp(rotation_hor, playerRotationY, Time.deltaTime * resetCameraSpeed);
            rotation_ver = Mathf.Lerp(rotation_ver, playerRotationX, Time.deltaTime * resetCameraSpeed);
        }
        else
        {
            //�J��������]����X�s�[�h��ݒ�
            rotation_hor += Input.GetAxis("Mouse X") * mouseXSpeed;
            rotation_ver -= Input.GetAxis("Mouse Y") * mouseYSpeed;
        }

        //restrict vertical angle to -90 ~ +90
        if (Mathf.Abs(rotation_ver) > 90)
            rotation_ver = Mathf.Sign(rotation_ver) * 90;

        //base vector to rotate
        var rotation = Vector3.Normalize(initCameraRotation); //base(normalized)
        rotation = Quaternion.Euler(rotation_ver, rotation_hor, 0) * rotation; //rotate vector

        //�J�����̖��܂��h�����߂Ƀ��C���[���w�肷��
        RaycastHit hit;
        int layermask = 1 << 3; //1�̃r�b�g��3���C���[��(Floor_obstacle������ꏊ)�������V�t�g
        float distance = distance_base; //copy default(mouseScroll zoom)
        //�X�t�B�A���C�L���X�g�Ŗ��܂�h�~
        if (Physics.SphereCast(targettrack + Vector3.up * 1.7f, 0.5f,
        rotation, out hit, distance, layermask))
        {
            distance = hit.distance; //overwrite copy
        }

        //turn self
        transform.rotation = Quaternion.Euler(rotation_ver, rotation_hor, 0); //Quaternion IN!!

        //turn around + zoom
        transform.position = rotation * distance;

        //��]�̒��S���W��Y�𒲐�
        var necklevel = Vector3.up * neckHeight;
        transform.position += necklevel;

        //�J�����̈ړ�(Lerp���g���Đ��`���)
        targettrack = Vector3.Lerp(
            targettrack,target.transform.position, Time.deltaTime * 10);
        transform.position += targettrack;
    }

    void cameracon()
    {
        float x_Rotation = Input.GetAxis("Mouse X");
        float y_Rotation = Input.GetAxis("Mouse Y");
        x_Rotation = x_Rotation * mouseXSpeed;
        y_Rotation = y_Rotation * mouseYSpeed;
        transform.Rotate(0, x_Rotation, 0);
        transform.Rotate(-y_Rotation, 0, 0);
    }
}
