using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera              myCamera = null;

    [Header("�J�������Ǐ]����^�[�Q�b�g")]
    [SerializeField]
    private GameObject          target;

    private PlayerController    player;

    [Header("�J�����ƃ^�[�Q�b�g�̏�������")]
    [SerializeField]
    private float               distance_base = 10.0f;
    [SerializeField]
    private float               maxDistanceBase = 10.0f;
    [Header("�J������X�ړ��X�s�[�h")]
    [SerializeField]
    private float               mouseXSpeed = 3.0f;
    [Header("�J������Y�ړ��X�s�[�h")]
    [SerializeField]
    private float               mouseYSpeed = 1.5f;

    /// <summary>
    /// �J�����̉�]�ʂ�ێ��������
    /// </summary>
    [SerializeField]
    private float               rotation_hor;
    [SerializeField]
    private float               rotation_ver;

    [SerializeField]
    private float               resetCameraSpeed = 5.0f;

    private Vector3             targettrack;
    [Header("=================")]
    [SerializeField]
    private float               desiredDistanceBehindPlayer = 3.0f;
    [SerializeField]
    private float               focusCameraPosX = 1.5f;
    [SerializeField]
    private float               focusCameraPosY = 2.0f;

    [SerializeField]
    private Vector3             initCameraRotation = new Vector3(0, 0.2f, -5);
    // �J�����ƃv���C���[�̋���
    [SerializeField]
    private float               baseDistance = 2;
    // �J�����̍���
    [SerializeField]
    private float               height = 1.0f;
    // �J�����̓����̊��炩��
    [SerializeField]
    private float               damping = 10.0f;

    [SerializeField]
    private float               neckHeight = 0;

    [SerializeField]
    private float               testHor = 0;

    //���ڂ��邽�߂̃t���O
    private static bool         focusFlag = false;

    public static bool          FocusFlag { get { return focusFlag; } set { focusFlag = value; } }

    //���ڂ�����W��ێ��������
    private static GameObject   lockObject;

    public static GameObject    LockObject { get { return lockObject; }set { lockObject = value; } }

    [SerializeField]
    private bool                fpsMode = false;
    public bool IsFPSMode() {  return fpsMode; }

    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    private void Start()
    {
        myCamera = GetComponent<Camera>();
        if(myCamera == null)
        {
            Debug.LogError("�J�������A�^�b�`����܂���ł����B");
        }
        target = GameObject.FindWithTag("Player");
        if (target != null && player == null)
        {
            player = target.GetComponent<PlayerController>();
        }

        rotation_hor = 0f;
        rotation_ver = 0f;
        targettrack = Vector3.zero;

        distance_base = maxDistanceBase;
    }

    private void Update()
    {
        //�J�������Ǐ]���Ă�Ώۂ��`�F�b�N
        CheckTarget();
        switch (GameManager.GameState)
        {
            case GameManager.GameStateEnum.Game:
                GameStateCameraControle();
                break;
            case GameManager.GameStateEnum.GameOver:
                neckHeight -= 0.02f;
                if(neckHeight < 0.5f)
                {
                    neckHeight = 0.5f;
                }
                distance_base -= 0.02f;
                if (distance_base < 3.0f)
                {
                    distance_base = 3.0f;
                }
                break;
            case GameManager.GameStateEnum.GameClear:
                distance_base = 10.0f;
                break;
        }
    }

    private void GameStateCameraControle()
    {
        if(player == null) { return; }
        SetCameraMode();
        if (fpsMode)
        {
            Vector3 fpsPos = player.transform.position;
            fpsPos.y = player.transform.position.y + neckHeight;
            transform.position = fpsPos;
            // ���݂̉�]�p�x���擾
            Vector3 currentRotation = transform.eulerAngles;

            // z���̉�]��0�ɐݒ�
            currentRotation.z = 0;

            // ��]�p�x��ݒ�
            transform.eulerAngles = currentRotation;
        }
        if (Mathf.Abs(rotation_hor) >= 360)
        {
            rotation_hor = 0;
        }
    }

    private void SetCameraMode()
    {
        ChangeFpsMode(player.GetToolController().CurrentToolTag == ToolInventoryController.ToolObjectTag.CrossBow);
    }

    void ChangeFpsMode(bool mode)
    {
        fpsMode = mode;
        foreach (var renderer in player.GetRendererData().GetRendererList())
        {
            renderer.enabled = !mode;
        }
    }

    private void CheckTarget()
    {
        switch (GameManager.GameState)
        {
            case GameManager.GameStateEnum.Game:
            case GameManager.GameStateEnum.Pose:
                if (player != null) { return; }
                if(target.tag == "Player")
                {
                    player = target.GetComponent<PlayerController>();
                }
                break;
            case GameManager.GameStateEnum.GameOver:
                break;
            case GameManager.GameStateEnum.GameClear:
                if(target == GameSceneSystemController.Instance.GetCameraFocusObject()) { return; }
                GameObject o = GameSceneSystemController.Instance.GetCameraFocusObject();
                if(o.GetComponent<BossController>() == null) { return; }
                target = o;
                break;
        }
    }

    void FixedUpdate()
    {
        switch (GameManager.GameState)
        {
            case GameManager.GameStateEnum.Game:
                if (player == null) { return; }
                if (fpsMode)
                {
                    FPSCamera();
                }
                else
                {
                    TPSCamera();
                }
                break;
            case GameManager.GameStateEnum.GameOver:
                if (player == null) { return; }
                GameOverCamera();
                break;
            case GameManager.GameStateEnum.GameClear:
                TargetCameraUpdate();
                break;
        }
    }

    /// <summary>
    /// �ȉ��̓v���C���[�𒆐S�ɃJ�����𐧌䂷�鏈��
    /// </summary>
    private void TPSCamera()
    {
        if (target == null||player == null){return;}
        if(lockObject == null)
        {
            focusFlag = false;
        }
        if (focusFlag&&player.GetKeyInput().IsCameraLockEnabled())
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
        else if (!focusFlag && player.GetKeyInput().IsCameraLockEnabled() || NoTPSCameraState())
        {
            ResetCameraAngles(resetCameraSpeed);
        }
        else
        {
            //�J��������]����X�s�[�h��ݒ�
            rotation_hor += InputManager.CameraXInput() * mouseXSpeed;
            rotation_ver -= InputManager.CameraYInput() * mouseYSpeed;
        }

        MoveCameraPositionAndRotatetion();
    }
    /// <summary>
    /// State�̏�Ԃɂ���ăJ�����̕��������Z�b�g���邩���߂�t���O
    /// </summary>
    /// <returns></returns>
    private bool NoTPSCameraState()
    {
        switch (player.CurrentState)
        {
            case CharacterTag.StateTag.Grab:
            case CharacterTag.StateTag.ClimbWall:
            case CharacterTag.StateTag.WallJump:
                return true;
        }
        return false;
    }
    /// <summary>
    /// �J�����̕������v���C���[���o�b�N�ɉf���ʒu�ɐ��`��Ԃňړ�
    /// �����ňړ�����X�s�[�h��ݒ�
    /// </summary>
    /// <param name="speed"></param>
    public void ResetCameraAngles(float speed)
    {
        float playerRotationY = player.transform.rotation.eulerAngles.y;
        if (playerRotationY - rotation_hor > 180)
        {
            playerRotationY -= 360;
        }
        float playerRotationX = player.transform.rotation.eulerAngles.x;
        rotation_hor = Mathf.Lerp(rotation_hor, playerRotationY, Time.deltaTime * speed);
        rotation_ver = Mathf.Lerp(rotation_ver, playerRotationX, Time.deltaTime * speed);
    }

    public bool IsCameraVerticalRotation()
    {
        return rotation_ver  - player.transform.rotation.eulerAngles.x < 0.1f;
    }

    private void FPSCamera()
    {
        rotation_hor += InputManager.CameraXInput() * mouseXSpeed;
        rotation_ver -= InputManager.CameraYInput() * mouseYSpeed;

        MoveCameraPositionAndRotatetion();
    }

    private void GameOverCamera()
    {
        rotation_hor += 2f;
        float dis = player.transform.rotation.eulerAngles.y + 145 - rotation_hor;
        if(dis <= 0.1f)
        {
            rotation_hor = player.transform.rotation.eulerAngles.y + 145;
        }
        rotation_ver = 0;

        MoveCameraPositionAndRotatetion();
    }

    private void MoveCameraPositionAndRotatetion()
    {
        //restrict vertical angle to -90 ~ +90
        if (Mathf.Abs(rotation_ver) > 60)
            rotation_ver = Mathf.Sign(rotation_ver) * 60;

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
            targettrack, target.transform.position, Time.deltaTime * 10);

        transform.position += targettrack;
    }

    /// <summary>
    /// �ȉ��̓N���A��������target�ɓ����Ă�I�u�W�F�N�g�𒆐S�ɃJ�����𐧌䂷�鏈��
    /// </summary>
    private void TargetCameraUpdate()
    {
        if (target == null) {return;}

        rotation_hor = target.transform.rotation.eulerAngles.y;
        rotation_ver = 60;

        MoveCameraPositionAndRotatetion();
    }
}
