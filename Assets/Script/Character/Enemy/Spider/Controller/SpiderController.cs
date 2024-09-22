using UnityEngine;
using UnityEngine.AI;

public class SpiderController : EnemyController
{
    /// <summary>
    /// �N���̏�Ԃ̓��͂��Ǘ�����N���X
    /// </summary>
    private SpiderState             spiderState = null;
    public SpiderState              GetSpiderInput() { return spiderState; }
    /// <summary>
    /// �N���̌��ʉ����Ǘ�����N���X
    /// </summary>
    private SpiderSoundController   spiderSoundController = null;
    public SpiderSoundController    GetSpiderSoundController() {  return spiderSoundController; }
    /// <summary>
    /// �N���̃_���[�W&���S���̏����̊Ǘ����s���N���X
    /// </summary>
    protected SpiderDamageCommand   spiderDamage = null;
    /// <summary>
    /// NavMeshAgent�̏������܂Ƃ߂��N���X
    /// </summary>
    protected NavMeshController navMeshController = null;
    /// <summary>
    /// ��Get�֐�
    /// </summary>
    /// <returns></returns>
    public NavMeshController GetNavMeshController() { return navMeshController; }

    protected override void SetMotionController()
    {
        motion = new SpiderMotionController(this);
    }
    protected override void InitializeAssign()
    {
        base.InitializeAssign();
        spiderState = GetComponent<SpiderState>();
        spiderSoundController = GetComponent<SpiderSoundController>();
        navMeshController = new NavMeshController(GetComponent<NavMeshAgent>(), this);
        spiderDamage = new SpiderDamageCommand(this);
        
        if(spiderState == null)
        {
            Debug.LogError("SpiderState���A�^�b�`����Ă��܂���");
        }
        else
        {
            spiderState.SetController(this);
        }

        if(spiderSoundController == null)
        {
            Debug.LogError("SpiderSoundController���A�^�b�`����Ă��܂���");
        }
        else
        {
            spiderSoundController.AwakeInitilaize();
        }
    }

    protected override void Update()
    {
        if (Time.timeScale <= 0) { return; }
        spiderSoundController.TimerUpdate();
        base.Update();
        spiderState.Execute();
        motion.EndMotionNameCheck();
    }

    protected override void MoveStateCheck()
    {
        switch (characterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Null:
            case CharacterTagList.StateTag.Idle:
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.Damage:
            case CharacterTagList.StateTag.Die:
                return;
        }
        characterStatus.MoveInput = true;
    }

    private void FixedUpdate()
    {
        MoveStateCheck();
        if (characterStatus.DeathFlag) { return; }
        if (characterStatus.MoveInput)
        {
            MoveCommand();
        }
        else
        {
            StopMove();
        }
        spiderDamage?.Execute();

        knockBackCommand?.Execute();
    }

    private void MoveCommand()
    {
        spiderSoundController.FixedPlaySESound((int)SpiderSoundController.SpiderSoundTag.Foot);
    }
    private void OnTriggerEnter(Collider other)
    {
        ToolController tool = other.GetComponent<ToolController>();
        if(tool != null)
        {
            switch (tool.GetToolTag())
            {
                case ToolTag.Shield:
                case ToolTag.Other:
                    return;
            }
            if (timer.GetTimerDamageCoolDown().IsEnabled()) { return; }
            timer.GetTimerDamageCoolDown().StartTimer(damageCoolDownCount);
            spiderDamage.Attacker = other.gameObject;
            spiderDamage.SetDamageFlag(true);
            effectController.CreateVFX((int)EffectTagList.CharacterEffectTag.Damage, other.transform.position,1f, Quaternion.identity);
        }
    }

}
