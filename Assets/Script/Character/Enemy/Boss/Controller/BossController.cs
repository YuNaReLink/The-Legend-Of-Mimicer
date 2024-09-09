using UnityEngine;

public class BossController : EnemyController
{
    private BossState               bossState = null;

    private BossDamageCommand       bossDamageCommand = null;
    public BossDamageCommand        GetBossDamageCommand() { return bossDamageCommand; }

    private BossSoundController     bossSoundController = null;
    public BossSoundController      GetBossSoundController() { return bossSoundController; }

    [SerializeField]
    private bool                    stunFlag = false;
    public bool                     StunFlag => stunFlag;
    public void                     SetStunFlag(bool flag) {  stunFlag = flag; }
    [SerializeField]
    private bool                    revivalFlag = false;
    public bool                     RevivalFlag => revivalFlag;
    public void                     SetRevivalFlag(bool flag) {  revivalFlag = flag; }

    private const float BaseDieTimerCount = 5f;

    private const float BaseDieEffectScale = 10f;

    protected override void InitializeAssign()
    {
        base.InitializeAssign();
        bossState = new BossState(this);
        if(bossState != null)
        {
            bossState.Initilaize();
        }
        bossDamageCommand = new BossDamageCommand(this);

        bossSoundController = GetComponent<BossSoundController>();
        if(bossSoundController != null)
        {
            bossSoundController.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("BossSoundControllerがアタッチされていません");
        }
    }

    protected override void SetMotionController()
    {
        motion = new BossMotionController(this);
    }

    protected override void Update()
    {
        if (Time.timeScale <= 0) { return; }
        base.Update();
        //ボスの状態を設定
        bossState.StateInput();
        //特定のモーションを特定の条件で止めたり再生したりするメソッド
        motion.StopMotionCheck();
        //特定のモーション終了時に処理を行うメソッド
        motion.EndMotionNameCheck();
    }

    private void FixedUpdate()
    {
        if (characterStatus.DeathFlag) { return; }
        //状態によって動くか動かないかを設定
        MoveStateCheck();
        UpdateCommand();
    }

    protected override void MoveStateCheck()
    {
        switch (characterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Idle:
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.Gurid:
            case CharacterTagList.StateTag.Damage:
            case CharacterTagList.StateTag.Die:
            case CharacterTagList.StateTag.GetUp:
                return;
        }
        characterStatus.MoveInput = true;
    }

    private void UpdateCommand()
    {
        //ボスはプレイヤーの情報を見つけていたら動くようにしている
        if (TargetStateCheck()) { return; }
        if(bossDamageCommand != null)
        {
            bossDamageCommand.Execute();
        }
        if (characterStatus.MoveInput)
        {
            Accele();
        }
        else
        {
            StopMove();
        }
        //ボスに移動を適用
        Move();
        //ボスオブジェクトに回転を適用
        TransformRotate();
    }

    private bool TargetStateCheck()
    {
        if(target == null) { return true; }
        if(target.CharacterStatus.CurrentState != CharacterTagList.StateTag.Die) { return false; }
        target = null;
        return true;
    }

    private void Accele()
    {
        Vector3 vel = characterStatus.Velocity;
        vel = transform.forward * data.Acceleration;
        float currentSpeed  = vel.magnitude;
        if(currentSpeed >= data.MaxSpeed)
        {
            vel = vel.normalized * data.MaxSpeed;
        }
        characterStatus.Velocity = vel;
    }

    private void TransformRotate()
    {
        switch (characterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.Gurid:
            case CharacterTagList.StateTag.Damage:
            case CharacterTagList.StateTag.GetUp:
                return;
        }
        if(target == null) { return; }
        Vector3 dir = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.5f * Time.deltaTime);
    }

    public override void Death()
    {
        if (GameSceneSystemController.Instance != null)
        {
            GameSceneSystemController.Instance.BossBattleStart = false;
        }
        GameSceneSystemController.Instance.GameClearUpdate(gameObject);
        base.Death();
    }

    protected override float GetDieTimerCount(){ return BaseDieTimerCount;}

    protected override float GetDieEffectScale() { return BaseDieEffectScale; }
}
