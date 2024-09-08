using UnityEngine;
using CharacterTagList;

public class PlayerMotion : MotionController
{
    private PlayerController controller;
    public PlayerMotion(PlayerController _controller)
    {
        controller = _controller;
    }
    //Parameters�̖��O���擾
    private readonly string dirname         = "Direction";
    private readonly string jumpcountname   = "JumpCount";
    private readonly string threeattackname = "ThreeAttack";
    private readonly string damagename      = "Damage";
    private readonly string pushingname     = "Pushing";

    public override void ChangeMotion(StateTag _state)
    {
        //�A�j���[�^�[����
        Animator anim = controller.GetAnimator();
        //���̓N���X�̑��
        PlayerInput input = controller.GetKeyInput();
        if (ChangeMotionStopCheck(_state,anim, input)) { return; }
        //��Ԃ̐��l����
        int statenumber =       (int)_state;
        int dirnumber =         (int)input.CurrentDirection;
        int jumpCount =         controller.GetObstacleCheck().GetLowJumpCount();
        int threeAttackCount =  (int)controller.TripleAttack; 
        int damageCount =       (int)controller.DamageTag;
        int pushingcount =      (int)controller.PushTag;
        //�ߋ��ƌ��݂̏�Ԃ��L�^
        controller.CharacterStatus.PastState = controller.CharacterStatus.CurrentState;
        controller.CharacterStatus.CurrentState = _state;
        input.PastDirection = input.CurrentDirection;

        //�A�j���[�V�����J�ڂ̐ݒ�
        anim.SetBool(boolname, controller.BattleMode);
        anim.SetInteger(stateName, statenumber);
        anim.SetInteger(dirname, dirnumber);
        anim.SetInteger(jumpcountname, jumpCount);
        anim.SetInteger(threeattackname, threeAttackCount);
        anim.SetInteger(damagename, damageCount);
        anim.SetInteger(pushingname,pushingcount);
    }

    private bool ChangeMotionStopCheck(StateTag _state, Animator anim, PlayerInput input)
    {
        bool animBoolCheck = anim.GetBool(boolname) == controller.BattleMode;
        bool dirCheck = input.CurrentDirection == input.PastDirection;
        ObstacleCheck obstacleCheck = controller.GetObstacleCheck();
        bool wallActionCheck = !obstacleCheck.IsClimbFlag();
        //������ԁ�"BattleMode"�Ɛ퓬���[�h�̏����������Ȃ烊�^�[��
        if (controller.CharacterStatus.CurrentState == _state && dirCheck
            && animBoolCheck&&controller.CharacterStatus.CurrentState != StateTag.Attack&&
            controller.CharacterStatus.CurrentState != StateTag.Push)
        {
            return true; 
        }
        bool damage = controller.CharacterStatus.CurrentState ==
            StateTag.Damage&&
            anim.GetCurrentAnimatorStateInfo(0).IsName("DamageLanding")&&
            !MotionEndCheck();
        bool jumpStop = controller.CharacterStatus.CurrentState == StateTag.Jump&&!controller.CharacterStatus.Landing&&
            _state != StateTag.Grab&& _state != StateTag.ClimbWall && _state != StateTag.JumpAttack;

        bool climbStop = controller.CharacterStatus.CurrentState == StateTag.ClimbWall && !controller.CharacterRB.useGravity;

        bool wallJumpStop = controller.CharacterStatus.CurrentState == StateTag.WallJump && _state != StateTag.Grab&&
            !controller.GetObstacleCheck().IsClimbFlag()&&controller.GetObstacleCheck().WallHitFlagCheck();
        if (jumpStop||climbStop||wallJumpStop)
        {
            return true;
        }
        return false;
    }
    public override bool MotionEndCheck()
    {
        Animator anim = controller.GetAnimator();
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if(animInfo.normalizedTime >= 1.0f) { return true; }
        return false;
    }

    /// <summary>
    /// ���[�V��������ԂɌ��炸�����I�ɕύX����֐�
    /// </summary>
    /// <param name="_state"></param>
    public override void ForcedChangeMotion(StateTag _state)
    {
        //�A�j���[�^�[����
        Animator anim = controller.GetAnimator();
        //���̓N���X�̑��
        PlayerInput input = controller.GetKeyInput();

        if (controller.CharacterStatus.CurrentState == _state) { return; }

        //��Ԃ̐��l����
        int statenumber = (int)_state;
        int dirnumber = (int)input.CurrentDirection;
        int jumpCount = controller.GetObstacleCheck().GetLowJumpCount();
        int threeAttackCount = (int)controller.TripleAttack;
        int damageCount = (int)controller.DamageTag;
        int pushingcount = (int)controller.PushTag;

        //�ߋ��ƌ��݂̏�Ԃ��L�^
        controller.CharacterStatus.PastState = controller.CharacterStatus.CurrentState;
        controller.CharacterStatus.CurrentState = _state;
        input.PastDirection = input.CurrentDirection;

        //�A�j���[�V�����J�ڂ̐ݒ�
        anim.SetBool(boolname, controller.BattleMode);
        anim.SetInteger(stateName, statenumber);
        anim.SetInteger(dirname, dirnumber);
        anim.SetInteger(jumpcountname, jumpCount);
        anim.SetInteger(threeattackname, threeAttackCount);
        anim.SetInteger(damagename, damageCount);
        anim.SetInteger(pushingname, pushingcount);
    }

    public override bool IsEndRollingMotionNameCheck()
    {
        Animator anim = controller.GetAnimator();
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        PlayerMotionName motionName = new PlayerMotionName();
        string[] rollingName = motionName.GetRollingName();
        foreach(string motion in rollingName)
        {
            if (animInfo.IsName(motion)&&animInfo.normalizedTime >=1.0f)
            {
                return true;
            }
        }
        return false;
    }
    public override void Change(AnimationClip clip)
    {
        AllocateMotion("noGuard", clip);
    }
    private void AllocateMotion(string name, AnimationClip clip)
    {
        AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[controller.GetAnimator().layerCount];
        for (int i = 0; i < controller.GetAnimator().layerCount; i++)
        {
            layerInfo[i] = controller.GetAnimator().GetCurrentAnimatorStateInfo(i);
        }

        // (3)  AnimationClip�������ւ��āA�����I�ɃA�b�v�f�[�g
        // �X�e�[�g�����Z�b�g�����
        //�����ɓ��Ă͂܂�Ȃ���Ή������Ȃ�
        PlayerMotionName setmotionnames = new PlayerMotionName();
        foreach (var motionname in setmotionnames.GetGuardEnabledMotionName())
        {
            if (controller.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName(motionname))
            {
                if (controller.GetAnimatorOverrideController()[motionname] == clip) { return; }
                controller.GetAnimatorOverrideController()[name] = clip;
                controller.GetAnimator().Update(0.0f);
            }
        }
    }
    public override void StopMotionCheck()
    {
        Animator anim = controller.GetAnimator();
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        PlayerMotionName motionName = new PlayerMotionName();

        foreach (string motion in motionName.GetMotionName())
        {
            if (animInfo.IsName(motion))
            {
                StopMotionCommand(motion,anim);
                return;
            }
        }
    }
    private void StopMotionCommand(string motion, Animator anim)
    {
        switch (motion)
        {
            case "jumpAttack":
                if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
                {
                    if (!controller.CharacterStatus.Landing)
                    {
                        anim.enabled = false;
                    }
                    else
                    {
                        anim.enabled = true;
                    }
                }
                break;
        }
    }
    //�ێ����Ă郂�[�V���������I����������肷�鏈��
    public override void EndMotionNameCheck()
    {
        Animator anim = controller.GetAnimator();
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if(animInfo.normalizedTime < 1.0f) { return; }
        PlayerMotionName motionName = new PlayerMotionName();

        foreach(string motion in motionName.GetMotionName())
        {
            if (animInfo.IsName(motion))
            {
                EndMotionCommand(motion);
                return;
            }
        }
    }
    //���[�V�������I��������ɂ��鏈��
    private void EndMotionCommand(string motion)
    {
        switch (motion)
        {
            case "getUp":
                controller.CharacterStatus.CurrentState = StateTag.Null;
                break;
            case "attack1":
            case "attack2":
            case "attack3":
                if(controller.CharacterStatus.CurrentState == StateTag.ReadySpinAttack) { return; }
                controller.GetKeyInput().ThreeAttackCount = 0;
                controller.CharacterStatus.CurrentState = StateTag.Null;
                controller.TripleAttack = TripleAttack.Null;
                break;
            case "jumpAttack":
                controller.GetKeyInput().ThreeAttackCount = 0;
                controller.CharacterStatus.CurrentState = StateTag.Null;
                break;
            case "spinAttack":
                controller.GetKeyInput().ThreeAttackCount = 0;
                controller.CharacterStatus.CurrentState = StateTag.Null;
                controller.TripleAttack = TripleAttack.Null;
                break;
            case "storing":
            case "posture":
                controller.CharacterStatus.CurrentState = StateTag.Null;
                break;
            case "DamageLanding":
            case "damageImpact":
                controller.GetKeyInput().ThreeAttackCount = 0;
                controller.TripleAttack = TripleAttack.Null;
                controller.CharacterStatus.CurrentState = StateTag.Null;
                break;
        }
    }
}
