using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class SpiderInput : MonoBehaviour
{
    private SpiderController controller = null;
    public void SetController(SpiderController _controller)
    {
        controller = _controller;
    }
    public void Execute()
    {
        if (controller.DeathFlag) { return; }
        if (!controller.FoundPlayer)
        {
            Loiter();
        }
        else
        {
            FoundNavigateInput();
        }
    }

    private void Loiter()
    {
        if (controller.Target != null)
        {
            controller.FoundPlayer = true;
        }
        switch (controller.CurrentState)
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
        if (controller.GetTimer().GetTimerIdle().IsEnabled()) { return; }
        controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Run);
        controller.GetNavMeshController().SetGoalPosition();
    }

    private void WalkInput()
    {
        bool arrival = controller.GetNavMeshController().Arrival();
        if (arrival)
        {
            controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Idle);
            controller.GetNavMeshController().PositionReset();
            controller.CharacterRB.velocity = Vector3.zero;
            controller.GetTimer().GetTimerIdle().StartTimer(3f);
        }
    }

    private void FoundNavigateInput()
    {
        AnimatorStateInfo info = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
        if (info.IsName("attack")) { return; }
        if (controller.Target != null)
        {
            if (controller.GetTimer().GetTimerAttackCoolDown().IsEnabled())
            {
                controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Idle);
                controller.GetNavMeshController().PositionReset();
                controller.CharacterRB.velocity = Vector3.zero;
                return;
            }
            Vector3 sub = transform.position - controller.Target.transform.position;
            float dis = sub.magnitude;
            if (dis > 2f)
            {
                controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Run);
                controller.GetNavMeshController().SetTargetPosition();
            }
            else
            {
                AttackInput();
            }
        }
        else
        {
            controller.FoundPlayer = false;
        }
    }

    private void AttackInput()
    {
        controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Attack);
        controller.GetNavMeshController().PositionReset();
        controller.GetTimer().GetTimerAttackCoolDown().StartTimer(3f);
    }
}
