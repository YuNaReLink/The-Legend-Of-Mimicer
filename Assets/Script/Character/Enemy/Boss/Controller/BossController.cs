using UnityEngine;

/// <summary>
/// �{�X�̐���N���X
/// </summary>
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

    private const float             BaseDieTimerCount = 5f;

    private const float             BaseDieEffectScale = 10f;

    protected override void InitializeAssign()
    {
        base.InitializeAssign();
        bossSoundController = GetComponent<BossSoundController>();
        bossState = new BossState(this);
        bossDamageCommand = new BossDamageCommand(this);
        
        bossState.Initilaize();

        if(bossSoundController == null)
        {
            Debug.LogError("BossSoundController���A�^�b�`����Ă��܂���");
        }
        else
        {
            bossSoundController.AwakeInitilaize();
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
        //�{�X�̏�Ԃ�ݒ�
        bossState.StateInput();
        //����̃��[�V���������̏����Ŏ~�߂���Đ������肷�郁�\�b�h
        motion.StopMotionCheck();
        //����̃��[�V�����I�����ɏ������s�����\�b�h
        motion.EndMotionNameCheck();
    }

    private void FixedUpdate()
    {
        if (characterStatus.DeathFlag) { return; }
        //��Ԃɂ���ē����������Ȃ�����ݒ�
        MoveStateCheck();
        UpdateCommand();
    }

    protected override void MoveStateCheck()
    {
        bool stateCheck =   characterStatus.CurrentState == CharacterTagList.StateTag.Idle ||
                            characterStatus.CurrentState == CharacterTagList.StateTag.Attack ||
                            characterStatus.CurrentState == CharacterTagList.StateTag.Gurid ||
                            characterStatus.CurrentState == CharacterTagList.StateTag.Damage ||
                            characterStatus.CurrentState == CharacterTagList.StateTag.Die ||
                            characterStatus.CurrentState == CharacterTagList.StateTag.GetUp;
        if (stateCheck) { return; }
        characterStatus.MoveInput = true;
    }

    private void UpdateCommand()
    {
        //�{�X�̓v���C���[�̏��������Ă����瓮���悤�ɂ��Ă���
        if (TargetStateCheck()) { return; }
        bossDamageCommand?.Execute();
        if (characterStatus.MoveInput)
        {
            Accele();
        }
        else
        {
            StopMove();
        }
        //�{�X�Ɉړ���K�p
        Move();
        //�{�X�I�u�W�F�N�g�ɉ�]��K�p
        TransformRotate(0.5f);
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
