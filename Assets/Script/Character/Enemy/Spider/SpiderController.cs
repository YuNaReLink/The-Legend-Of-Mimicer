using UnityEngine;
using UnityEngine.AI;

public class SpiderController : EnemyController
{
    /// <summary>
    /// クモの状態の入力を管理するクラス
    /// </summary>
    private SpiderInput             spiderInput = null;
    public SpiderInput              GetSpiderInput() { return spiderInput; }
    /// <summary>
    /// クモの効果音を管理するクラス
    /// </summary>
    private SpiderSoundController   spiderSoundController = null;
    public SpiderSoundController    GetSpiderSoundController() {  return spiderSoundController; }
    /// <summary>
    /// クモのダメージ&死亡時の処理の管理を行うクラス
    /// </summary>
    protected SpiderDamageCommand   spiderDamage = null;
    protected override void Start()
    {
        base.Start();
    }

    protected override void SetMotionController()
    {
        motion = new SpiderMotionController(this);
    }

    protected override void InitializeAssign()
    {
        base.InitializeAssign();
        navMeshController = new NavMeshController(GetComponent<NavMeshAgent>(), this);
        spiderInput = GetComponent<SpiderInput>();
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

    protected override void UpdateMoveInput()
    {
        switch (currentState)
        {
            case CharacterTagList.StateTag.Idle:
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.Damage:
            case CharacterTagList.StateTag.Die:
                return;
        }
        input = true;
    }

    private void FixedUpdate()
    {
        if (death) { return; }
        if (input)
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
        else
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if(player == null) { return; }
            if (player.LeftAction == null) { return; }
            GameObject shild = other.GetComponentInChildren<ShieldController>().gameObject;
            if(shild == null) { return; }
            knockBackCommand.KnockBackFlag = true;
            knockBackCommand.Attacker = shild;
        }
    }

}
