using UnityEngine;

/// <summary>
/// 全ての敵が継承してるコントローラー
/// クモ、キノコ、ボスが継承してる
/// </summary>
public class EnemyController : CharacterController
{
    /// <summary>
    /// エネミーのスクリプタブルオブジェクトのインスタンス
    /// </summary>
    [SerializeField]
    protected EnemyScriptableObject     data = null;
    /// <summary>
    /// のGet関数
    /// </summary>
    /// <returns></returns>
    public EnemyScriptableObject        GetData() {  return data; }
    /// <summary>
    /// プレイヤーを発見してるか判定するフラグ
    /// </summary>
    [SerializeField]
    protected bool                      foundPlayer = false;
    /// <summary>
    /// のGetSet関数
    /// </summary>
    public bool                         FoundPlayer { get { return foundPlayer; }set { foundPlayer = value; } }
    /// <summary>
    /// 発見した時にPlayerControllerのクラスを保持するクラスのインスタンス宣言
    /// </summary>
    [SerializeField]
    protected PlayerController          target = null;
    /// <summary>
    /// のGetSet関数
    /// </summary>
    public PlayerController             Target { get { return target; } set { target = value; } }
    /// <summary>
    /// ダメージの処理を管理するクラス
    /// </summary>
    protected EnemyDamageCommand        damage = null;
    //のGet関数
    public EnemyDamageCommand           GetDamage() { return damage; }
    /// <summary>
    /// 敵の継承先で使うタイマーをまとめたクラス
    /// </summary>
    protected EnemyTimer                timer = null;
    //の Get関数
    public EnemyTimer                   GetTimer() { return timer; }

    protected override void Awake()
    {
        InitializeAssign();
    }

    protected override void InitializeAssign()
    {
        base.InitializeAssign();
        damage = new EnemyDamageCommand(this);
        timer = new EnemyTimer();

        timer.InitializeAssignTimer();
    }
    
    protected override void Start()
    {
        base.Start();
        //最初は3秒待たせる
        timer.GetTimerIdle().StartTimer(3f);
        //状態は待機に設定
        characterStatus.CurrentState = CharacterTagList.StateTag.Idle;
        //スクリプタブルオブジェクトがあるなら
        if(data != null)
        {
            characterStatus.SetMaxHP(data.MaxHP);
            characterStatus.HP = characterStatus.GetMaxHP();
        }
    }


    protected override void SetMotionController()
    {
        motion = new EnemyMotion(this);
    }

    protected override void Update()
    {
        base.Update();
        timer.TimerUpdate();
    }


    public override void Death()
    {
        base.Death();
        motion.ChangeMotion(CharacterTagList.StateTag.Die);
        timer.GetTimerDie().StartTimer(GetDieTimerCount());
        timer.GetTimerDie().OnCompleted += () =>
        {
            CreateDieEffect(GetDieEffectScale());
            Destroy(gameObject);
            if (gameObject == CameraController.LockObject)
            {
                CameraController.LockObject = null;
            }
        };
    }

    protected virtual float GetDieTimerCount() { return 1f; }

    protected virtual float GetDieEffectScale() { return 1f; }

    private void CreateDieEffect(float scale)
    {
        effectController.CreateVFX((int)EffectTagList.CharacterEffectTag.Death, transform.position,scale, Quaternion.identity);
    }
}
