using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterTag;
using System.ComponentModel;

public class ObstacleCheck : MonoBehaviour
{
    /// <summary>
    /// �R���痎���鎞�ɍs���W�����v�̔�����s���ϐ�
    /// </summary>
    [SerializeField]
    private float stepCheckOffset = 0.5f;
    [SerializeField]
    private float stepUpCheckOffset = 0.4f;
    [SerializeField]
    private float stepCheckDistance = 0.7f;


    [SerializeField]
    private bool lowStep = false;
    public bool IsLowStep() { return lowStep; }

    [SerializeField]
    private float lowStepJumpPower = 1250;

    [SerializeField]
    private byte lowJumpCount = 0;
    public byte GetLowJumpCount() { return lowJumpCount; }


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
    private float[] wallCheckoffsetArray = new float[]
    {
        4f,
        2f,
        1f,
        0.25f
    };
    [SerializeField]
    private float[] wallCheckDistanceArray = new float[]
    {
        1f,
        0.7f,
        0.7f,
        0.7f
    };
    [SerializeField]
    private bool[] hitWallFlagArray = new bool[4];


    [SerializeField]
    private float wallJumpPower;

    public void SetWallJumpPower(float _power) { wallJumpPower = _power; }

    /// <summary>
    /// �R��o�邽�߂̕ϐ�
    /// </summary>
    [SerializeField]
    private float climbForward = 1.5f;

    [SerializeField]
    private float climbUp = 2.35f;

    //�i���W�����v�t���O
    [SerializeField]
    private bool stepJumpFlag = false;
    public bool IsStepJumpFlag() { return stepJumpFlag; }

    [SerializeField]
    private bool noGarbToClimbFlag = false;

    //�����ǂ�o��W�����v�t���O
    [SerializeField]
    private bool wallJumpFlag = false;
    public bool IsWallJumpFlag() { return  wallJumpFlag; }

    [SerializeField]
    private bool grabFlag = false;

    public bool IsGrabFlag() { return grabFlag; }

    [SerializeField]
    private bool grabCancel = false;
    public bool GrabCancel {  get { return grabCancel; } set { grabCancel = value; } }


    [SerializeField]
    private bool climbFlag = false;
    public bool IsClimbFlag() {  return climbFlag; }

    [SerializeField]
    private Vector3 climbOldPos = Vector3.zero;
    [SerializeField]
    private Vector3 climbPos = Vector3.zero;

    //�R���W�����v���������肷��bool�^
    [SerializeField]
    private bool cliffJump = false;

    public bool CliffJumpFlag { get { return cliffJump; } set { cliffJump = value; } }

    /// <summary>
    /// �J�����̌������l�������ǂƂ̓����蔻��t���O
    /// </summary>

    [SerializeField]
    private bool[] cameraForwardWallFlagArray = new bool[4];

    [SerializeField]
    private PlayerController controller;

    public void SetController(PlayerController _controller) { controller = _controller; }

    private void FootFallJumpCheck()
    {
        lowStep = false;
        //�v���C���[�̑O�ɒi�������邩���m�F
        Ray stepCheckRay = new Ray(transform.position + (transform.forward * stepCheckOffset) + (transform.up * stepUpCheckOffset), -transform.up);
        lowStep = Physics.Raycast(stepCheckRay, stepCheckDistance);
        Debug.DrawRay(stepCheckRay.origin, stepCheckRay.direction * stepCheckDistance, Color.white);
    }

    private void InitializeFlag()
    {
        bool state = controller.CurrentState != StateTag.ClimbWall && controller.CurrentState != StateTag.Grab &&
            controller.CurrentState != StateTag.WallJump;
        bool initwallhitflag = !hitWallFlagArray[(int)RayTag.Bottom] && !hitWallFlagArray[(int)RayTag.Middle] &&
            !hitWallFlagArray[(int)RayTag.Up] && !hitWallFlagArray[(int)RayTag.Upper];
        if (!initwallhitflag || !state) { return; }
        stepJumpFlag = false;
        noGarbToClimbFlag = false;
        wallJumpFlag = false;
        grabFlag = false;
        climbFlag = false;
        controller.CharacterRB.useGravity = true;
    }

    public void WallCheckInput()
    {
        lowStep = true;
        MoveDirectionCheck();
        //�ǂ����邩�`�F�b�N�A�Ȃ������瑁�����^�[��
        bool wallhit = WallCheck();
        MoveInputCheck();
        if (!wallhit) { return; }
        InitializeFlag();
        if (grabFlag) { return; }
        if (climbFlag) { return; }
        if (grabCancel) { return; }
        FootFallJumpCheck();

        DeltaTimeCountDown timerStopWallAction = controller.GetTimer().GetTimerWallActionStop();

        if (controller.Landing)
        {
            if (controller.GetTimer().GetTimerWallActionStop().IsEnabled()) { return; }
            //�i���̃`�F�b�N
            bool stepCheck =
                hitWallFlagArray[(int)RayTag.Bottom] &&!hitWallFlagArray[(int)RayTag.Up]&&
                !hitWallFlagArray[(int)RayTag.Middle] && !hitWallFlagArray[(int)RayTag.Upper];
            if (stepCheck)
            {
                timerStopWallAction.StartTimer(0.25f);
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
                timerStopWallAction.StartTimer(0.25f);
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
                hitWallFlagArray[(int)RayTag.Up]&&!hitWallFlagArray[(int)RayTag.Upper];
            if (highWallCheck)
            {
                timerStopWallAction.StartTimer(0.25f);
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

    private void MoveInputCheck()
    {
        if (!grabFlag) { return; }
        if(!hitWallFlagArray[(int)RayTag.Bottom] || !hitWallFlagArray[(int)RayTag.Middle] ||
            hitWallFlagArray[(int)RayTag.Up] || hitWallFlagArray[(int)RayTag.Upper])
        {
            grabFlag = false;
        }
    }

    private void MoveDirectionCheck()
    {
        float h = controller.GetKeyInput().Horizontal;
        float v = controller.GetKeyInput().Vertical;
        Vector3 cameraForward = controller.GetCameraDirection(Camera.main.transform.forward);
        Vector3 cameraRight = controller.GetCameraDirection(Camera.main.transform.right);
        Vector3 dir = h * cameraRight + v * cameraForward;
        Ray[] wallCheckRay = new Ray[4];
        for (int i = 0; i < wallCheckRay.Length; i++)
        {
            //�������쐬
            wallCheckRay[i] = new Ray(transform.position + Vector3.up * wallCheckoffsetArray[i], dir);
            cameraForwardWallFlagArray[i] = Physics.Raycast(wallCheckRay[i], 1f);
            Debug.DrawRay(wallCheckRay[i].origin, wallCheckRay[i].direction * 1f, Color.green);
        }
    }

    public bool IsMoveDirectionWallHitFlag()
    {
        for (int i = 0;i<cameraForwardWallFlagArray.Length;i++)
        {
            if (!cameraForwardWallFlagArray[i]) { continue; }
            return true;
        }
        return false;
    }

    private bool WallCheck()
    {
        Ray[] wallCheckRay = new Ray[4];
        //  �ǔ�����i�[
        RaycastHit[] hit = new RaycastHit[4];
        for(int i = 0; i < wallCheckRay.Length; i++)
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
            //�����������̂������G��������
            if (hit[i].collider.gameObject.tag == "Enemy"||
                hit[i].collider.gameObject.tag == "Furniture"||
                hit[i].collider.gameObject.tag == "Damage"||
                hit[i].collider.gameObject.tag == "SearchArea"||
                hit[i].collider.gameObject.tag == "Decoration")
            {
                InitilaizeWallHitFlag();
                return false;
            }
        }
        return true;
    }

    private void InitilaizeWallHitFlag()
    {
        for(int i = 0;i< hitWallFlagArray.Length; i++)
        {
            hitWallFlagArray[i] = false;
        }
    }

    public void Execute()
    {
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
        bool enabledstate = controller.CurrentState == StateTag.ClimbWall || controller.CurrentState == StateTag.Grab ||
            controller.CurrentState == StateTag.WallJump;
        if (enabledstate) { return; }
        if (stepJumpFlag) { return; }
        if (climbFlag) { return; }
        if (controller.Jumping) { return; }
        if (!lowStep && controller.Landing)
        {
            lowJumpCount++;
            if(lowJumpCount > 2)
            {
                lowJumpCount = 0;
            }
            controller.GetKeyInput().GetMotion().ChangeMotion(StateTag.Jump);
            controller.JumpForce(lowStepJumpPower);
            cliffJump = true;
            controller.Jumping = true;
        }
    }

    private void StepJumpCommand()
    {
        if (!controller.Landing) { return; }
        if (controller.Jumping) { return; }
        if (stepJumpFlag && controller.MoveInput)
        {
            lowJumpCount = 0;
            controller.CharacterRB.velocity = Vector3.zero;
            controller.GetKeyInput().GetMotion().ChangeMotion(StateTag.Jump);
            controller.JumpForce(wallJumpPower);
            stepJumpFlag = false;
            controller.Jumping = true;
        }
    }

    private void WallJumpCommand()
    {

        if (wallJumpFlag && controller.MoveInput)
        {
            controller.GetKeyInput().GetMotion().ChangeMotion(StateTag.WallJump);
            controller.JumpForce(wallJumpPower);
            grabFlag = true;
            wallJumpFlag = false;
            InitilaizeWallHitFlag();
        }
    }

    private void GrabCommand()
    {
        bool grabCheck = grabFlag && hitWallFlagArray[(int)RayTag.Bottom] && hitWallFlagArray[(int)RayTag.Middle] &&
            !hitWallFlagArray[(int)RayTag.Up] && !hitWallFlagArray[(int)RayTag.Upper];
        if (grabCheck)
        {
            if (controller.CharacterRB.useGravity)
            {
                controller.CharacterRB.useGravity = false;
                controller.CharacterRB.velocity = Vector3.zero;
                controller.Velocity = controller.StopMoveVelocity();
            }
            controller.Velocity = controller.StopMoveVelocity();
            controller.CharacterRB.velocity = controller.StopRigidBodyVelocity();
            controller.GetKeyInput().GetMotion().ChangeMotion(StateTag.Grab);
            if (MoveKeyInput(controller))
            {
                SetClimbPostion();
                controller.GetKeyInput().GetMotion().ChangeMotion(StateTag.ClimbWall);
            }
            else if (controller.GetKeyInput().IsDownKey())
            {
                controller.GetTimer().GetTimerWallActionStop().StartTimer(0.25f);
                stepJumpFlag = false;
                noGarbToClimbFlag = false;
                wallJumpFlag = false;
                grabFlag = false;
                climbFlag = false;
                controller.CharacterRB.useGravity = true;
                controller.GetKeyInput().GetMotion().ChangeMotion(StateTag.Fall);
                grabCancel = true;
            }
        }
    }

    private bool MoveKeyInput(PlayerController controller)
    {
        if (controller.GetKeyInput().IsUpKey())
        {
            return true;
        }
        return false;
    }

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

    private void Climb()
    {
        if (noGarbToClimbFlag)
        {
            controller.CharacterRB.useGravity = false;
            controller.CharacterRB.velocity = Vector3.zero;
            controller.Velocity = controller.StopMoveVelocity();
            SetClimbPostion();
            controller.GetKeyInput().GetMotion().ChangeMotion(StateTag.ClimbWall);
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
            controller.GetKeyInput().GetMotion().ChangeMotion(StateTag.Idle);
        }
    }
    //  �C�[�W���O�֐�
    private float Ease(float x)
    {
        return x * x * x;
    }
}
