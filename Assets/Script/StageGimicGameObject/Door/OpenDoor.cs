using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    /// <summary>
    /// 扉が開いたとき
    /// プレイヤーが移動するための座標を保持するオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject          movePosition;
    /// <summary>
    /// movePositionオブジェクトを扉の
    /// 反対側に移動させるか判定するクラス
    /// </summary>
    [SerializeField]
    private HitPlayerExecute    hitExecute;
    /// <summary>
    /// ドアのコライダーを保持する変数
    /// </summary>
    [SerializeField]
    private Collider            doorCollider;
    /// <summary>
    /// プレイヤーが自動で移動する時の速度
    /// </summary>
    [SerializeField]
    private float               playerSpeed = 3f;

    /// <summary>
    /// 扉が開くためのフラグ
    /// </summary>
    [SerializeField]
    private bool                open = false;
    /// <summary>
    /// プレイヤーが移動するためのフラグ
    /// </summary>
    [SerializeField]
    private bool                movePlayer = false;
    /// <summary>
    /// 扉が閉まるためのフラグ
    /// </summary>
    [SerializeField]
    private bool                close = false;
    /// <summary>
    /// 最初の扉の位置を保存するVector3
    /// </summary>
    [SerializeField]
    private Vector3             baseDoorPos = Vector3.zero;
    /// <summary>
    /// 扉が動く時の動く座標
    /// </summary>
    [SerializeField]
    private Vector3             moveDoorPos = new Vector3(0, 7, 0);
    /// <summary>
    /// 扉の開閉速度
    /// </summary>
    [SerializeField]
    private float               openSpeed = 10f;
    /// <summary>
    /// 扉が開いてから閉まるまでのフラグ
    /// </summary>
    [SerializeField]
    private bool                start = false;
    /// <summary>
    /// キー入力の判定を行うクラス
    /// </summary>
    [SerializeField]
    private TriggerCheck        triggerCheck = null;
    private void Awake()
    {
        AwakeInitilaize();
    }

    private void AwakeInitilaize()
    {
        if (movePosition == null) { return; }
        hitExecute = movePosition.GetComponent<HitPlayerExecute>();
    }

    private void Start()
    {

        open = false;
        movePlayer = false;
        close = false;
        start = false;

        baseDoorPos = transform.localPosition;
    }

    private void Update()
    {
        //0番目:キー入力で作動
        KeyInput();
        //1番目:ドア開閉の初期化
        InitilaizeOpenDoor();
        //2番目:ドアを上にあげる
        Open();
        //プレイヤーを指定の位置まで動かす
        MovePlayer();
        //6番目:ドアを閉め、終わったら位置を修正、その他の設定を行う
        Close();
    }

    private void KeyInput()
    {
        if (open || movePlayer || close) { return; }
        if (!InputManager.GetItemButton() || !triggerCheck.IsHitPlayer()) { return; }
        start = true;
        triggerCheck.SetTriggerTag(false);
    }

    private void InitilaizeOpenDoor()
    {
        bool startOpenTheDoor = !open && !movePlayer && !close;
        if (!startOpenTheDoor) { return; }
        if (!start) { return; }
        doorCollider.isTrigger = true;
        open = true;
        hitExecute.enabled = false;
        triggerCheck.GetController().CharacterStatus.StopController = true;
        triggerCheck.GetController().StopMove();
        triggerCheck.GetController().GetMotion().ChangeMotion(CharacterTagList.StateTag.Idle);
        start = false;
    }

    private void Open()
    {
        if (!open) { return; }
        MoveDoor();
    }

    private void MoveDoor()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, moveDoorPos, openSpeed * Time.deltaTime);
        //3番目:ドアが指定した位置まで上がったらその位置を代入
        Vector3 doorpos = transform.localPosition;
        if (doorpos.y < moveDoorPos.y) { return; }
        transform.localPosition = new Vector3(0, moveDoorPos.y, 0);
        open = false;
        movePlayer = true;
    }

    private void MovePlayer()
    {
        if (!movePlayer)
        {
            if (!hitExecute.PlayerHit) { return; }
            Vector3 changePos = movePosition.transform.localPosition;
            changePos.z *= -1;
            movePosition.transform.localPosition = changePos;
            hitExecute.PlayerHit = false;
        }
        else
        {
            //4番目:プレイヤーをその位置に強制的に移動
            Vector3 lookPos = movePosition.transform.position;
            lookPos.y = triggerCheck.GetPlayer().transform.position.y;
            triggerCheck.GetPlayer().transform.LookAt(lookPos);
            triggerCheck.GetPlayer().transform.position = Vector3.MoveTowards(triggerCheck.GetPlayer().transform.position, movePosition.transform.position, playerSpeed * Time.deltaTime);
            triggerCheck.GetController().GetMotion().ChangeMotion(CharacterTagList.StateTag.Run);
            //5番目:ドアを閉めるフラグを設定
            if (triggerCheck.GetPlayer().transform.position != movePosition.transform.position) { return; }
            movePlayer = false;
            close = true;
        }
    }

    private void Close()
    {
        if (!close) { return; }
        //扉を指定位置まで動かす
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, baseDoorPos, openSpeed * Time.deltaTime);
        //指定位置まで移動したら
        if (transform.localPosition.y > baseDoorPos.y) { return; }
        //移動を停止
        doorCollider.isTrigger = false;
        transform.localPosition = new Vector3(0, baseDoorPos.y, 0);
        close = false;
        triggerCheck.GetController().CharacterStatus.StopController = false;
        hitExecute.enabled = true;
        triggerCheck.SetTriggerTag(true);
    }
}
