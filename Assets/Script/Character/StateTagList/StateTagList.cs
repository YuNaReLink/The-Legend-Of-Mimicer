
/// <summary>
/// Character�N���X���p�������N���X�Ŏg�p����^�O���܂Ƃ߂�namespace
/// </summary>
namespace CharacterTagList
{
    //�L�����N�^�[���ʂŎg���^�O
    public enum StateTag
    {
        Null = -1,
        Idle,
        Run,
        Rolling,
        Jump,
        WallJump,
        Grab,
        ClimbWall,
        Attack,
        JumpAttack,
        SpinAttack,
        ReadySpinAttack,
        Gurid,
        Fall,
        Damage,
        Die,
        ChangeMode,
        Push,
        Pull,
        GetUp,
        DataEnd
    }
    //�v���C���[�Ŏg���ړ������𔻒肷��^�O
    public enum DirectionTag
    {
        Null = -1,
        Up,
        Down,
        Left,
        Right,
        DataEnd
    }
    //�v���C���[�̎O�i�U���𔻒肷��^�O
    public enum TripleAttack
    {
        Null = -1,
        First,
        Second,
        Third,
        DataEnd
    }
    //�v���C���[�̃K�[�h��Ԃ𔻒肷��^�O
    public enum GuardState
    {
        Null = -1,
        Crouch,
        Normal,
        DataEnd
    }
    //�L�����N�^�[�̃_���[�W�𔻕ʂ���^�O
    public enum DamageTag
    {
        Null = -1,
        Fall,
        NormalAttack,
        DataEnd
    }
    //�v���C���[���I�u�W�F�N�g��������Ԃ𔻕ʂ�����^�O
    public enum PushTag
    {
        Null = -1,
        Start,
        Pushing,
        DataEnd
    }
    //�L�����N�^�[�����n���Ă邩���ĂȂ�����PhysicMaterial��ύX���邽�߂ɔ��ʂ���^�O
    public enum PhysicState
    {
        Null = -1,
        Land,
        Jump,
        DataEnd,
    }
}
