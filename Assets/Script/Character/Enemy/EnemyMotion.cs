using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotion
{
    [SerializeField]
    private EnemyController controller = null;

    public EnemyMotion(EnemyController _controller)
    {
        controller = _controller;
    }

    public void ChangeMotion(CharacterTag.StateTag tag)
    {
        if (tag == controller.CurrentState) { return; }
        controller.PastState = controller.CurrentState;
        controller.CurrentState = tag;
        controller.GetAnimator().SetInteger("State", (int)tag);
    }
}
