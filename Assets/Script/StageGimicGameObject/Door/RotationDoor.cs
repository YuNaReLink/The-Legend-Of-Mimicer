using UnityEngine;

/// <summary>
/// 横に開く形式の扉の処理を行うクラス
/// </summary>
public class RotationDoor : MonoBehaviour
{
    /// <summary>
    /// 開く扉のオブジェクト
    /// </summary>
    private GameObject          door = null;
    /// <summary>
    /// 扉が開くためのフラグ
    /// </summary>
    [SerializeField]
    private bool                open = false;

    //開くスピードを調整するための変数
    [SerializeField]
    private float               moveSpeed = 2.0f; 
    /// <summary>
    /// 閉まった時の回転角度
    /// </summary>
    private Quaternion          closedRotation;
    /// <summary>
    /// 開いた時の回転角度
    /// </summary>
    private Quaternion          openRotation;
    /// <summary>
    /// 扉を開くためにプレイヤーとの当たり判定を行っているクラス
    /// </summary>
    private TriggerCheck        triggerCheck = null;
    /// <summary>
    /// 扉の効果音の管理を行うクラス
    /// </summary>
    private SoundController     soundController = null;
    /// <summary>
    /// 扉が閉まるまでのカウント
    /// </summary>
    private DeltaTimeCountDown  closeTimer = null;

    private void Awake()
    {
        door = transform.GetChild(0).gameObject;
        triggerCheck = GetComponent<TriggerCheck>();
        soundController = GetComponent<SoundController>();
        closeTimer = new DeltaTimeCountDown();
        
        if (soundController == null)
        {
            Debug.LogError("SoundControllerがアタッチされていません");
        }
        else
        {
            soundController.AwakeInitilaize();
        }
    }
    private void Start()
    {
        StartInitilaize();
    }

    private void StartInitilaize()
    {
        if (door != null)
        {
            closedRotation = door.transform.rotation;
            openRotation = Quaternion.Euler(0, 90, 0) * closedRotation;
        }
    }

    private void Update()
    {
        closeTimer.Update();
        if (closeTimer.IsEnabled()) { return; }
        OpenInput();
        
        MoveRotation();
    }

    private void OpenInput()
    {
        if (triggerCheck.GetController() == null) { return; }
        if (InputManager.GetItemButton()&& !open)
        {
            // Fキーが押されたら開閉を切り替える
            open = true;
            soundController.PlaySESound((int)SoundTagList.OpenDoorSoundTag.Open);
            triggerCheck.SetTriggerTag(false);
        }
    }

    private void MoveRotation()
    {
        if (open)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    private void Open()
    {
        door.transform.rotation = Quaternion.Lerp(door.transform.rotation, openRotation, Time.deltaTime * moveSpeed);
        Vector3 sub = door.transform.rotation.eulerAngles - openRotation.eulerAngles;
        if(sub.magnitude < 0.1f)
        {
            open = false;
            closeTimer.StartTimer(5f);
            closeTimer.OnCompleted += () =>
            {
                if(triggerCheck.GetController() != null)
                {
                    triggerCheck.SetTriggerTag(true);
                }
            };
        }
    }

    private void Close()
    {
        door.transform.rotation = Quaternion.Lerp(door.transform.rotation, closedRotation, Time.deltaTime * moveSpeed);
    }
}
