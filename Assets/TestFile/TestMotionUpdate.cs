using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMotionUpdate : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private string clipName = "idle";

    private AnimatorOverrideController overrideController;

    [SerializeField]
    private AnimationClip clip = null;
    [SerializeField]
    private AnimationClip nullClip = null;

    // Start is called before the first frame update
    void Start()
    {
        // animation
        animator ??= GetComponent<Animator>();

        // (2)  AnimationOverrideControllerの生成と割り当て
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Change(clip);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            Change(nullClip);
        }
    }

    public void Change(AnimationClip clip)
    {
        // exchange motion
        AllocateMotion(clipName, clip);
    }

    private void AllocateMotion(string name, AnimationClip clip)
    {
        AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[animator.layerCount];
        for (int i = 0; i < animator.layerCount; i++)
        {
            layerInfo[i] = animator.GetCurrentAnimatorStateInfo(i);
        }

        // (3)  AnimationClipを差し替えて、強制的にアップデート
        // ステートがリセットされる
        overrideController[name] = clip;
        animator.Update(0.0f);

        // ステートを戻す
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.Play(layerInfo[i].fullPathHash, i, layerInfo[i].normalizedTime);
        }
    }
}
