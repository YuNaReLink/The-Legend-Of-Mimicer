using UnityEngine;

/// <summary>
/// �S�Ă̓G���p�����Ă�R���g���[���[
/// �N���A�L�m�R�A�{�X���p�����Ă�
/// </summary>
public class EnemyController : CharacterController
{
    /// <summary>
    /// �G�l�~�[�̃X�N���v�^�u���I�u�W�F�N�g�̃C���X�^���X
    /// </summary>
    [SerializeField]
    protected EnemyScriptableObject     data = null;
    /// <summary>
    /// ��Get�֐�
    /// </summary>
    /// <returns></returns>
    public EnemyScriptableObject        GetData() {  return data; }
    /// <summary>
    /// �v���C���[�𔭌����Ă邩���肷��t���O
    /// </summary>
    [SerializeField]
    protected bool                      foundPlayer = false;
    /// <summary>
    /// ��GetSet�֐�
    /// </summary>
    public bool                         FoundPlayer { get { return foundPlayer; }set { foundPlayer = value; } }
    /// <summary>
    /// ������������PlayerController�̃N���X��ێ�����N���X�̃C���X�^���X�錾
    /// </summary>
    [SerializeField]
    protected PlayerController          target = null;
    /// <summary>
    /// ��GetSet�֐�
    /// </summary>
    public PlayerController             Target { get { return target; } set { target = value; } }
    /// <summary>
    /// �_���[�W�̏������Ǘ�����N���X
    /// </summary>
    protected EnemyDamageCommand        damage = null;
    //��Get�֐�
    public EnemyDamageCommand           GetDamage() { return damage; }
    /// <summary>
    /// �G�̌p����Ŏg���^�C�}�[���܂Ƃ߂��N���X
    /// </summary>
    protected EnemyTimer                timer = null;
    //�� Get�֐�
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
        //�ŏ���3�b�҂�����
        timer.GetTimerIdle().StartTimer(3f);
        //��Ԃ͑ҋ@�ɐݒ�
        characterStatus.CurrentState = CharacterTagList.StateTag.Idle;
        //�X�N���v�^�u���I�u�W�F�N�g������Ȃ�
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
