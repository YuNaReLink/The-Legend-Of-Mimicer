using UnityEngine;

public class BossController : EnemyController
{
    private BossInput               bossStateInput = null;

    private BossDamageCommand       bossDamageCommand = null;
    public BossDamageCommand        GetBossDamageCommand() { return bossDamageCommand; }

    private BossSoundController     bossSoundController = null;
    public BossSoundController      GetBossSoundController() { return bossSoundController; }

    [SerializeField]
    private bool                    stunFlag = false;
    public bool                     StunFlag { get { return stunFlag; } set { stunFlag = value; } }
    [SerializeField]
    private bool                    revivalFlag = false;
    public bool                     RevivalFlag { get { return revivalFlag; } set { revivalFlag = value; } }

    private const float baseDieTimerCount = 5f;

    private const float baseDieEffectScale = 10f;
    protected override void Start()
    {
        base.Start();
    }

    protected override void InitializeAssign()
    {
        base.InitializeAssign();
        bossStateInput = new BossInput(this);
        if(bossStateInput != null)
        {
            bossStateInput.Initilaize();
        }
        bossDamageCommand = new BossDamageCommand(this);

        bossSoundController = GetComponent<BossSoundController>();
        if(bossSoundController != null)
        {
            bossSoundController.AwakeInitilaize();
        }
        else
        {
            Debug.LogError("BossSoundController���A�^�b�`����Ă��܂���");
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
        bossStateInput.StateInput();
        //����̃��[�V���������̏����Ŏ~�߂���Đ������肷�郁�\�b�h
        motion.StopMotionCheck();
        //����̃��[�V�����I�����ɏ������s�����\�b�h
        motion.EndMotionNameCheck();
    }

    private void FixedUpdate()
    {
        if (death) { return; }
        //��Ԃɂ���ē����������Ȃ�����ݒ�
        UpdateMoveInput();
        UpdateCommand();
    }

    protected override void UpdateMoveInput()
    {
        switch (currentState)
        {
            case CharacterTagList.StateTag.Idle:
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.Gurid:
            case CharacterTagList.StateTag.Damage:
            case CharacterTagList.StateTag.Die:
            case CharacterTagList.StateTag.GetUp:
                return;
        }
        input = true;
    }

    private void UpdateCommand()
    {
        //�{�X�̓v���C���[�̏��������Ă����瓮���悤�ɂ��Ă���
        if (TargetStateCheck()) { return; }
        if(bossDamageCommand != null)
        {
            bossDamageCommand.Execute();
        }
        if (input)
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
        TransformRotate();
    }

    private bool TargetStateCheck()
    {
        if(target == null) { return true; }
        if(target.CurrentState != CharacterTagList.StateTag.Die) { return false; }
        target = null;
        return true;
    }

    private void Accele()
    {
        Vector3 vel = velocity;
        vel = transform.forward * data.Acceleration;
        float currentSpeed  = vel.magnitude;
        if(currentSpeed >= data.MaxSpeed)
        {
            vel = vel.normalized * data.MaxSpeed;
        }
        velocity = vel;
    }

    private void TransformRotate()
    {
        switch (currentState)
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

    protected override float GetDieTimerCount(){ return baseDieTimerCount;}

    protected override float GetDieEffectScale() { return baseDieEffectScale; }
}
