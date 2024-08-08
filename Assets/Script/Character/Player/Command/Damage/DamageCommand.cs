using CharacterTag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCommand
{
    private PlayerController controller = null;
    public DamageCommand(PlayerController _controller)
    {
        controller = _controller;
    }

    public void Input()
    {
        if(controller.DamageTag == DamageTag.Null) { return; }
        switch (controller.DamageTag)
        {
            case DamageTag.Fall:
                controller.GetKeyInput().GetMotion().ChangeMotion(StateTag.Damage);
                controller.GetFallDistanceCheck().FallDamage = false;
                controller.DamageTag = DamageTag.Null;
                break;
            case DamageTag.NormalAttack:
                controller.GetKeyInput().GetMotion().ChangeMotion(StateTag.Damage);
                controller.DamageTag = DamageTag.Null;
                break;
            case DamageTag.GreatAttack:
                controller.DamageTag = DamageTag.Null;
                break;
        }
    }

    private void DamagePttarn()
    {

    }

    public void Execute()
    {

    }
}
