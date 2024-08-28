using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject movePosition;
    [SerializeField]
    private HitPlayerExecute hitExecute;
    [SerializeField]
    private Collider doorCollider;

    [SerializeField]
    private float playerSpeed = 3f;

    //開閉扉に必要なフラグ
    [SerializeField]
    private bool open = false;
    [SerializeField]
    private bool movePlayer = false;
    [SerializeField]
    private bool close = false;

    [SerializeField]
    private Vector3 baseDoorPos = Vector3.zero;

    [SerializeField]
    private Vector3 moveDoorPos = new Vector3(0, 7, 0);

    [SerializeField]
    private float openSpeed = 10f;

    //錠前オブジェクト
    //[SerializeField]
    //private LookObject lookObject;

    [SerializeField]
    private bool start = false;

    [SerializeField]
    private TriggerCheck triggerCheck = null;
    private void Start()
    {
        if (movePosition != null)
        {
            hitExecute = movePosition.GetComponent<HitPlayerExecute>();
        }

        //lookObject = GetComponentInChildren<LookObject>();

        open = false;
        movePlayer = false;
        close = false;
        start = false;

        baseDoorPos = transform.localPosition;
    }

    private void Update()
    {
        //0番目:キー入力で作動
        if (!open && !movePlayer && !close)
        {
            if (InputManager.GetItemButton()&& triggerCheck.IsHitPlayer())
            {
                start = true;
            }
        }

        //1番目:ドア開閉の初期化
        bool startOpenTheDoor = !open && !movePlayer && !close;
        if (startOpenTheDoor)
        {
            if (start)
            {
                doorCollider.isTrigger = true;
                open = true;
                hitExecute.enabled = false;
                triggerCheck.GetController().StopController = true;
                triggerCheck.GetController().StopMove();
                triggerCheck.GetController().GetMotion().ChangeMotion(CharacterTag.StateTag.Idle);
                start = false;
                return;
            }
        }


        //2番目:ドアを上にあげる
        if (open)
        {
            MoveDoor();
        }

        if (!movePlayer)
        {
            if (hitExecute.PlayerHit)
            {
                Vector3 changePos = movePosition.transform.localPosition;
                changePos.z *= -1;
                movePosition.transform.localPosition = changePos;
                hitExecute.PlayerHit = false;
            }
        }
        else
        {
            //4番目:プレイヤーをその位置に強制的に移動
            Vector3 lookPos = movePosition.transform.position;
            lookPos.y = triggerCheck.GetPlayer().transform.position.y;
            triggerCheck.GetPlayer().transform.LookAt(lookPos);
            triggerCheck.GetPlayer().transform.position = Vector3.MoveTowards(triggerCheck.GetPlayer().transform.position, movePosition.transform.position, playerSpeed * Time.deltaTime);
            triggerCheck.GetController().GetMotion().ChangeMotion(CharacterTag.StateTag.Run);
            //5番目:ドアを閉めるフラグを設定
            if (triggerCheck.GetPlayer().transform.position == movePosition.transform.position)
            {
                movePlayer = false;
                close = true;
            }
        }


        //6番目:ドアを閉め、終わったら位置を修正、その他の設定を行う
        if (close)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, baseDoorPos, openSpeed * Time.deltaTime);
            if (transform.localPosition.y <= baseDoorPos.y)
            {
                doorCollider.isTrigger = false;
                transform.localPosition = new Vector3(0, baseDoorPos.y, 0);
                close = false;
                triggerCheck.GetController().StopController = false;
                hitExecute.enabled = true;
            }
        }
    }

    private void MoveDoor()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, moveDoorPos, openSpeed * Time.deltaTime);
        //3番目:ドアが指定した位置まで上がったらその位置を代入
        Vector3 doorpos = transform.localPosition;
        if (doorpos.y >= moveDoorPos.y)
        {
            transform.localPosition = new Vector3(0, moveDoorPos.y, 0);
            open = false;
            movePlayer = true;
        }
    }
}
