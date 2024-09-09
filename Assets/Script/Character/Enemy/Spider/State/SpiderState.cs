using UnityEngine;

public class SpiderState : MonoBehaviour
{
    private SpiderController controller = null;
    public void SetController(SpiderController _controller)
    {
        controller = _controller;
    }
    public void Execute()
    {
        if (controller.CharacterStatus.DeathFlag) { return; }
        if (!controller.FoundPlayer)
        {
            LoiterMode();
        }
        else
        {
            AttackMode();
        }
    }

    private void LoiterMode()
    {
        if (controller.Target != null)
        {
            controller.FoundPlayer = true;
        }
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Idle:
            case CharacterTagList.StateTag.Attack:
                MoveInput();
                break;
            case CharacterTagList.StateTag.Run:
            case CharacterTagList.StateTag.Null:
                StopInput();
                break;
        }
    }

    private void MoveInput()
    {
        if (controller.GetTimer().GetTimerIdle().IsEnabled()) { return; }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Run);
        controller.GetNavMeshController().SetGoalPosition();
    }

    private void StopInput()
    {
        bool arrival = controller.GetNavMeshController().Arrival();
        if (arrival)
        {
            controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Idle);
            controller.GetNavMeshController().PositionReset();
            controller.CharacterRB.velocity = Vector3.zero;
            controller.GetTimer().GetTimerIdle().StartTimer(3f);
        }
    }

    private void AttackMode()
    {
        if (controller.GetTimer().GetTimerAttackCoolDown().IsEnabled()) { return; }
        if (controller.Target != null)
        {
            Vector3 sub = transform.position - controller.Target.transform.position;
            float dis = sub.magnitude;
            if (dis > 2f)
            {
                controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Run);
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
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Attack);
        controller.GetNavMeshController().PositionReset();
        controller.GetTimer().GetTimerAttackCoolDown().StartTimer(3f);
    }
}
