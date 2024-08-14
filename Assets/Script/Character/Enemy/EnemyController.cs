using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : CharacterController
{
    [SerializeField]
    protected EnemyScriptableObject data = null;
    public EnemyScriptableObject GetData() {  return data; }

    protected NavMeshController navMeshController = null;

    [SerializeField]
    protected bool foundPlayer = false;
    [SerializeField]
    protected PlayerController target = null;
    public PlayerController Target { get { return target; } set { target = value; } }

    /// <summary>
    /// NavMeshAgentÇ≈égÇ§ïœêî
    /// </summary>
    [SerializeField]
    protected Vector3 goalPosition = Vector3.zero;
    public Vector3 GoalPosition { get { return goalPosition; } set { goalPosition = value; } }
    [SerializeField]
    protected float loiterRadius = 10f;
    public float GetLoiterRadius() { return loiterRadius; }

    protected EnemyDamageCommand damage = null;
    public EnemyDamageCommand GetDamage() { return damage; }

    protected EnemyTimer timer = null;
    public EnemyTimer GetTimer() { return timer; }
    protected override void Start()
    {
        base.Start();
        InitializeAssign();

        currentState = CharacterTag.StateTag.Idle;

        if(data != null)
        {
            maxHp = data.MaxHP;
            hp = maxHp;
        }
    }

    protected override void InitializeAssign()
    {
        base.InitializeAssign();

        

        damage = new EnemyDamageCommand(this);

        timer = new EnemyTimer();
        timer.InitializeAssignTimer();
        timer.GetTimerIdle().StartTimer(3f);
    }

    protected override void SetMotionController()
    {
        motion = new EnemyMotion(this);
    }

    protected override void Update()
    {
        if (Time.timeScale <= 0) { return; }
        base.Update();
        timer.TimerUpdate();
    }

    public override void Death()
    {
        base.Death();
        timer.GetTimerDie().StartTimer(1f);
        timer.GetTimerDie().OnCompleted += () =>
        {
            CreateDieEffect(GetDieEffectScale());
            Destroy(gameObject);
            if (gameObject == PlayerCameraController.LockObject)
            {
                PlayerCameraController.LockObject = null;
            }
        };
    }

    protected virtual float GetDieEffectScale() { return 1f; }

    private void CreateDieEffect(float scale)
    {
        vfxController.CreateVFX(VFXScriptableObject.VFXTag.Die, transform.position,scale, Quaternion.identity);
    }
}
