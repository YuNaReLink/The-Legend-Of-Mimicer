using UnityEngine;
using UnityEngine.AI;

public class SpiderController : EnemyController
{
    private SpiderInput spiderInput = null;

    private SpiderSoundController spiderSoundController = null;
    public SpiderSoundController GetSpiderSoundController() {  return spiderSoundController; }
    protected SpiderDamageCommand spiderDamage = null;
    public SpiderDamageCommand GetSpiderDamage() { return spiderDamage; }
    protected override void Start()
    {
        base.Start();
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
    }

    protected override void UpdateMoveInput()
    {
        switch (currentState)
        {
            case CharacterTag.StateTag.Idle:
            case CharacterTag.StateTag.Attack:
            case CharacterTag.StateTag.Damage:
            case CharacterTag.StateTag.Die:
                return;
        }
        input = true;
    }

    private void FixedUpdate()
    {
        if (death) { return; }
        MoveCommand();
        if (!input)
        {
            Decele();
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
        if(currentState != CharacterTag.StateTag.Run) { return; }
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
            vfxController.CreateVFX(VFXScriptableObject.VFXTag.Damage, other.transform.position,1f, Quaternion.identity);
        }
        else
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if(player == null) { return; }
            if (player.LeftCommand == null) { return; }
            GameObject shild = other.GetComponentInChildren<ShieldController>().gameObject;
            if(shild == null) { return; }
            knockBackCommand.KnockBackFlag = true;
            knockBackCommand.Attacker = shild;
        }
    }

}
