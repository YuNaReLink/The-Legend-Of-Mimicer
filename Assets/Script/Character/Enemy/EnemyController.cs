using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : CharacterController
{
    [SerializeField]
    private EnemyScriptableObject data = null;
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

    [SerializeField]
    protected EnemyMotion motion;
    public EnemyMotion GetMotion() { return motion; }

    protected EnemyDamageDetection damage = null;

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

        motion = new EnemyMotion(this);

        damage = new EnemyDamageDetection(this);

        timer = new EnemyTimer();
        timer.InitializeAssignTimer();
        timer.GetTimerIdle().StartTimer(3f);
    }

    protected override void Update()
    {
        base.Update();
        timer.TimerUpdate();
    }
}
