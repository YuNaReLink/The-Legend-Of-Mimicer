using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInput
{
    private BossController controller = null;
    public BossInput(BossController _controller)
    {
        controller = _controller;
    }

    private float maxAttackDistance = 6.0f;

    public void StateInput()
    {
        if(controller.Target == null) { return; }
        PlayerController player = controller.Target;

        Vector3 sub = controller.transform.position - player.transform.position;
        float dis = sub.magnitude;
        if(dis > maxAttackDistance)
        {

        }
        else
        {

        }
    }

}
