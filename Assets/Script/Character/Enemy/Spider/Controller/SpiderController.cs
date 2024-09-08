using UnityEngine;
using UnityEngine.AI;

public class SpiderController : EnemyController
{
    /// <summary>
    /// クモの状態の入力を管理するクラス
    /// </summary>
    private SpiderState             spiderInput = null;
    public SpiderState              GetSpiderInput() { return spiderInput; }
    /// <summary>
    /// クモの効果音を管理するクラス
    /// </summary>
    private SpiderSoundController   spiderSoundController = null;
    public SpiderSoundController    GetSpiderSoundController() {  return spiderSoundController; }
    /// <summary>
    /// クモのダメージ&死亡時の処理の管理を行うクラス
    /// </summary>
    protected SpiderDamageCommand   spiderDamage = null;

    protected override void SetMotionController()
    {
        motion = new SpiderMotionController(this);
    }
    protected override void InitializeAssign()
    {
        base.InitializeAssign();
        navMeshController = new NavMeshController(GetComponent<NavMeshAgent>(), this);
        spiderInput = GetComponent<SpiderState>();
        if(spiderInput != null)
        {
            spiderInput.SetController(this);
        }
        spiderSoundController = GetComponent<SpiderSoundController>();
        if(spiderSoundController != null)
        {
            spiderSoundController.AwakeInitilaize();
        }
        spiderDamage = new SpiderDamageCommand(this);
    }

    protected override void Update()
    {
        if (Time.timeScale <= 0) { return; }
        spiderSoundController.TimerUpdate();
        base.Update();
        spiderInput.Execute();
        motion.EndMotionNameCheck();
    }

    protected override void MoveStateCheck()
    {
        switch (characterStatus.CurrentState)
        {
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
        if(spiderDamage != null)
        {
            spiderDamage.Execute();
        }
        if(knockBackCommand != null)
        {
            knockBackCommand.Execute();
        }
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
            timer.GetTimerDamageCoolDown().StartTimer(0.25f);
            spiderDamage.Attacker = other.gameObject;
            spiderDamage.DamageFlag = true;
            effectController.CreateVFX((int)EffectTagList.CharacterEffectTag.Damage, other.transform.position,1f, Quaternion.identity);
        }
    }

}
