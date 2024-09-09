using UnityEngine;

/// <summary>
/// �v���C���[���ǂ�R�̃A�N�V�����̔�����s���N���X
/// </summary>
public class ObstacleCheck : MonoBehaviour
{
    //�i���W�����v��Ray�̊J�n�ʒu
    [SerializeField]
    private float               stepCheckOffset = 0.5f;
    //�i���W�����v��Ray�̏�����̊J�n�ʒu
    [SerializeField]
    private float               stepUpCheckOffset = 0.4f;
    //�i���W�����v��Ray�̋����̐��l
    [SerializeField]
    private float               stepCheckDistance = 0.7f;
    //�R����W�����v���邩�𔻒肷��t���O
    [SerializeField]
    private bool                lowStep = false;

    //�R�W�����v�̃��[�V�����̃J�E���g
    [SerializeField]
    private byte                lowJumpCount = 0;

    private const byte          MaxLowJumpCount = 2;
    public byte                 GetLowJumpCount() { return lowJumpCount; }
    /// <summary>
    /// ���n���W��ۑ����邩���Ȃ��������߂�t���O
    /// </summary>
    private bool                savePosition = false;
    public bool                 IsSavePosition() { return savePosition; }

    public enum RayTag
    {
        Null = -1,
        Upper,
        Up,
        Middle,
        Bottom,
        DataEnd
    }
    /// <summary>
    /// �ǂ�o�邽�߂̔�����s���ϐ�
    /// </summary>
    [SerializeField]
    private float[]             wallCheckoffsetArray = new float[]
    {
        4f,
        2f,
        1f,
        0.25f
    };
    /// <summary>
    /// �R�����Ray�̒����̐��l���i�[�����z��
    /// </summary>
    [SerializeField]
    private float[]             wallCheckDistanceArray = new float[]
    {
        1f,
        0.7f,
        0.7f,
        0.7f
    };
    /// <summary>
    /// �ǂƂ̓����蔻����s���t���O���i�[�����z��
    /// </summary>
    [SerializeField]
    private bool[]              hitWallFlagArray = new bool[4];

    //�R��o�鎞�Ƀv���C���[��O�ɐi�܂��邽�߂̃X�s�[�h�ϐ�
    [SerializeField]
    private float               climbForward = 1.5f;
    //��L�̏�����ւ̃X�s�[�h�ϐ�
    [SerializeField]
    private float               climbUp = 2.35f;
    //�i���W�����v�t���O
    [SerializeField]
    private bool                stepJumpFlag = false;
    //�͂܂�&�o������Ă��Ȃ������肷��t���O
    [SerializeField]
    private bool                noGarbToClimbFlag = false;
    //�����ǂ�o��W�����v�t���O
    [SerializeField]
    private bool                wallJumpFlag = false;
    public bool                 IsWallJumpFlag() { return wallJumpFlag; }
    //�͂܂�𔻒肷��t���O
    [SerializeField]
    private bool                grabFlag = false;
    public bool                 IsGrabFlag() { return grabFlag; }
    //�͂܂���L�����Z������t���O
    [SerializeField]
    private bool                grabCancel = false;
    public void                 SetGrabCancel(bool flag) {  grabCancel = flag; }
    //�o����s�����߂̃t���O
    [SerializeField]
    private bool                climbFlag = false;
    public bool                 IsClimbFlag() { return climbFlag; }
    //�o�鎞�̊J�n�n�_��ێ�����ϐ�
    [SerializeField]
    private Vector3             climbOldPos = Vector3.zero;
    //�o�鎞�̃S�[���n�_��ێ�����ϐ�
    [SerializeField]
    private Vector3             climbPos = Vector3.zero;
    //�R���W�����v���������肷��bool�^
    [SerializeField]
    private bool                cliffJump = false;
    public bool                 CliffJumpFlag { get { return cliffJump; } set { cliffJump = value; } }

    /// <summary>
    /// �J�����̌������l�������ǂƂ̓����蔻��t���O
    /// </summary>
    [SerializeField]
    private bool[]              cameraForwardWallFlagArray = new bool[4];
    //Player�N���X�̃C���X�^���X�錾
    [SerializeField]
    private PlayerController    controller = null;
    public void                 SetController(PlayerController _controller) { controller = _controller; }

    private const float StopWallActionCount = 0.1f;
    /// <summary>
    /// �ǂƂ̓����蔻����s���֐�
    /// </summary>
    public void WallCheckInput()
    {
        if (controller.GetCameraController().IsFPSMode()) { return; }
        MoveDirectionCheck();
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.SpinAttack:
            case CharacterTagList.StateTag.ReadySpinAttack:
            case CharacterTagList.StateTag.JumpAttack:
                return;
            case CharacterTagList.StateTag.Rolling:
                if(controller.GetKeyInput().CurrentDirection != CharacterTagList.DirectionTag.Up)
                {
                    return;
                }
                break;
        }
        //�ǂ����邩�`�F�b�N�A�Ȃ������瑁�����^�[��
        bool wallhit = WallCheck();
        GrabCheck();
        if (!wallhit) { return; }
        InitializeObstacleFlag();
        if (grabFlag || climbFlag || grabCancel)
        {
            return; 
        }
        FootFallJumpCheck();
        SaveResetLandingPosition();

        DeltaTimeCountDown timerStopWallAction = controller.GetTimer().GetTimerWallActionStop();
        //�ǂƂ̓�����`�F�b�N
        if (controller.CharacterStatus.Landing)
        {
            if (controller.GetTimer().GetTimerWallActionStop().IsEnabled()) { return; }
            //�i���̃`�F�b�N
            bool stepCheck =
                hitWallFlagArray[(int)RayTag.Bottom] && !hitWallFlagArray[(int)RayTag.Up] &&
                !hitWallFlagArray[(int)RayTag.Middle] && !hitWallFlagArray[(int)RayTag.Upper];
            if (stepCheck)
            {
                timerStopWallAction.StartTimer(StopWallActionCount);
                timerStopWallAction.OnCompleted += () =>
                {
                    if (controller.GetKeyInput().Vertical == 0 &&
                        controller.GetKeyInput().Horizontal == 0) { return; }
                    stepJumpFlag = true;
                };
                return;
            }
            //�v���C���[�̐g���Ɠ����ǂ��`�F�b�N
            bool middleWallCheck =
                hitWallFlagArray[(int)RayTag.Bottom] && hitWallFlagArray[(int)RayTag.Middle] &&
                !hitWallFlagArray[(int)RayTag.Up] && !hitWallFlagArray[(int)RayTag.Upper];
            if (middleWallCheck)
            {
                timerStopWallAction.StartTimer(StopWallActionCount);
                timerStopWallAction.OnCompleted += () =>
                {
                    if (controller.GetKeyInput().Vertical == 0 &&
                        controller.GetKeyInput().Horizontal == 0) { return; }
                    noGarbToClimbFlag = true;
                };
                return;
            }
            //�v���C���[�������������ǂ��`�F�b�N
            bool highWallCheck =
                hitWallFlagArray[(int)RayTag.Middle] &&
                hitWallFlagArray[(int)RayTag.Up] && !hitWallFlagArray[(int)RayTag.Upper];
            if (highWallCheck)
            {
                timerStopWallAction.StartTimer(StopWallActionCount);
                timerStopWallAction.OnCompleted += () =>
                {
                    if (controller.GetKeyInput().Vertical == 0 &&
                        controller.GetKeyInput().Horizontal == 0) { return; }
                    wallJumpFlag = true;
                };
                return;
            }
        }
        else
        {
            //�v���C���[���������ɊR�ɓ����������̃`�F�b�N
            bool middleWallCheck =
                hitWallFlagArray[(int)RayTag.Bottom] && hitWallFlagArray[(int)RayTag.Middle] &&
                !hitWallFlagArray[(int)RayTag.Up] && !hitWallFlagArray[(int)RayTag.Upper];
            if (middleWallCheck)
            {
                grabFlag = true;
                return;
            }
        }
    }
    /// <summary>
    /// �L�[���͂ƃJ�����̕�������Ray�̕������擾����֐�
    /// </summary>
    /// <returns></returns>
    private Vector3 CreateRayAdvanceDirection()
    {
        float h = controller.GetKeyInput().Horizontal;
        float v = controller.GetKeyInput().Vertical;
        Vector3 cameraForward = controller.GetCameraDirection(Camera.main.transform.forward);
        Vector3 cameraRight = controller.GetCameraDirection(Camera.main.transform.right);
        return h * cameraRight + v * cameraForward;
    }
    /// <summary>
    /// �L�[���͂ňړ����Ă������Ray���΂��������s���֐�
    /// </summary>
    private void MoveDirectionCheck()
    {
        //Ray���΂������̈ʒu���擾
        Vector3 dir = CreateRayAdvanceDirection();
        //Ray4����for�ŏ���
        Ray[] wallCheckRay = new Ray[4];
        for (int i = 0; i < wallCheckRay.Length; i++)
        {
            //�������쐬
            wallCheckRay[i] = new Ray(transform.position + Vector3.up * wallCheckoffsetArray[i], dir);
            //��΂��������ɉ�����������������
            cameraForwardWallFlagArray[i] = Physics.Raycast(wallCheckRay[i], wallCheckDistanceArray[i]);
            //��������
            Debug.DrawRay(wallCheckRay[i].origin, wallCheckRay[i].direction * wallCheckDistanceArray[i], Color.green);
        }
    }
    private bool WallCheck()
    {
        Ray[] wallCheckRay = new Ray[4];
        //  �ǔ�����i�[
        RaycastHit[] hit = new RaycastHit[4];
        for (int i = 0; i < wallCheckRay.Length; i++)
        {
            //�������쐬
            wallCheckRay[i] = new Ray(transform.position + Vector3.up * wallCheckoffsetArray[i], transform.forward);
            //������΂��Ĕ�΂�����œ����������ǂ������m�F
            hitWallFlagArray[i] = Physics.Raycast(wallCheckRay[i], out hit[i], wallCheckDistanceArray[i]);
            //�����̉���
            Debug.DrawRay(wallCheckRay[i].origin, wallCheckRay[i].direction * wallCheckDistanceArray[i], Color.red);
        }

        //���C�L���X�g�œ������Ă�����̂��Ȃ������烊�^�[��
        for (int i = 0; i < hit.Length; i++)
        {
            //�����������Ă��Ȃ����̏���
            if (hit[i].collider == null) { continue; }
            //�����������̂���������̃I�u�W�F�N�g��������
            if (HitObjectCheck(hit[i].collider.gameObject.tag))
            {
                InitilaizeWallHitFlag();
                return false;
            }
        }
        return true;
    }
    private string[] hitObjectTagList = new string[]
    {
        "Enemy",
        "Furniture",
        "Damage",
        "SearchArea",
        "Decoration"
    };
    private bool HitObjectCheck(string tag)
    {
        foreach(string t in hitObjectTagList)
        {
            if(t == tag)
            {
                return true;
            }
        }
        return false;
    }
    private void GrabCheck()
    {
        if (!grabFlag) { return; }
        if (!hitWallFlagArray[(int)RayTag.Bottom] || !hitWallFlagArray[(int)RayTag.Middle] ||
            hitWallFlagArray[(int)RayTag.Up] || hitWallFlagArray[(int)RayTag.Upper])
        {
            grabFlag = false;
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Fall);
        }
    }
    private void InitializeObstacleFlag()
    {
        bool state = controller.CharacterStatus.CurrentState != CharacterTagList.StateTag.ClimbWall && controller.CharacterStatus.CurrentState != CharacterTagList.StateTag.Grab &&
            controller.CharacterStatus.CurrentState != CharacterTagList.StateTag.WallJump;
        bool allNoWallHitFlag = !hitWallFlagArray[(int)RayTag.Bottom] && !hitWallFlagArray[(int)RayTag.Middle] &&
            !hitWallFlagArray[(int)RayTag.Up] && !hitWallFlagArray[(int)RayTag.Upper];
        if (!allNoWallHitFlag || !state) { return; }
        stepJumpFlag = false;
        noGarbToClimbFlag = false;
        wallJumpFlag = false;
        grabFlag = false;
        climbFlag = false;
        controller.CharacterRB.useGravity = true;
    }
    private void FootFallJumpCheck()
    {
        //�v���C���[�̑O�ɒi�������邩���m�F
        Ray stepCheckRay = new Ray(transform.position + (transform.forward * stepCheckOffset) + (transform.up * stepUpCheckOffset), -transform.up);
        lowStep = Physics.Raycast(stepCheckRay, stepCheckDistance);
        Debug.DrawRay(stepCheckRay.origin, stepCheckRay.direction * stepCheckDistance, Color.white);
        for (int i = 1; i < hitWallFlagArray.Length; i++)
        {
            if (hitWallFlagArray[i])
            {
                lowStep = true;
                break;
            }
        }
    }
    private void SaveResetLandingPosition()
    {
        savePosition = false;
        //�v���C���[�̑O�ɒi�������邩���m�F
        Ray saveCheckRay = new Ray(transform.position + (CreateRayAdvanceDirection() * 1.5f) + (transform.up * stepUpCheckOffset), -transform.up);
        savePosition = Physics.Raycast(saveCheckRay, stepCheckDistance);
        Debug.DrawRay(saveCheckRay.origin, saveCheckRay.direction * stepCheckDistance, Color.white);
    }
    public bool WallHitFlagCheck()
    {
        int hitcount = 0;
        for (int i = 1; i < hitWallFlagArray.Length; i++)
        {
            if (hitWallFlagArray[i])
            {
                hitcount++;
            }
        }
        if (hitcount > 0)
        {
            return true;
        }
        return false;
    }
    private void InitilaizeWallHitFlag()
    {
        for (int i = 0; i < hitWallFlagArray.Length; i++)
        {
            hitWallFlagArray[i] = false;
        }
    }
    public void Execute()
    {
        if (controller.GetCameraController().IsFPSMode()) { return; }
        LowStepCommand();
        //�i�����щz���鎞�̏���
        StepJumpCommand();
        //�����ǂɌ������Ă��铮��
        WallJumpCommand();
        //�ǂɒ͂܂铮��
        GrabCommand();
        //�ǂ�o�铮��
        Climb();
    }
    private void LowStepCommand()
    {
        //�͂܂��Ă��邩
        if (grabFlag) { return; }
        //�o���Ă��邩
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.ClimbWall:
            case CharacterTagList.StateTag.Grab:
            case CharacterTagList.StateTag.WallJump:
            case CharacterTagList.StateTag.JumpAttack:
                return;
        }
        if (stepJumpFlag) { return; }
        if (climbFlag) { return; }
        if (controller.CharacterStatus.Jumping) { return; }
        if (!lowStep && controller.CharacterStatus.Landing)
        {
            lowJumpCount++;
            if (lowJumpCount > MaxLowJumpCount)
            {
                lowJumpCount = 0;
            }
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Jump);
            controller.JumpForce(controller.GetData().LowStepJumpPower);
            cliffJump = true;
            controller.CharacterStatus.Jumping = true;
            controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Jump);
        }
    }
    /// <summary>
    /// �i���W�����v�̏������s���֐�
    /// </summary>
    private void StepJumpCommand()
    {
        if (!controller.CharacterStatus.Landing) { return; }
        if (controller.CharacterStatus.Jumping) { return; }
        if (stepJumpFlag && controller.CharacterStatus.MoveInput)
        {
            lowJumpCount = 0;
            controller.CharacterRB.velocity = Vector3.zero;
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Jump);
            controller.JumpForce(controller.GetData().WallJumpPower);
            stepJumpFlag = false;
            controller.CharacterStatus.Jumping = true;
            controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Jump);
        }
    }
    /// <summary>
    /// �͂܂�O�̕ǃW�����v���s���֐�
    /// </summary>
    private void WallJumpCommand()
    {

        if (wallJumpFlag && controller.CharacterStatus.MoveInput)
        {
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.WallJump);
            controller.JumpForce(controller.GetData().WallJumpPower);
            grabFlag = true;
            wallJumpFlag = false;
            InitilaizeWallHitFlag();
            controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Jump);
        }
    }
    /// <summary>
    /// �͂܂�̏������s���֐�
    /// </summary>
    private void GrabCommand()
    {
        if(controller.CharacterStatus.CurrentState == CharacterTagList.StateTag.Jump)
        {
            cliffJump = false;
        }
        bool grabCheck = grabFlag&&
                         hitWallFlagArray[(int)RayTag.Bottom] && hitWallFlagArray[(int)RayTag.Middle] &&
                        !hitWallFlagArray[(int)RayTag.Up] && !hitWallFlagArray[(int)RayTag.Upper];
        if (!grabCheck)
        {
            return;
        }
        if (controller.CharacterRB.useGravity)
        {
            controller.CharacterRB.useGravity = false;
            controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Grab);
        }
        controller.CharacterRB.velocity = Vector3.zero;
        if (MoveKeyInput()&& controller.GetCameraController().IsCameraVerticalRotation())
        {
            SetClimbPostion();
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.ClimbWall);
            controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Climb);
        }
        else if (controller.GetKeyInput().Vertical <= -1.0f&& controller.GetCameraController().IsCameraVerticalRotation())
        {
            controller.GetTimer().GetTimerWallActionStop().StartTimer(0.25f);
            stepJumpFlag = false;
            noGarbToClimbFlag = false;
            wallJumpFlag = false;
            grabFlag = false;
            climbFlag = false;
            controller.CharacterRB.useGravity = true;
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Fall);
            grabCancel = true;
        }
        else
        {
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Grab);
        }
    }
    private bool MoveKeyInput()
    {
        return controller.GetKeyInput().Vertical >= 1.0f;
    }
    /// <summary>
    /// �o��ۂ̓o������̒n�_��ێ�����֐�
    /// </summary>
    private void SetClimbPostion()
    {
        //  �J�n�ʒu��ێ�
        climbOldPos = transform.position;
        //  �I���ʒu���Z�o
        climbPos = transform.position + transform.forward * climbForward + Vector3.up * climbUp;
        //  �݂͂�����
        grabFlag = false;
        //  �悶�o������s
        climbFlag = true;
        //���o��t���O������
        noGarbToClimbFlag = false;
    }
    /// <summary>
    /// �o��̓�������������֐�
    /// </summary>
    private void Climb()
    {
        if (noGarbToClimbFlag)
        {
            controller.CharacterRB.useGravity = false;
            controller.CharacterRB.velocity = Vector3.zero;
            controller.CharacterStatus.Velocity = controller.StopMoveVelocity();
            SetClimbPostion();
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.ClimbWall);
            controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Climb);
        }

        if (!climbFlag) { return; }
        //  �悶�o�胂�[�V�����̐i�s�x���擾
        AnimatorStateInfo animInfo = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
        if (!animInfo.IsName("climb")) { return; }
        float f = animInfo.normalizedTime;
        //  ���E�͌㔼�ɂ����đ����ړ�����
        float x = Mathf.Lerp(climbOldPos.x, climbPos.x, Ease(f));
        float z = Mathf.Lerp(climbOldPos.z, climbPos.z, Ease(f));
        //  �㉺�͓��������ňړ�
        float y = Mathf.Lerp(climbOldPos.y, climbPos.y, f);

        //  ���W���X�V
        transform.position = new Vector3(x, y, z);
        controller.CharacterRB.useGravity = false;
        //  �i�s�x��8���𒴂�����悶�o��̏I��
        if (f >= 0.8f)
        {
            wallJumpFlag = false;
            climbFlag = false;
            grabFlag = false;
            lowStep = false;
            controller.CharacterRB.useGravity = true;
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Idle);
        }
    }
    /// <summary>
    /// �ړ���������ɔ�΂��Ă�Ray��1�ł��������Ă镨����������true��Ԃ��֐�
    /// </summary>
    /// <returns></returns>
    public bool CameraForwardWallCheck()
    {
        for (int i = 1;i < cameraForwardWallFlagArray.Length;i++)
        {
            if (cameraForwardWallFlagArray[i])
            {
                return true;
            }
        }
        return false;
    }
    //  �C�[�W���O�֐�
    private float Ease(float x)
    {
        return x * x * x;
    }
}
