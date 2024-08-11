using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("カメラが追従するターゲット")]
    [SerializeField]
    private GameObject target;
    [Header("ターゲットがプレイヤーの時にアタッチされるインスタンス宣言")]
    [SerializeField]
    private PlayerController player;

    [Header("カメラとターゲットの初期距離")]
    [SerializeField]
    private float distance_base = 10.0f;
    [SerializeField]
    private float maxDistanceBase = 10.0f;
    [Header("カメラのX移動スピード")]
    [SerializeField]
    private float mouseXSpeed = 3.0f;
    [Header("カメラのY移動スピード")]
    [SerializeField]
    private float mouseYSpeed = 1.5f;

    /// <summary>
    /// カメラの回転量を保持するもの
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
    // カメラとプレイヤーの距離
    [SerializeField]
    private float baseDistance = 2;
    // カメラの高さ
    [SerializeField]
    private float height = 1.0f;
    // カメラの動きの滑らかさ
    [SerializeField]
    private float damping = 10.0f;

    [SerializeField]
    private float neckHeight = 0;

    [SerializeField]
    private float testHor = 0;

    //注目するためのフラグ
    private static bool focusFlag = false;

    public static bool FocusFlag { get { return focusFlag; } set { focusFlag = value; } }

    //注目する座標を保持するもの
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
            // 現在の回転角度を取得
            Vector3 currentRotation = transform.eulerAngles;

            // z軸の回転を0に設定
            currentRotation.z = 0;

            // 回転角度を設定
            transform.eulerAngles = currentRotation;
        }
        else
        {
            //カメラが追従してる対象をチェック
            CheckTarget();

            //カメラとプレイヤーとの距離が1.0fに設定
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
        else if (!focusFlag && player.GetKeyInput().IsCKeyEnabled())
        {
            float playerRotationY = player.transform.rotation.eulerAngles.y;
            float playerRotationX = player.transform.rotation.eulerAngles.x;
            rotation_hor = Mathf.Lerp(rotation_hor, playerRotationY, Time.deltaTime * resetCameraSpeed);
            rotation_ver = Mathf.Lerp(rotation_ver, playerRotationX, Time.deltaTime * resetCameraSpeed);
        }
        else
        {
            //カメラが回転するスピードを設定
            rotation_hor += Input.GetAxis("Mouse X") * mouseXSpeed;
            rotation_ver -= Input.GetAxis("Mouse Y") * mouseYSpeed;
        }

        //restrict vertical angle to -90 ~ +90
        if (Mathf.Abs(rotation_ver) > 90)
            rotation_ver = Mathf.Sign(rotation_ver) * 90;

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
