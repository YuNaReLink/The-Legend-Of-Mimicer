using UnityEngine;

public class BossMotionController : MotionController
{
    private BossController controller = null;
    public BossMotionController(BossController _controller)
    {
       controller = _controller;
    }
    /// <summary>
    /// �{�X�̃A�j���[�V������ύX����֐�
    /// </summary>
    /// <param name="tag"></param>
    public override void ChangeMotion(CharacterTagList.StateTag tag)
    {
        //��Ԃ��O�Ɠ����Ȃ瑁�����^�[��
        if (tag == controller.CharacterStatus.CurrentState) { return; }
        //�A�j���[�V�����擾
        Animator anim = controller.GetAnimator();
        //�A�j���[�V�����̑��x��0�ȉ��Ȃ�
        if (anim.speed <= 0)
        {
            //���x��1��
            anim.speed = 1;
        }
        //���݂̏�Ԃ��ߋ���
        controller.CharacterStatus.PastState = controller.CharacterStatus.CurrentState;
        //���݂ɐV������Ԃ�
        controller.CharacterStatus.CurrentState = tag;
        //�V������Ԃ�int�^�ŃA�j���[�V�����ɐݒ�
        anim.SetInteger(stateName, (int)tag);
    }

    private const float maxStampAttackNormalizedTime = 0.7f;

    private const float maxStunNormalizedTime = 0.8f;
    /// <summary>
    /// �ʂ̃A�j���[�V������
    /// �r���ŃA�j���[�V�������~�߂������ɏ�������֐���Boss�o�[�W����
    /// </summary>
    public override void StopMotionCheck()
    {
        //��Ԃ��Ȃ��Ȃ瑁�����^�[��
        if (controller.CharacterStatus.CurrentState == CharacterTagList.StateTag.Null) { return; }
        //�A�j���[�V�����擾
        Animator anim = controller.GetAnimator();
        //���݂̃A�j���[�V�����̏ڍ׎擾
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        //��Ԃɂ���ď�������
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
                //�U�����ŃA�j���[�V�����i�s�x��maxStampAttackNormalizedTime�ȏ�Ȃ�
                if (animInfo.IsName("stampAttack") && animInfo.normalizedTime >= maxStampAttackNormalizedTime)
                {
                    //�^�C�}�[�������Ă�����
                    if (controller.GetTimer().GetTimerAttackCoolDown().IsEnabled())
                    {
                        anim.speed = 0;
                    }
                    //�~�܂��Ă�����
                    else
                    {
                        anim.speed = 1;
                    }
                }
                break;
            case CharacterTagList.StateTag.Damage:
                //���ݏ�ԂŃA�j���[�V�����i�s�x��maxStunNormalizedTime�ȏ�Ȃ�
                if (animInfo.IsName("stun") && animInfo.normalizedTime >= maxStunNormalizedTime)
                {
                    //�^�C�}�[�������Ă�����
                    if (controller.GetTimer().GetTimerStun().IsEnabled())
                    {
                        anim.speed = 0;
                    }
                    //�~�܂��Ă�����
                    else
                    {
                        anim.speed = 1;
                    }
                }
                break;
        }
    }
    /// <summary>
    /// �w�肪����A�j���[�V�����̏I���ɏ������������̂��L�q����֐���Boss�o�[�W����
    /// </summary>
    public override void EndMotionNameCheck()
    {
        //�A�j���[�V�����擾
        Animator anim = controller.GetAnimator();
        //���݂̃A�j���[�V�����̏ڍ׎擾
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        //�A�j���[�V�����i�s�x���I����Ă�����
        if (animInfo.normalizedTime < 1.0f) { return; }
        //�w�肵�Ă�{�X���[�V���������擾
        BossMotionName motionName = new BossMotionName();
        //���݂̃A�j���[�V�������w�肵�Ă郂�[�V�������ƈ�v������̂���������
        foreach (string motion in motionName.GetMotionName())
        {
            if (animInfo.IsName(motion))
            {
                //���[�V�����I�����̏������s��
                EndMotionCommand(motion);
                return;
            }
        }
    }
    /// <summary>
    /// ���[�V�����I�����ɍs�������̏ڍ�
    /// </summary>
    /// <param name="motion"></param>
    public void EndMotionCommand(string motion)
    {
        //�A�j���[�V�����擾
        Animator anim = controller.GetAnimator();
        //�A�j���[�V�����ڍ׎擾
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        switch (motion)
        {
            case "Idle":
                break;
            case "walk":
                break;
            case "stampAttack":
                controller.CharacterStatus.CurrentState = CharacterTagList.StateTag.Null;
                break;
            case "guard":
                CameraController cameraController = controller.Target.GetCameraController();
                if (cameraController == null) { return; }
                if (cameraController.IsFPSMode()) { return; }
                controller.CharacterStatus.CurrentState = CharacterTagList.StateTag.Null;
                break;
            case "stun":
                //���ݏI���ɕ��A�t���O��ON
                controller.RevivalFlag = true;
                break;
            case "returnUp":
                controller.CharacterStatus.CurrentState = CharacterTagList.StateTag.Null;
                break;
        }
    }
}
