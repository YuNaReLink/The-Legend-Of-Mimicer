using UnityEngine;

public class CameraController : MonoBehaviour
{
    //メインカメラを取得
    private Camera              myCamera = null;
    [Header("カメラが追従するターゲット")]
    [SerializeField]
    private GameObject          target;
    //プレイヤーのインスタンス宣言
    private PlayerController    player;
    //カメラコントローラーで使う変数
    [SerializeField]
    private CameraStatus        cameraStatus;
    public CameraStatus         CameraStatus => cameraStatus;
    /// <summary>
    /// カメラの回転量を保持するもの
    /// </summary>
    private float               rotation_hor;

    private float               rotation_ver;
    //カメラがどれくらい動いたかを取得するVector3変数
    private Vector3             targettrack;


    [Header("カメラがターゲットに注目した時のカメラの位置を固定する値"),SerializeField]
    private Vector3             focusCameraPosition = new Vector3(1.5f, 2.0f, 3.0f);

    [Header("カメラを回転させる時にスピードを正規化する変数"),SerializeField]
    private Vector3             cameraRotationNormalizeSpeed = new Vector3(0, 0.2f, -5);

    [SerializeField]
    private float               neckHeight = 2.0f;

    //注目するためのフラグ
    private static bool         focusFlag = false;

    public static bool          FocusFlag { get { return focusFlag; } set { focusFlag = value; } }

    //注目する座標を保持するもの
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
            Debug.LogError("カメラがアタッチされませんでした。");
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
        //カメラが追従してる対象をチェック
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
    /// ゲームの状態によってカメラが追従するターゲットを確認し必要ならターゲットを代入する
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
    /// ゲーム状態時のカメラ処理
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
            // 現在の回転角度を取得
            Vector3 currentRotation = transform.eulerAngles;

            // z軸の回転を0に設定
            currentRotation.z = 0;

            // 回転角度を設定
            transform.eulerAngles = currentRotation;
        }
        if (Mathf.Abs(rotation_hor) >= 360)
        {
            rotation_hor = 0;
        }
    }
    /// <summary>
    /// FPSカメラモード時に
    /// </summary>
    /// <param name="mode"></param>
    void ChangeFpsMode(bool mode)
    {
        if(fpsMode == mode) { return; }
        fpsMode = mode;
    }

    /// <summary>
    /// ゲームオーバー時のカメラとの距離と高さを調整する
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
    /// ゲームクリア時のカメラと対象の距離を設定する関数
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
    /// FPSのカメラの処理を行う関数
    /// </summary>
    private void FPSCamera()
    {
        rotation_hor += InputManager.CameraXInput() * cameraStatus.MouseXSpeed;
        rotation_ver -= InputManager.CameraYInput() * cameraStatus.MouseYSpeed;

        MoveCameraPositionAndRotatetion();
    }
    /// <summary>
    /// 以下はプレイヤーを中心にカメラを制御する処理
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
            // プレイヤーのローカル座標系でのカメラのオフセット
            Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
            Vector3 offset = new Vector3(focusCameraPosition.x, focusCameraPosition.y, -focusCameraPosition.z);
            // プレイヤーの回転に合わせてローカル座標系のオフセットを変換
            Vector3 rotatedOffset = player.transform.rotation * offset;
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
            // ワールド座標系に変換してカメラの位置に適用
            transform.position = Vector3.Lerp(transform.position, player.transform.position + rotatedOffset, Time.deltaTime * 2.0f);
            return;
        }
        else if (!focusFlag && player.GetKeyInput().IsCameraLockEnabled() || NoTPSCameraState())
        {
            ResetCameraAngles(cameraStatus.ResetCameraSpeed);
        }
        else
        {
            //カメラが回転するスピードを設定
            rotation_hor += InputManager.CameraXInput() * cameraStatus.MouseXSpeed;
            rotation_ver -= InputManager.CameraYInput() * cameraStatus.MouseYSpeed;
        }

        MoveCameraPositionAndRotatetion();
    }
    /// <summary>
    /// Stateの状態によってカメラの方向をリセットするか決めるフラグ
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
    /// カメラの方向をプレイヤーをバックに映す位置に線形補間で移動
    /// 引数で移動するスピードを設定
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
    /// カメラのX回転数値とプレイヤーのX回転数値の差が0.1以下かを調べる関数
    /// </summary>
    /// <returns></returns>
    public bool IsCameraVerticalRotation()
    {
        return rotation_ver  - player.transform.rotation.eulerAngles.x < 0.1f;
    }
    /// <summary>
    /// ゲームオーバー時のカメラの動きと回転を行う関数
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
    /// クリアした時にtargetに入ってるオブジェクトを中心にカメラを制御する処理
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
    /// カメラの動きと回転を行う関数
    /// </summary>
    private void MoveCameraPositionAndRotatetion()
    {
        //restrict vertical angle to -90 ~ +90
        if (Mathf.Abs(rotation_ver) > 60)
            rotation_ver = Mathf.Sign(rotation_ver) * 60;

        //base vector to rotate
        var rotation = Vector3.Normalize(cameraRotationNormalizeSpeed); //base(normalized)
        rotation = Quaternion.Euler(rotation_ver, rotation_hor, 0) * rotation; //rotate vector

        //カメラの埋まりを防ぐためにレイヤーを指定する
        RaycastHit hit;
        int layermask = 1 << 3; //1のビットを3レイヤー分(Floor_obstacleがある場所)だけ左シフト
        float distance = cameraStatus.BaseDistance; //copy default(mouseScroll zoom)
        //スフィアレイキャストで埋まり防止
        if (Physics.SphereCast(targettrack + Vector3.up * 1.7f, 0.5f,
        rotation, out hit, distance, layermask))
        {
            distance = hit.distance; //overwrite copy
        }

        //turn self
        transform.rotation = Quaternion.Euler(rotation_ver, rotation_hor, 0); //Quaternion IN!!

        //turn around + zoom
        transform.position = rotation * distance;

        //回転の中心座標のYを調整
        var necklevel = Vector3.up * neckHeight;
        transform.position += necklevel;

        //カメラの移動(Lerpを使って線形補間)
        targettrack = Vector3.Lerp(
            targettrack, target.transform.position, Time.deltaTime * 10);

        transform.position += targettrack;
    }
}
