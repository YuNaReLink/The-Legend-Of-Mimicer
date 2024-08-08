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

    /// <summary>
    ///�v���C���[���{�X�ƍő勗�����ꂽ���̊
    /// </summary>
    private float maxMoveDistance = 12.0f;
    /// <summary>
    /// �v���C���[���{�X�ɍŒ���߂Â������ɍU������
    /// </summary>
    private float minAttackDistance = 4.0f;

    public void Initilaize()
    {
        controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Idle);
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
        if(controller.Target == null) { return; }
        //�ǂ�ȏ󋵂ɂ����炸����������������
        StunInput();
        GuardInput();
        if (controller.GetTimer().GetTimerStun().IsEnabled()||controller.CurrentState == CharacterTag.StateTag.Damage) { return; }
        //�v���C���[�Ƃ̋������v��
        float dis = MathDistanceAndPlayer();
        //���������ꂷ���Ă���
        if(dis > maxMoveDistance)
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
        Debug.Log("���ꂷ�������ߐڋߒ�");
    }

    private void BattleInput()
    {
        //�������v��
        float dis = MathDistanceAndPlayer();
        //�U�����s���Œ዗�������������߂����
        if (dis < minAttackDistance)
        {
            //�U������
            AttackInput();
            Debug.Log("�U����");
        }
        else
        {
            //��������Ȃ���Εʍs������
            WalkInput();
            Debug.Log("�v���C���[�ɐڋߒ�");
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
        switch (controller.CurrentState)
        {
            case CharacterTag.StateTag.Attack:
            case CharacterTag.StateTag.Gurid:
                return;
        }
        controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Idle);
    }

    private void WalkInput()
    {
        switch (controller.CurrentState)
        {
            case CharacterTag.StateTag.Attack:
            case CharacterTag.StateTag.Gurid:
                return;
        }
        controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Run);
    }

    private void AttackInput()
    {
        switch (controller.CurrentState)
        {
            case CharacterTag.StateTag.Gurid:
                return;
        }
        controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Attack);
        controller.GetTimer().GetTimerAttackCoolDown().StartTimer(5f);
    }

    public void GuardInput()
    {
        switch (controller.CurrentState)
        {
            case CharacterTag.StateTag.Attack:
            case CharacterTag.StateTag.Damage:
                return;
        }
        PlayerCameraController cameraController = controller.Target.GetCameraScript();
        if(cameraController == null) { return; }
        if (!cameraController.IsFPSMode()) { return; }
        controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Gurid);
    }

    private void StunInput()
    {
        if (!controller.StunFlag) { return; }
        controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Damage);
        controller.GetTimer().GetTimerStun().StartTimer(5f);
        controller.StunFlag = false;
    }

}
