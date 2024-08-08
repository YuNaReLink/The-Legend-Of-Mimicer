using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterTag;

public class SwordEffectController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem slashEffect;

    [SerializeField]
    private CharacterController controller = null;

    public void SetController(CharacterController _controller) {  controller = _controller; }


    private StateTag[] StateArray = new StateTag[]
    {
        StateTag.Attack,
        StateTag.JumpAttack,
        StateTag.SpinAttack,
    };

    public void StopEffect()
    {
        slashEffect.Stop();
    }

    public void UpdateEffect()
    {
        if (controller == null) { return; }

        StateTag state = controller.CurrentState;
        bool play = false;
        foreach (StateTag tag in StateArray)
        {
            if (tag == state)
            {
                play = true;
            }
        }

        if (play)
        {
            if (slashEffect.IsAlive()) { return; }
            slashEffect.Play();
        }
        else
        {
            slashEffect.Stop();
        }
    }
}
