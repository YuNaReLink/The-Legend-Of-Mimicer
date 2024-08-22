using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    private BossInput bossStateInput = null;
    public BossInput GetBossStateInput() {  return bossStateInput; }

    [SerializeField]
    private bool stunFlag = false;
    public bool StunFlag { get { return stunFlag; } set { stunFlag = value; } }
    [SerializeField]
    private bool revivalFlag = false;
    public bool RevivalFlag { get { return revivalFlag; } set { revivalFlag = value; } }
    protected override void Start()
    {
        base.Start();
    }

    protected override void InitializeAssign()
    {
        base.InitializeAssign();
        bossStateInput = new BossInput(this);
        if(bossStateInput == null)
        {
            Debug.LogError("bossStateInputが生成されていません");
        }
        else
        {
            bossStateInput.Initilaize();
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
        bossStateInput.StateInput();
        //特定のモーションを特定の条件で止めたり再生したりするメソッド
        motion.StopMotionCheck();
        //特定のモーション終了時に処理を行うメソッド
        motion.EndMotionNameCheck();
    }

    private void FixedUpdate()
    {
        if (death) { return; }
        UpdateMoveInput();
        UpdateCommand();
    }

    protected override void UpdateMoveInput()
    {
        switch (currentState)
        {
            case CharacterTag.StateTag.Idle:
            case CharacterTag.StateTag.Attack:
            case CharacterTag.StateTag.Gurid:
            case CharacterTag.StateTag.Damage:
            case CharacterTag.StateTag.Die:
            case CharacterTag.StateTag.GetUp:
                return;
        }
        input = true;
    }

    private void UpdateCommand()
    {
        if (TargetStateCheck()) { return; }
        if(damage != null)
        {
            damage.Execute();
        }
        if (input)
        {
            Accele();
        }
        else
        {
            Decele();
        }
        Move();
        TransformRotate();
    }

    private bool TargetStateCheck()
    {
        if(target == null) { return true; }
        if(target.CurrentState != CharacterTag.StateTag.Die) { return false; }
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
            case CharacterTag.StateTag.Attack:
            case CharacterTag.StateTag.Gurid:
            case CharacterTag.StateTag.Damage:
            case CharacterTag.StateTag.GetUp:
                return;
        }
        if(target == null) { return; }
        Vector3 dir = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.5f * Time.deltaTime);
    }

    public override void Death()
    {
        GameSceneSystemController.GameClearUpdate(this.gameObject);
        base.Death();
    }

    protected override float GetDieTimerCount(){ return 5f;}

    protected override float GetDieEffectScale() { return 10f; }
}
