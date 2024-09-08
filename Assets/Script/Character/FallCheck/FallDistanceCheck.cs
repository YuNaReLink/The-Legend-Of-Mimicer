using UnityEngine;

public class FallDistanceCheck : InterfaceBaseCommand
{
    //�Q�Ɨp�v���C���[�R���g���[���[
    private PlayerController    controller = null;
    //�R���X�g���N�^����擾
    public FallDistanceCheck(PlayerController _controller)
    {
        controller = _controller;
    }
    //�@�������ꏊ
    private float               fallenPosition = 0f;
    //�������Ă���n�ʂɗ�����܂ł̋���
    private float               fallenDistance = 0f;
    //�ǂ̂��炢�̍�������_���[�W��^���邩
    private const float         takeDamageDistance = 10f;
    /// <summary>
    /// �����Ă邩�ǂ����̃t���O
    /// </summary>
    private bool                fall = false;
    /// <summary>
    /// �����_���[�W��^���邩�ǂ����̃t���O
    /// </summary>
    private bool                fallDamage = false;
    public bool                 FallDamage {  get { return fallDamage; } set { fallDamage = value; } }

    //������
    public void Initialize()
    {
        fallenDistance = 0f;
        fallenPosition = controller.transform.position.y;
        fall = false;
        fallDamage = false;
    }

    public void Execute()
    {
        if (!controller.CharacterStatus.Landing)
        {
            // �����n�_�ƌ��ݒn�̋������v�Z�i�W�����v���ŏ�ɔ��ŗ��������ꍇ���l������ׂ̏����j
			fallenPosition = Mathf.Max(fallenPosition, controller.transform.position.y);
        }
        else
        {
            fallenPosition = controller.transform.position.y;
            fallenDistance = 0;
        }
    }

    public void CollisionEnter()
    {
        //�@�����������v�Z
        fallenDistance = fallenPosition - controller.transform.position.y;

        //�@�����ɂ��_���[�W���������鋗���𒴂���ꍇ�_���[�W��^����
        if (fallenDistance >= takeDamageDistance)
        {
            fallDamage = true;
        }
    }

    public void CollisionExit()
    {
        //�@�ŏ��̗����n�_��ݒ�
        fallenPosition = controller.transform.position.y;
        fallenDistance = 0;
    }
}
