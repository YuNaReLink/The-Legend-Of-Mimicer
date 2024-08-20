using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̉�]���s���N���X
/// </summary>
public class PlayerRotation
{
    private PlayerController controller = null;
    public PlayerRotation(PlayerController _controller, Quaternion rotation)
    {
        controller = _controller;
        targetRotation = rotation;
    }

    private Quaternion targetRotation = Quaternion.identity;

    public Quaternion SelfRotation(PlayerController controller)
    {
        PlayerInput input = controller.GetKeyInput();

        //���g�������Ă�ꍇ�̒��ڏ���
        if (PlayerCameraController.FocusFlag&& input.IsCameraLockEnabled()&&
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
        else if (input.IsCameraLockEnabled())
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
            var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
            var velocity = horizontalRotation * new Vector3(controller.GetKeyInput().Horizontal, 0, controller.GetKeyInput().Vertical).normalized;
            var rotationSpeed = 600 * Time.deltaTime;
            if (velocity.magnitude > 0.5f)
            {
                targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
            }
            return Quaternion.RotateTowards(controller.transform.rotation, targetRotation, rotationSpeed);
        }
    }
}
