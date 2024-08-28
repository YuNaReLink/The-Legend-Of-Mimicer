using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera              myCamera = null;

    [Header("カメラが追従するターゲット")]
    [SerializeField]
    private GameObject          target;

    private PlayerController    player;

    [Header("カメラとターゲットの初期距離")]
    [SerializeField]
    private float               distance_base = 10.0f;
    [SerializeField]
    private float               maxDistanceBase = 10.0f;
    [Header("カメラのX移動スピード")]
    [SerializeField]
    private float               mouseXSpeed = 3.0f;
    [Header("カメラのY移動スピード")]
    [SerializeField]
    private float               mouseYSpeed = 1.5f;

    /// <summary>
    /// カメラの回転量を保持するもの
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
    // カメラとプレイヤーの距離
    [SerializeField]
    private float               baseDistance = 2;
    // カメラの高さ
    [SerializeField]
    private float               height = 1.0f;
    // カメラの動きの滑らかさ
    [SerializeField]
    private float               damping = 10.0f;

    [SerializeField]
    private float               neckHeight = 0;

    [SerializeField]
    private float               testHor = 0;

    //注目するためのフラグ
    private static bool         focusFlag = false;

    public static bool          FocusFlag { get { return focusFlag; } set { focusFlag = value; } }

    //注目する座標を保持するもの
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
            Debug.LogError("カメラがアタッチされませんでした。");
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
        //カメラが追従してる対象をチェック
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
            Vector3 offset = new Vector3(focusCameraPosX, focusCameraPosY, -desiredDistanceBehindPlayer);
            // プレイヤーの回転に合わせてローカル座標系のオフセットを変換
            Vector3 rotatedOffset = player.transform.rotation * offset;
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
            // ワールド座標系に変換してカメラの位置に適用
            transform.position = Vector3.Lerp(transform.position, player.transform.position + rotatedOffset, Time.deltaTime * 2.0f);
            return;
        }
        else if (!focusFlag && player.GetKeyInput().IsCameraLockEnabled() || NoTPSCameraState())
        {
            ResetCameraAngles(resetCameraSpeed);
        }
        else
        {
            //カメラが回転するスピードを設定
            rotation_hor += InputManager.CameraXInput() * mouseXSpeed;
            rotation_ver -= InputManager.CameraYInput() * mouseYSpeed;
        }

        MoveCameraPositionAndRotatetion();
    }
    /// <summary>
    /// Stateの状態によってカメラの方向をリセットするか決めるフラグ
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

        //カメラの埋まりを防ぐためにレイヤーを指定する
        RaycastHit hit;
        int layermask = 1 << 3; //1のビットを3レイヤー分(Floor_obstacleがある場所)だけ左シフト
        float distance = distance_base; //copy default(mouseScroll zoom)
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

    /// <summary>
    /// 以下はクリアした時にtargetに入ってるオブジェクトを中心にカメラを制御する処理
    /// </summary>
    private void TargetCameraUpdate()
    {
        if (target == null) {return;}

        rotation_hor = target.transform.rotation.eulerAngles.y;
        rotation_ver = 60;

        MoveCameraPositionAndRotatetion();
    }
}
