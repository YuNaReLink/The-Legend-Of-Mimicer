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
            if (tag == state&&MotionTimeCheck(tag))
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

    private bool MotionTimeCheck(StateTag tag)
    {
        AnimatorStateInfo animInfo = controller.GetAnimator().GetCurrentAnimatorStateInfo(0);
        switch (tag)
        {
            case StateTag.Attack:
                if(animInfo.normalizedTime >= 0.3f&& animInfo.normalizedTime < 0.7f) 
                {
                    return true; 
                }
                break;
            case StateTag.JumpAttack:
                if (animInfo.normalizedTime >= 0.3f && animInfo.normalizedTime < 0.5f)
                { 
                    return true; 
                }
                break;
            case StateTag.SpinAttack:
                if (animInfo.normalizedTime < 0.6f) 
                {
                    return true; 
                }
                break;
            default:
                return false;
        }

        return false;
    }

}
