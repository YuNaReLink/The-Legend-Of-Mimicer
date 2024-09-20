using UnityEngine;

/// <summary>
/// クロスボウのアニメーションの処理を行うクラス
/// </summary>
public class CrossBowAnimation : MonoBehaviour
{
    [SerializeField]
    private CrossBowShoot       shoot = null;

    [SerializeField]
    private Animator            animator = null;
    public Animator             GetAnimator() { return animator; }

    private string              shootFlagName = "Fire";

    private void Update()
    {
        if (shoot.Fire)
        {
            ChangeMotion();
        }
        else
        {
            EndFire();
        }
    }

    private void ChangeMotion()
    {
        animator.SetBool(shootFlagName,shoot.Fire);
        shoot.Fire = false;
    }

    private void EndFire()
    {
        AnimatorStateInfo animInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!animInfo.IsName("Shoot")) { return; }
        if(animInfo.normalizedTime < 1.0f) { return; }
        animator.SetBool(shootFlagName, shoot.Fire);
    }
}
