using UnityEngine;

public class CameraController : MonoBehaviour
{
    //���C���J�������擾
    private Camera              myCamera = null;
    [Header("�J�������Ǐ]����^�[�Q�b�g")]
    [SerializeField]
    private GameObject          target;
    //�v���C���[�̃C���X�^���X�錾
    private PlayerController    player;
    //�J�����R���g���[���[�Ŏg���ϐ�
    [SerializeField]
    private CameraStatus        cameraStatus;
    public CameraStatus         CameraStatus => cameraStatus;
    /// <summary>
    /// �J�����̉�]�ʂ�ێ��������
    /// </summary>
    private float               rotation_hor;

    private float               rotation_ver;
    //�J�������ǂꂭ�炢�����������擾����Vector3�ϐ�
    private Vector3             targettrack;


    [Header("�J�������^�[�Q�b�g�ɒ��ڂ������̃J�����̈ʒu���Œ肷��l"),SerializeField]
    private Vector3             focusCameraPosition = new Vector3(1.5f, 2.0f, 3.0f);

    [Header("�J��������]�����鎞�ɃX�s�[�h�𐳋K������ϐ�"),SerializeField]
    private Vector3             cameraRotationNormalizeSpeed = new Vector3(0, 0.2f, -5);

    [SerializeField]
    private float               neckHeight = 2.0f;

    //���ڂ��邽�߂̃t���O
    private static bool         focusFlag = false;

    public static bool          FocusFlag { get { return focusFlag; } set { focusFlag = value; } }

    //���ڂ�����W��ێ��������
    private static GameObject   lockObject;

    public static GameObject    LockObject { get { return lockObject; }set { lockObject = value; } }

    [SerializeField]
    private bool                fpsMode = false;
    public bool                 IsFPSMode() {  return fpsMode; }

    private void Awake()
    {
        myCamera = GetComponent<Camera>();
        target = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        if(myCamera == null)
        {
            Debug.LogError("�J�������A�^�b�`����܂���ł����B");
        }
        if (target != null)
        {
            player = target.GetComponent<PlayerController>();
        }

        cameraStatus.StartInitialize();

        rotation_hor = 0f;
        rotation_ver = 0f;
        targettrack = Vector3.zero;
    }

    private void Update()
    {
        //�J�������Ǐ]���Ă�Ώۂ��`�F�b�N
        CheckCameraTarget();
        switch (GameManager.GameState)
        {
            case GameManager.GameStateEnum.Game:
                GameStautsCameraControl();
                break;
            case GameManager.GameStateEnum.Pose:
                cameraStatus.PoseCameraControl();
                break;
            case GameManager.GameStateEnum.GameOver:
                GameOverCameraControl();
                break;
            case GameManager.GameStateEnum.GameClear:
                GameClearCameraControl();
                break;
        }
    }
    
    /// <summary>
    /// �Q�[���̏�Ԃɂ���ăJ�������Ǐ]����^�[�Q�b�g���m�F���K�v�Ȃ�^�[�Q�b�g��������
    /// </summary>
    private void CheckCameraTarget()
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
            case GameManager.GameStateEnum.GameClear:
                if(target == GameSceneSystemController.Instance.GetCameraFocusObject()) { return; }
                GameObject o = GameSceneSystemController.Instance.GetCameraFocusObject();
                if(o.GetComponent<BossController>() == null) { return; }
                target = o;
                break;
        }
    }
    /// <summary>
    /// �Q�[����Ԏ��̃J��������
    /// </summary>
    private void GameStautsCameraControl()
    {
        if(player == null) { return; }
        ChangeFpsMode(player.GetToolController().CurrentToolTag == ToolInventoryController.ToolObjectTag.CrossBow);
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
    /// <summary>
    /// FPS�J�������[�h����
    /// </summary>
    /// <param name="mode"></param>
    void ChangeFpsMode(bool mode)
    {
        if(fpsMode == mode) { return; }
        fpsMode = mode;
    }

    /// <summary>
    /// �Q�[���I�[�o�[���̃J�����Ƃ̋����ƍ����𒲐�����
    /// </summary>
    private const float removeNeckHeightNum = 0.02f;
    private const float minNeckHeight = 0.5f;
    private const float removeGameOverCameraDistance = 0.02f;
    private const float minGameOverCameraDistance = 3.0f;
    private void GameOverCameraControl()
    {
        neckHeight -= removeNeckHeightNum;
        if (neckHeight < minNeckHeight)
        {
            neckHeight = minNeckHeight;
        }
        cameraStatus.SetBaseDistance(cameraStatus.BaseDistance - removeGameOverCameraDistance);
        if (cameraStatus.BaseDistance < minGameOverCameraDistance)
        {
            cameraStatus.SetBaseDistance(minGameOverCameraDistance);
        }
    }
    /// <summary>
    /// �Q�[���N���A���̃J�����ƑΏۂ̋�����ݒ肷��֐�
    /// </summary>
    private const float gameClearDistance = 10.0f;
    private void GameClearCameraControl()
    {
        cameraStatus.SetBaseDistance(gameClearDistance);
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
    /// FPS�̃J�����̏������s���֐�
    /// </summary>
    private void FPSCamera()
    {
        rotation_hor += InputManager.CameraXInput() * cameraStatus.MouseXSpeed;
        rotation_ver -= InputManager.CameraYInput() * cameraStatus.MouseYSpeed;

        MoveCameraPositionAndRotatetion();
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
            Vector3 offset = new Vector3(focusCameraPosition.x, focusCameraPosition.y, -focusCameraPosition.z);
            // �v���C���[�̉�]�ɍ��킹�ă��[�J�����W�n�̃I�t�Z�b�g��ϊ�
            Vector3 rotatedOffset = player.transform.rotation * offset;
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
            // ���[���h���W�n�ɕϊ����ăJ�����̈ʒu�ɓK�p
            transform.position = Vector3.Lerp(transform.position, player.transform.position + rotatedOffset, Time.deltaTime * 2.0f);
            return;
        }
        else if (!focusFlag && player.GetKeyInput().IsCameraLockEnabled() || NoTPSCameraState())
        {
            ResetCameraAngles(cameraStatus.ResetCameraSpeed);
        }
        else
        {
            //�J��������]����X�s�[�h��ݒ�
            rotation_hor += InputManager.CameraXInput() * cameraStatus.MouseXSpeed;
            rotation_ver -= InputManager.CameraYInput() * cameraStatus.MouseYSpeed;
        }

        MoveCameraPositionAndRotatetion();
    }
    /// <summary>
    /// State�̏�Ԃɂ���ăJ�����̕��������Z�b�g���邩���߂�t���O
    /// </summary>
    /// <returns></returns>
    private bool NoTPSCameraState()
    {
        switch (player.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Grab:
            case CharacterTagList.StateTag.ClimbWall:
            case CharacterTagList.StateTag.WallJump:
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
    /// <summary>
    /// �J������X��]���l�ƃv���C���[��X��]���l�̍���0.1�ȉ����𒲂ׂ�֐�
    /// </summary>
    /// <returns></returns>
    public bool IsCameraVerticalRotation()
    {
        return rotation_ver  - player.transform.rotation.eulerAngles.x < 0.1f;
    }
    /// <summary>
    /// �Q�[���I�[�o�[���̃J�����̓����Ɖ�]���s���֐�
    /// </summary>
    private const float addGameOverCameraRotationHor = 2.0f;
    private const float rotationEulerAnglesYOffset = 145.0f;
    private void GameOverCamera()
    {
        rotation_hor += addGameOverCameraRotationHor;
        float dis = player.transform.rotation.eulerAngles.y + rotationEulerAnglesYOffset - rotation_hor;
        if(dis <= 0.1f)
        {
            rotation_hor = player.transform.rotation.eulerAngles.y + rotationEulerAnglesYOffset;
        }
        rotation_ver = 0;

        MoveCameraPositionAndRotatetion();
    }
    /// <summary>
    /// �N���A��������target�ɓ����Ă�I�u�W�F�N�g�𒆐S�ɃJ�����𐧌䂷�鏈��
    /// </summary>
    private const float targetRotationVer = 60.0f;
    private void TargetCameraUpdate()
    {
        if (target == null) {return;}

        rotation_hor = target.transform.rotation.eulerAngles.y;
        rotation_ver = targetRotationVer;

        MoveCameraPositionAndRotatetion();
    }
    /// <summary>
    /// �J�����̓����Ɖ�]���s���֐�
    /// </summary>
    private void MoveCameraPositionAndRotatetion()
    {
        //restrict vertical angle to -90 ~ +90
        if (Mathf.Abs(rotation_ver) > 60)
            rotation_ver = Mathf.Sign(rotation_ver) * 60;

        //base vector to rotate
        var rotation = Vector3.Normalize(cameraRotationNormalizeSpeed); //base(normalized)
        rotation = Quaternion.Euler(rotation_ver, rotation_hor, 0) * rotation; //rotate vector

        //�J�����̖��܂��h�����߂Ƀ��C���[���w�肷��
        RaycastHit hit;
        int layermask = 1 << 3; //1�̃r�b�g��3���C���[��(Floor_obstacle������ꏊ)�������V�t�g
        float distance = cameraStatus.BaseDistance; //copy default(mouseScroll zoom)
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
}
