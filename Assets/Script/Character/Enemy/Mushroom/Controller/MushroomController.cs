using UnityEngine;

public class MushroomController : EnemyController
{
    private MushroomState mushroomState = null;
    public MushroomState GetMushroomState() { return mushroomState; }

    private SoundController soundController = null;
    public SoundController GetSoundController() { return soundController; }

    private MushroomDamageCommand mushroomDamage = null;
    protected override void SetMotionController()
    {
        motion = new MushroomMotionController(this);
    }
    protected override void InitializeAssign()
    {
        base.InitializeAssign();
        mushroomState = GetComponent<MushroomState>();
        if(mushroomState == null)
        {
            Debug.LogError("MushroomStateがアタッチされていません");
        }
        soundController = GetComponent<SoundController>();
        if(soundController == null)
        {
            Debug.LogError("SoundControllerがアタッチされていません");
        }
        soundController?.AwakeInitilaize();

        mushroomState?.SetController(this);
        mushroomDamage = new MushroomDamageCommand(this);
    }
    protected override void Update()
    {
        if (Time.timeScale <= 0) { return; }
        base.Update();
        mushroomState.StateUpdate();
        motion.EndMotionNameCheck();
    }

    private void FixedUpdate()
    {
        MoveStateCheck();
        if (characterStatus.DeathFlag)
        {
            StopMove();
            return; 
        }
        if (!foundPlayer)
        {
            mushroomState?.StateFixedUpdate(Time.deltaTime);
        }
        else
        {
            if (characterStatus.MoveInput)
            {
                Accele();
            }
            else
            {
                StopMove();
            }
            TransformRotate();
        }
        Move();
        mushroomDamage?.Execute();
        knockBackCommand?.Execute();
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

    private void Accele()
    {
        Vector3 vel = characterStatus.Velocity;
        vel = transform.forward * data.Acceleration;
        float currentSpeed = vel.magnitude;
        if (currentSpeed >= data.MaxSpeed)
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
            case CharacterTagList.StateTag.Damage:
                return;
        }
        if (target == null) { return; }
        Vector3 dir = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2.5f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        ToolController tool = other.GetComponent<ToolController>();
        if (tool != null)
        {
            switch (tool.GetToolTag())
            {
                case ToolTag.Shield:
                case ToolTag.Other:
                    return;
            }
            if (timer.GetTimerDamageCoolDown().IsEnabled()) { return; }
            timer.GetTimerDamageCoolDown().StartTimer(0.25f);
            mushroomDamage.Attacker = other.gameObject;
            mushroomDamage.DamageFlag = true;
            effectController.CreateVFX((int)EffectTagList.CharacterEffectTag.Damage, other.transform.position, 1f, Quaternion.identity);
        }
    }
}
