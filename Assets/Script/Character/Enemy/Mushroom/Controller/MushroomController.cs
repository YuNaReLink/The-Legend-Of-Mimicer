using UnityEngine;

/// <summary>
/// キノコモンスターの制御を行うクラス
/// </summary>
public class MushroomController : EnemyController
{
    private MushroomState           mushroomState = null;

    private SoundController         soundController = null;
    public SoundController          GetSoundController() { return soundController; }

    private MushroomDamageCommand   mushroomDamage = null;
    protected override void         SetMotionController()
    {
        motion = new MushroomMotionController(this);
    }
    protected override void InitializeAssign()
    {
        base.InitializeAssign();
        mushroomState = GetComponent<MushroomState>();
        soundController = GetComponent<SoundController>();
        mushroomDamage = new MushroomDamageCommand(this);


        if(mushroomState == null)
        {
            Debug.LogError("MushroomStateがアタッチされていません");
        }
        else
        {
            mushroomState.SetController(this);
        }

        if(soundController == null)
        {
            Debug.LogError("SoundControllerがアタッチされていません");
        }
        else
        {
            soundController.AwakeInitilaize();
        }

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
            TransformRotate(2.5f);
        }
        Move();
        mushroomDamage?.Execute();
        knockBackCommand?.Execute();
    }
    protected override void MoveStateCheck()
    {
        bool stateCheck = characterStatus.CurrentState == CharacterTagList.StateTag.Idle ||
                          characterStatus.CurrentState == CharacterTagList.StateTag.Attack ||
                          characterStatus.CurrentState == CharacterTagList.StateTag.Damage ||
                          characterStatus.CurrentState == CharacterTagList.StateTag.Die;
        if (stateCheck) { return; }
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

    private void OnTriggerEnter(Collider other)
    {
        ToolController tool = other.GetComponent<ToolController>();
        if (tool != null)
        {
            bool toolTagCheck = tool.GetToolTag() == ToolTag.Shield ||
                                tool.GetToolTag() == ToolTag.Other;
            if (toolTagCheck) { return; }
            if (timer.GetTimerDamageCoolDown().IsEnabled()) { return; }
            timer.GetTimerDamageCoolDown().StartTimer(damageCoolDownCount);
            mushroomDamage.Attacker = other.gameObject;
            mushroomDamage.SetDamageFlag(true);
            effectController.CreateVFX((int)EffectTagList.CharacterEffectTag.Damage, other.transform.position, 1f, Quaternion.identity);
        }
    }
}
