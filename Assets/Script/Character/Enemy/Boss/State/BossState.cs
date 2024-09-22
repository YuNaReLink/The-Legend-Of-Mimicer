using UnityEngine;

public class BossState
{
    private BossController      controller = null;
    public BossState(BossController _controller)
    {
        controller = _controller;
    }

    /// <summary>
    ///�v���C���[���{�X�ƍő勗�����ꂽ���̊
    /// </summary>
    private const float         MaxMoveDistance = 12.0f;
    /// <summary>
    /// �v���C���[���{�X�ɍŒ���߂Â������ɍU������
    /// </summary>
    private const float         MinAttackDistance = 4.0f;

    public void Initilaize()
    {
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Idle);
    }

    /// <summary>
    /// �v���C���[�Ƃ̋������v��֐�
    /// </summary>
    /// <returns></returns>
    private float MathDistanceAndPlayer()
    {
        PlayerController player = controller.Target;
        Vector3 sub = controller.transform.position - player.transform.position;
        return sub.magnitude;
    }

    public void StateInput()
    {
        if(controller.Target == null) 
        {
            IdleInput();
            return; 
        }
        if (controller.CharacterStatus.DeathFlag) { return; }
        //�ǂ�ȏ󋵂ɂ����炸����������������
        //�{�X���|��鏈��
        controller.GetBossDamageCommand().DoUpdate();
        //�{�X���N���オ�鏈��
        RevivalInput();
        //�{�X������̏������ŃK�[�h���鏈��
        GuardInput();
        if (controller.GetTimer().GetTimerStun().IsEnabled()) { return; }
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Damage:
            case CharacterTagList.StateTag.GetUp:
                return;
        }
        //�v���C���[�Ƃ̋������v��
        float dis = MathDistanceAndPlayer();
        //���������ꂷ���Ă���
        if(dis > MaxMoveDistance)
        {
            //�v���C���[�ɋ߂Â�
            NearsToPlayerInput();
        }
        else
        {
            //�v���C���[�Ɛ퓬
            BattleInput();
        }
    }

    private void NearsToPlayerInput()
    {
        WalkInput();
    }

    private void BattleInput()
    {
        //�������v��
        float dis = MathDistanceAndPlayer();
        //�U�����s���Œ዗�������������߂����
        if (dis < MinAttackDistance)
        {
            //�U������
            AttackInput();
        }
        else
        {
            //��������Ȃ���Εʍs������
            WalkInput();
        }

    }

    /// <summary>
    /// ���L�͓��͕ʊ֐�
    /// �ҋ@
    /// ����
    /// �U��
    /// �h��
    /// ����
    /// </summary>
    private void IdleInput()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.Gurid:
                return;
        }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Idle);
    }

    private void WalkInput()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.Gurid:
                return;
        }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Run);
    }

    private void AttackInput()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Gurid:
            case CharacterTagList.StateTag.Attack:
                return;
        }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Attack);
        controller.GetTimer().GetTimerAttackCoolDown().StartTimer(5f);
    }

    public void GuardInput()
    {
        switch (controller.CharacterStatus.CurrentState)
        {
            case CharacterTagList.StateTag.Attack:
            case CharacterTagList.StateTag.Damage:
            case CharacterTagList.StateTag.GetUp:
                return;
        }
        CameraController cameraController = controller.Target.GetCameraController();
        if(cameraController == null) { return; }
        if (!cameraController.IsFPSMode()) { return; }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Gurid);
    }

    public void RevivalInput()
    {
        if (!controller.RevivalFlag) {  return; }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.GetUp);
        controller.SetRevivalFlag(false);
    }

}
