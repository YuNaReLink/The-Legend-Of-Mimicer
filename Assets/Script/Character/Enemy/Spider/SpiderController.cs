using UnityEngine;
using UnityEngine.AI;

public class SpiderController : EnemyController
{
    

    protected override void Start()
    {
        base.Start();
    }

    protected override void InitializeAssign()
    {
        base.InitializeAssign();
        navMeshController = new NavMeshController(GetComponent<NavMeshAgent>(), this);
    }

    protected override void Update()
    {
        base.Update();
        if (death) { return; }
        DamageInput();

        if (!foundPlayer)
        {
            LoiterInput();
        }
        else
        {
            FoundNavigateInput();
        }
    }

    private void DamageInput()
    {
        damage.Execute();
    }

    private void LoiterInput()
    {
        if(target != null)
        {
            foundPlayer = true;
        }
        switch (currentState)
        {
            case CharacterTag.StateTag.Idle:
            case CharacterTag.StateTag.Attack:
                IdleInput();
                break;
            case CharacterTag.StateTag.Run:
                WalkInput();
                break;
        }
    }

    private void IdleInput()
    {
        if (timer.GetTimerIdle().IsEnabled()) { return; }
        motion.ChangeMotion(CharacterTag.StateTag.Run);
        navMeshController.SetGoalPosition();
    }
    private void WalkInput()
    {
        bool arrival = navMeshController.Arrival();
        if (arrival)
        {
            motion.ChangeMotion(CharacterTag.StateTag.Idle);
            navMeshController.PositionReset();
            characterRB.velocity = Vector3.zero;
            timer.GetTimerIdle().StartTimer(3f);
        }
    }

    private void FoundNavigateInput()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("attack")) { return; }
        if(target != null)
        {
            if (timer.GetTimerAttackCoolDown().IsEnabled())
            {
                motion.ChangeMotion(CharacterTag.StateTag.Idle);
                navMeshController.PositionReset();
                characterRB.velocity = Vector3.zero;
                return;
            }
            Vector3 sub = transform.position - target.transform.position;
            float dis = sub.magnitude;
            if(dis > 2f)
            {
                motion.ChangeMotion(CharacterTag.StateTag.Run);
                navMeshController.SetTargetPosition();
            }
            else
            {
                AttackInput();
            }
        }
        else
        {
            foundPlayer = false;
        }
    }

    private void AttackInput()
    {
        motion.ChangeMotion(CharacterTag.StateTag.Attack);
        navMeshController.PositionReset();
        timer.GetTimerAttackCoolDown().StartTimer(3f);
    }

    private void FixedUpdate()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;
        switch (tag)
        {
            case "Attack":
                if (timer.GetTimerDamageCoolDown().IsEnabled()) { return; }
                timer.GetTimerDamageCoolDown().StartTimer(1f);
                damage.Attacker = other.gameObject;
                //ToolController data = other.GetComponent<ToolController>();
                //damage.DamageValue = data.GetDamage();
                damage.DamageFlag = true;
                Instantiate(vfxObjects.GetVFXArray()[(int)VFXScriptableObject.VFXTag.Damage], other.transform.position, Quaternion.identity);
                break;
        }
    }

}
