using UnityEngine;

public class Chest : MonoBehaviour
{
    /// <summary>
    /// 蓋のオブジェクトの入れ物
    /// </summary>
    [SerializeField]
    private GameObject              lidObject = null;
    /// <summary>
    /// 初期の角度を保存
    /// </summary>
    [SerializeField]
    private Vector3                 baseRotate = Vector3.zero;
    /// <summary>
    /// 開く角度を保存するもの
    /// </summary>
    [SerializeField]
    private Vector3                 openRotate = Vector3.zero;
    /// <summary>
    /// 宝箱が開く回転角度X
    /// </summary>
    [SerializeField]
    private float                   openRotateX = -100;
    /// <summary>
    /// 宝箱の開くスピード
    /// </summary>
    [SerializeField]
    private float                   openSpeed = 5f;
    /// <summary>
    /// プレイヤーが宝箱周辺の当たり判定に当たっているか調べるためのクラス
    /// </summary>
    [SerializeField]
    private TriggerCheck            triggerCheck = null;
    /// <summary>
    /// 宝箱を開くためのフラグ
    /// </summary>
    [SerializeField]
    private bool                    open = false;
    /// <summary>
    /// 宝箱を開き終わった時のフラグ
    /// </summary>
    [SerializeField]
    private bool                    stop = false;
    /// <summary>
    /// アイテムをプレイヤーに取得させるクラス
    /// </summary>
    [SerializeField]
    private GetChestItem            getItem = null;

    private SoundController         soundController = null;

    private void Awake()
    {
        triggerCheck = GetComponentInChildren<TriggerCheck>();
        if(triggerCheck == null)
        {
            Debug.LogWarning("TriggerCheckがアタッチされていません");
        }
        getItem = GetComponent<GetChestItem>();
        if(getItem == null)
        {
            Debug.LogWarning("GetChestItemがアタッチされていません");
        }
        soundController = GetComponent<SoundController>();
        if(soundController != null)
        {
            soundController.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("ChestSoundControllerがアタッチされていません");
        }
    }
    private void Start()
    {
        open = false;
        stop = false;

        baseRotate = lidObject.transform.localRotation.eulerAngles;
        openRotate = baseRotate;
        openRotate.x = openRotateX;
    }

    private void Update()
    {
        if (!triggerCheck.IsHitPlayer()) { return; }
        if (open)
        {
            triggerCheck.SetTriggerTag(false);
        }
        if (stop) { return; }
        OpenInput();
        OpenCover();
    }

    private void OpenInput()
    {
        if (open) { return; }
        if (!triggerCheck.GetController().GetKeyInput().IsGetButton()) { return; }
        open = true;
        soundController.PlaySESound((int)SoundTagList.ChestSoundTag.Open);
        triggerCheck.SetTriggerTag(false);
        triggerCheck.SetHide(true);
    }

    private void OpenCover()
    {
        if (!open) { return; }
        if(lidObject == null) { return; }
        Vector3 sub = openRotate - baseRotate;
        Vector3 normal = sub.normalized;
        Vector3 addRotate = lidObject.transform.localRotation.eulerAngles;
        addRotate += normal * openSpeed;
        lidObject.transform.localRotation = Quaternion.Euler(addRotate);
        float currentRotate = lidObject.transform.localRotation.eulerAngles.x;
        currentRotate -= 360f;
        if(currentRotate <= openRotateX + 10f)
        {
            Vector3 rotate = Vector3.zero;
            rotate.x = openRotateX;
            lidObject.transform.localRotation = Quaternion.Euler(rotate);
            stop = true;
            //蓋が空き切った時にアイテムクラスでプレイヤーに指定されたアイテムを取得させる
            getItem.Get();
        }
    }
}
