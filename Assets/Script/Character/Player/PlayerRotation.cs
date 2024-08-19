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
            var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
            var velocity = horizontalRotation * new Vector3(controller.GetKeyInput().Horizontal, 0, controller.GetKeyInput().Vertical).normalized;
            var rotationSpeed = 600 * Time.deltaTime;
            if (velocity.magnitude > 0.5f)
            {
                targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
            }
            return Quaternion.RotateTowards(controller.transform.rotation, targetRotation, rotationSpeed);
            /*
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
             */
        }
    }
}
