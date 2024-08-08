using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterTag;
using System.ComponentModel;

public class ObstacleCheck : MonoBehaviour
{
    /// <summary>
    /// 崖から落ちる時に行うジャンプの判定を行う変数
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
    /// 壁を登るための判定を行う変数
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
    /// 崖を登るための変数
    /// </summary>
    [SerializeField]
    private float climbForward = 1.5f;

    [SerializeField]
    private float climbUp = 2.35f;

    //段差ジャンプフラグ
    [SerializeField]
    private bool stepJumpFlag = false;
    public bool IsStepJumpFlag() { return stepJumpFlag; }

    [SerializeField]
    private bool noGarbToClimbFlag = false;

    //高い壁を登るジャンプフラグ
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

    //崖をジャンプしたか判定するbool型
    [SerializeField]
    private bool cliffJump = false;

    public bool CliffJumpFlag { get { return cliffJump; } set { cliffJump = value; } }

    /// <summary>
    /// カメラの向きを考慮した壁との当たり判定フラグ
    /// </summary>

    [SerializeField]
    private bool[] cameraForwardWallFlagArray = new bool[4];

    [SerializeField]
    private PlayerController controller;

    public void SetController(PlayerController _controller) { controller = _controller; }

    private void FootFallJumpCheck()
    {
        lowStep = false;
        //プレイヤーの前に段差があるかを確認
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
        //壁があるかチェック、なかったら早期リターン
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
            //段差のチェック
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
            //プレイヤーの身長と同じ壁かチェック
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
            //プレイヤーよりも少し高い壁かチェック
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
            //プレイヤーが落下中に崖に当たった時のチェック
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
            //光線を作成
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
        //  壁判定を格納
        RaycastHit[] hit = new RaycastHit[4];
        for(int i = 0; i < wallCheckRay.Length; i++)
        {
            //光線を作成
            wallCheckRay[i] = new Ray(transform.position + Vector3.up * wallCheckoffsetArray[i], transform.forward);
            //光線飛ばして飛ばした先で当たったかどうかを確認
            hitWallFlagArray[i] = Physics.Raycast(wallCheckRay[i], out hit[i], wallCheckDistanceArray[i]);
            //光線の可視化
            Debug.DrawRay(wallCheckRay[i].origin, wallCheckRay[i].direction * wallCheckDistanceArray[i], Color.red);
        }
        
        //レイキャストで当たっているものがなかったらリターン
        for (int i = 0; i < hit.Length; i++)
        {
            //何も当たっていない時の条件
            if (hit[i].collider == null) { continue; }
            //当たったものがもし敵だったら
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
        //段差を飛び越える時の処理
        StepJumpCommand();
        //高い壁に向かってする動作
        WallJumpCommand();
        //壁に掴まる動作
        GrabCommand();
        //壁を登る動作
        Climb();
    }

    private void LowStepCommand()
    {
        
        //掴まっているか
        if (grabFlag) { return; }
        //登っているか
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
        //  開始位置を保持
        climbOldPos = transform.position;
        //  終了位置を算出
        climbPos = transform.position + transform.forward * climbForward + Vector3.up * climbUp;
        //  掴みを解除
        grabFlag = false;
        //  よじ登りを実行
        climbFlag = true;
        //直登りフラグを解除
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
        //  よじ登りモーションの進行度を取得
        AnimatorStateInfo animInfo = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
        if (!animInfo.IsName("climb")) { return; }
        float f = animInfo.normalizedTime;
        //  左右は後半にかけて早く移動する
        float x = Mathf.Lerp(climbOldPos.x, climbPos.x, Ease(f));
        float z = Mathf.Lerp(climbOldPos.z, climbPos.z, Ease(f));
        //  上下は等速直線で移動
        float y = Mathf.Lerp(climbOldPos.y, climbPos.y, f);

        //  座標を更新
        transform.position = new Vector3(x, y, z);
        controller.CharacterRB.useGravity = false;
        //  進行度が8割を超えたらよじ登りの終了
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
    //  イージング関数
    private float Ease(float x)
    {
        return x * x * x;
    }
}
