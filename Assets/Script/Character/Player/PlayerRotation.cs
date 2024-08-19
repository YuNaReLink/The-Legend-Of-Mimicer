using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̉�]���s���N���X
/// </summary>
public class PlayerRotation
{
    private PlayerController controller = null;
    public PlayerRotation(PlayerController _controller)
    {
        controller = _controller;
    }
    /// <summary>
    /// �J�����̉�]�֌W
    /// </summary>
    //�v���C���[�̈ړ���
    private Vector3 cameraVelocity;
    public Vector3 GetCameraVelocity() { return cameraVelocity; }
    //�v���C���[�̐i�s�����Ɍ����N�H�[�^�j�I��
    private Quaternion playerRot;
    //���݂̉�]�e���x
    private float currentAngularVelocity;
    //�ő�̉�]�p���x[deg/s]
    [SerializeField]
    private float maxAngularVelocity = Mathf.Infinity;
    //�i�s�����ɂ����邨���悻�̎���[s]
    private float smoothTime = 0.05f;
    //���݂̌����Ɛi�s�����̊p�x
    private float diffAngle;
    //���݂̉�]����p�x
    private float rotAngle;

    public void MathPlayerPos(Transform transform)
    {
        //���݂̈ʒu
        controller.CurrentPos = transform.position;
        //�ړ��ʌv�Z
        cameraVelocity = controller.CurrentPos - controller.PastPos;
        //y����0
        cameraVelocity.y = 0;
        //�ߋ��̈ʒu���X�V
        controller.PastPos = controller.CurrentPos;
    }

    public Quaternion SelfRotation(PlayerController controller)
    {
        PlayerInput input = controller.GetKeyInput();

        //���g�������Ă�ꍇ�̒��ڏ���
        if (PlayerCameraController.FocusFlag&& input.IsCKeyEnabled()&&
            PlayerCameraController.LockObject != null)
        {
            // �G�̕����x�N�g�����擾
            Vector3 targetObject = PlayerCameraController.LockObject.transform.position;
            targetObject.y = controller.transform.position.y;
            Vector3 enemyDirection = (targetObject - controller.transform.position).normalized;
            // �v���C���[����ɓG�̕���������
            Quaternion enemyRotation = Quaternion.LookRotation(enemyDirection, Vector3.up);
            return enemyRotation;
        }
        //�����ɓ��Ă͂܂��Ă�����v���C���[�̌�����
        //�J�����̕����ɂ���Ď擾���Ȃ��悤�ɂ���
        else if (input.IsCKeyEnabled())
        {
            Vector3 forwardPos = new Vector3(0,0,2.5f);
            //�O���������擾
            Vector3 resetfocusObjectDirection = ((controller.transform.position + controller.transform.forward) - controller.transform.position).normalized;
            resetfocusObjectDirection.y = 0;
            // �v���C���[����ɑO������������
            Quaternion resetcameraRotation = Quaternion.LookRotation(resetfocusObjectDirection, Vector3.up);
            return resetcameraRotation;
        }
        else
        {
            //��������Ȃ���Βʏ�̎O�l�̃J��������
            playerRot = Quaternion.LookRotation(cameraVelocity, Vector3.up);
            diffAngle = Vector3.Angle(controller.transform.forward, cameraVelocity);
            //��]���x���v�Z����
            float targetRotationSpeed = Mathf.Min(diffAngle / smoothTime, maxAngularVelocity);
            // ��]���x�𒲐�����
            currentAngularVelocity = Mathf.MoveTowards(currentAngularVelocity, targetRotationSpeed, maxAngularVelocity * Time.deltaTime);
            // ��]��K�p����
            rotAngle = currentAngularVelocity * Time.deltaTime;
            return Quaternion.RotateTowards(controller.transform.rotation, playerRot, rotAngle);
        }
    }
}
