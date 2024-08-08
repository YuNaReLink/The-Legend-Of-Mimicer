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

        // (2)  AnimationOverrideController�̐����Ɗ��蓖��
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

        // (3)  AnimationClip�������ւ��āA�����I�ɃA�b�v�f�[�g
        // �X�e�[�g�����Z�b�g�����
        overrideController[name] = clip;
        animator.Update(0.0f);

        // �X�e�[�g��߂�
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.Play(layerInfo[i].fullPathHash, i, layerInfo[i].normalizedTime);
        }
    }
}
