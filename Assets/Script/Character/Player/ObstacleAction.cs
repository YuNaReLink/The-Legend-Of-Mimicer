using UnityEngine;

/// <summary>
/// プレイヤーが壁や崖のアクションの判定を行うクラス
/// </summary>
[System.Serializable]
public class ObstacleAction
{
    [SerializeField]
    private PlayerController    controller = null;

    private Transform transform;

    //段差ジャンプのRayの開始位置
    [SerializeField]
    private float               stepCheckOffset = 0.5f;
    //段差ジャンプのRayの上方向の開始位置
    [SerializeField]
    private float               stepUpCheckOffset = 0.4f;
    //段差ジャンプのRayの距離の数値
    [SerializeField]
    private float               stepCheckDistance = 1.0f;
    //崖からジャンプするかを判定するフラグ
    [SerializeField]
    private bool                lowStep = false;
    //崖ジャンプのモーションのカウント
    [SerializeField]
    private byte                lowJumpCount = 0;
    //崖ジャンプのモーションの最大カウント
    private const byte          MaxLowJumpCount = 2;
    public byte                 GetLowJumpCount() { return lowJumpCount; }
    /// <summary>
    /// 着地座標を保存するかしないかを決めるフラグ
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
    /// 壁を登るための判定を行う変数
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
    /// 崖判定のRayの長さの数値を格納した配列
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
    /// 壁との当たり判定を行うフラグを格納した配列
    /// </summary>
    [SerializeField]
    private bool[]              hitWallFlagArray = new bool[4];

    //崖を登る時にプレイヤーを前に進ませるためのスピード変数
    [SerializeField]
    private float               climbForward = 1.5f;
    //上記の上方向へのスピード変数
    [SerializeField]
    private float               climbUp = 2.0f;
    //段差ジャンプフラグ
    [SerializeField]
    private bool                stepJumpFlag = false;
    //掴まり&登りもしていないか判定するフラグ
    [SerializeField]
    private bool                noGarbToClimbFlag = false;
    //高い壁を登るジャンプフラグ
    [SerializeField]
    private bool                wallJumpFlag = false;
    //掴まりを判定するフラグ
    [SerializeField]
    private bool                grabFlag = false;
    //掴まりをキャンセルするフラグ
    [SerializeField]
    private bool                grabCancel = false;
    public bool                 GrabCancel => grabCancel;
    public void                 SetGrabCancel(bool flag) {  grabCancel = flag; }
    //登りを行うためのフラグ
    [SerializeField]
    private bool                climbFlag = false;
    //登る時の開始地点を保持する変数
    [SerializeField]
    private Vector3             climbOldPos = Vector3.zero;
    //登る時のゴール地点を保持する変数
    [SerializeField]
    private Vector3             climbPos = Vector3.zero;
    /// <summary>
    /// カメラの向きを考慮した壁との当たり判定フラグ
    /// </summary>
    [SerializeField]
    private bool[]              cameraForwardWallFlagArray = new bool[4];
    //Playerクラスのインスタンス宣言
    //崖、壁とのアクションを行えないようにするカウントダウンのカウント数値
    private const float         StopWallActionCount = 0.1f;


    public void Setup(PlayerController c)
    {
        controller = c;
        transform = c.transform;
    }

    /// <summary>
    /// 壁、崖のアクションを行うか判定するbool関数
    /// </summary>
    /// <returns></returns>
    private bool IsObstacleAction()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.SpinAttack:
            case CharacterTagList.StateTag.ReadySpinAttack:
            case CharacterTagList.StateTag.JumpAttack:
                lowStep = true;
                return true;
            case CharacterTagList.StateTag.Rolling:
                if (controller.GetKeyInput().CurrentDirection != CharacterTagList.DirectionTag.Up)
                {
                    return true;
                }
                break;
        }
        return false;
    }
    /// <summary>
    /// 壁との当たり判定を行う関数
    /// </summary>
    public void WallCheckInput()
    {
        //FPSカメラモードがONなら
        if (CameraController.Instance.IsFPSMode()) { return; }
        //入力してる方向にRayを飛ばす処理
        MoveDirectionCheck();
        //プレイヤーの状態によって壁、崖との当たり判定を行わないようにするフラグ
        if (IsObstacleAction()) { return; }
        //壁があるかチェック、なかったら早期リターン
        //Rayをプレイヤーキャラクターのローカル座標から見て前に飛ばし何かに当たっているか判定する処理
        bool wallhit = WallCheck();
        //上記の障害物との当たり判定で何にも当たっていなかったらリターン
        if (!wallhit) { return; }
        InitializeObstacleFlag();
        if (grabFlag || climbFlag || grabCancel)
        {
            return; 
        }
        FootFallJumpCheck();
        SaveResetLandingPosition();

        DeltaTimeCountDown timerStopWallAction = controller.GetTimer().GetTimerWallActionStop();
        //壁との当たりチェック
        if (controller.CharacterStatus.Landing)
        {
            if (controller.GetTimer().GetTimerWallActionStop().IsEnabled()) { return; }
            //段差のチェック
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
            //プレイヤーの身長と同じ壁かチェック
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
            //プレイヤーよりも少し高い壁かチェック
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
    /// <summary>
    /// キー入力とカメラの方向からRayの方向を取得する関数
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
    /// キー入力で移動してる方向にRayを飛ばす処理を行う関数
    /// </summary>
    private void MoveDirectionCheck()
    {
        //Rayを飛ばす方向の位置を取得
        Vector3 dir = CreateRayAdvanceDirection();
        //Ray4つ分をforで処理
        Ray[] wallCheckRay = new Ray[4];
        for (int i = 0; i < wallCheckRay.Length; i++)
        {
            //光線を作成
            wallCheckRay[i] = new Ray(transform.position + Vector3.up * wallCheckoffsetArray[i], dir);
            //飛ばした方向に何か当たったか判定
            cameraForwardWallFlagArray[i] = Physics.Raycast(wallCheckRay[i], wallCheckDistanceArray[i]);
            //線を可視化
            Debug.DrawRay(wallCheckRay[i].origin, wallCheckRay[i].direction * wallCheckDistanceArray[i], Color.green);
        }
    }
    /// <summary>
    /// Rayを飛ばして障害物との当たり判定を行う関数
    /// </summary>
    /// <returns></returns>
    private bool WallCheck()
    {
        Ray[] wallCheckRay = new Ray[4];
        //  壁判定を格納
        RaycastHit[] hit = new RaycastHit[4];
        for (int i = 0; i < wallCheckRay.Length; i++)
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
            //当たったものがもし特定のオブジェクトだったら
            if (HitObjectCheck(hit[i].collider.gameObject.tag))
            {
                InitilaizeWallHitFlag();
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 当たった障害物の中で以下のタグなら判定を無視する
    /// </summary>
    private string[] hitObjectTagList = new string[]
    {
        "Enemy",
        "Furniture",
        "Damage",
        "SearchArea",
        "Decoration"
    };
    /// <summary>
    /// 当たった障害物のタグを上のタグリストから探して判定する関数
    /// </summary>
    /// <param name="tag">
    /// 当たった障害物のタグを代入する引数
    /// </param>
    /// <returns></returns>
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
    /// <summary>
    /// 掴まりを条件によって解除するフラグ
    /// </summary>
    private bool GrabCheck()
    {
        if (hitWallFlagArray[(int)RayTag.Bottom] && hitWallFlagArray[(int)RayTag.Middle] &&
           !hitWallFlagArray[(int)RayTag.Up]     &&!hitWallFlagArray[(int)RayTag.Upper])
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 条件によって障害物とのアクションを起こすフラグをfalseにする関数
    /// </summary>
    private void InitializeObstacleFlag()
    {
        bool state = controller.CharacterStatus.CurrentState != CharacterTagList.StateTag.ClimbWall && 
                     controller.CharacterStatus.CurrentState != CharacterTagList.StateTag.Grab &&
                     controller.CharacterStatus.CurrentState != CharacterTagList.StateTag.WallJump;
        
        bool allNoWallHitFlag = !hitWallFlagArray[(int)RayTag.Bottom] && 
                                !hitWallFlagArray[(int)RayTag.Middle] &&
                                !hitWallFlagArray[(int)RayTag.Up] && !hitWallFlagArray[(int)RayTag.Upper];
        //上記の二つの条件に当てはまっていたら障害物とのフラグを全てfalseにする
        if (!allNoWallHitFlag || !state) { return; }
        stepJumpFlag = false;
        noGarbToClimbFlag = false;
        wallJumpFlag = false;
        grabFlag = false;
        climbFlag = false;
        controller.CharacterRB.useGravity = true;
    }
    /// <summary>
    /// プレイヤーが崖から落ちそうになってるかを判定する関数
    /// </summary>
    private void FootFallJumpCheck()
    {
        //プレイヤーの前に段差があるかを確認
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
    /// <summary>
    /// 着地してる間だけステージから落ちても位置をリセットできる座標を取得する関数
    /// </summary>
    private void SaveResetLandingPosition()
    {
        savePosition = false;
        //プレイヤーの前に段差があるかを確認
        Ray saveCheckRay = new Ray(transform.position + (CreateRayAdvanceDirection() * 1.5f) + (transform.up * stepUpCheckOffset), -transform.up);
        savePosition = Physics.Raycast(saveCheckRay, stepCheckDistance);
        Debug.DrawRay(saveCheckRay.origin, saveCheckRay.direction * stepCheckDistance, Color.white);
    }
    /// <summary>
    /// 障害物に当たったことを判定するbool型を初期化する関数
    /// </summary>
    private void InitilaizeWallHitFlag()
    {
        for (int i = 0; i < hitWallFlagArray.Length; i++)
        {
            hitWallFlagArray[i] = false;
        }
    }
    /// <summary>
    /// フラグによってアクションを実行する関数
    /// </summary>
    public void Execute()
    {
        if (IsObstacleAction()) { return; }
        if (CameraController.Instance.IsFPSMode()) { return; }
        //崖からジャンプする処理
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
    /// <summary>
    /// 崖ジャンプの処理を行う関数
    /// </summary>
    private void LowStepCommand()
    {
        //掴まっているか
        if (grabFlag) { return; }
        //登っているか
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
        if (lowStep || !controller.CharacterStatus.Landing) { return; }
        lowJumpCount++;
        if (lowJumpCount > MaxLowJumpCount)
        {
            lowJumpCount = 0;
        }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Jump);
        controller.JumpForce(controller.GetData().LowStepJumpPower);
        controller.CharacterStatus.Jumping = true;
        controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Jump);
    }
    /// <summary>
    /// 段差ジャンプの処理を行う関数
    /// </summary>
    private void StepJumpCommand()
    {
        if (!controller.CharacterStatus.Landing) { return; }
        if (controller.CharacterStatus.Jumping) { return; }
        if (!stepJumpFlag || !controller.CharacterStatus.MoveInput) { return; }
        lowJumpCount = 0;
        controller.CharacterRB.velocity = Vector3.zero;
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Jump);
        controller.JumpForce(controller.GetData().WallJumpPower);
        stepJumpFlag = false;
        controller.CharacterStatus.Jumping = true;
        controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Jump);
    }
    /// <summary>
    /// 掴まり前の壁ジャンプを行う関数
    /// </summary>
    private void WallJumpCommand()
    {
        if (!wallJumpFlag || !controller.CharacterStatus.MoveInput) { return; }
        if (controller.CharacterStatus.Landing)
        {
            controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Jump);
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.WallJump);
            controller.JumpForce(controller.GetData().WallJumpPower);
        }
        if (GrabCheck())
        {
            grabFlag = true;
            wallJumpFlag = false;
            InitilaizeWallHitFlag();
        }
    }

    private const float WallActionStopCount = 0.25f;
    /// <summary>
    /// 掴まりの処理を行う関数
    /// </summary>
    private void GrabCommand()
    {
        if (grabFlag)
        {
            if (controller.CharacterRB.useGravity)
            {
                controller.GetKeyInput().SetVertical(0);
                controller.CharacterRB.useGravity = false;
                controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Grab);
            }
            if (MoveKeyInput()&& CameraController.Instance.IsCameraVerticalRotation())
            {
                SetClimbPostion();
                controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.ClimbWall);
                controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Climb);
            }
            else if (controller.GetKeyInput().Vertical <= -1.0f&&
                     CameraController.Instance.IsCameraVerticalRotation())
            {
                controller.GetTimer().GetTimerWallActionStop().StartTimer(WallActionStopCount);
                grabFlag = false;
                grabCancel = true;
                controller.CharacterRB.useGravity = true;
            }
            else
            {
                controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Grab);
            }
        }
        else if(noGarbToClimbFlag)
        {
            controller.CharacterRB.useGravity = false;
            SetClimbPostion();
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.ClimbWall);
            controller.GetSoundController().PlaySESound((int)SoundTagList.PlayerSoundTag.Climb);
        }
    }
    private bool MoveKeyInput()
    {
        return controller.GetKeyInput().Vertical >= 1.0f;
    }
    /// <summary>
    /// 登る際の登った後の地点を保持する関数
    /// </summary>
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
    /// <summary>
    /// 登りの動きを処理する関数
    /// </summary>
    private void Climb()
    {
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
        //  進行度が8割を超えたらよじ登りの終了
        if (f >= 0.8f)
        {
            wallJumpFlag = false;
            climbFlag = false;
            grabFlag = false;
            controller.CharacterRB.useGravity = true;
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Idle);
        }
    }
    //  イージング関数
    private float Ease(float x)
    {
        return x * x * x;
    }

    /// <summary>
    /// 移動する方向に飛ばしてるRayで1つでも当たってる物があったらtrueを返す関数
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
}
