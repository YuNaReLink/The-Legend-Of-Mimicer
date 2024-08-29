using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLockController : MonoBehaviour
{
    private PlayerConnectionUI playerConnectionUI = null;

    [SerializeField]
    private GameObject lockOnUI = null;


    public void Initialize(PlayerConnectionUI _playerConnectionUI)
    {
        playerConnectionUI = _playerConnectionUI;
    }

    private bool LockOnFlag()
    {
        return CameraController.FocusFlag &&
            CameraController.LockObject != null &&
            playerConnectionUI.GetPlayerController().GetKeyInput().IsCameraLockEnabled();
    }

    public void LockUIActiveCheck()
    {
        Camera camera = Camera.main;
        bool lockOn = LockOnFlag();
        if(lockOn)
        {
            ActiveLockUI(true);
            // 3D�I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ɕϊ�
            Vector3 screenPosition = camera.WorldToScreenPoint(CameraController.LockObject.transform.position);

            // Canvas��RectTransform���擾
            Canvas canvas = playerConnectionUI.GetGameUIController().GetComponent<Canvas>();
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            // �X�N���[�����W���L�����o�X�̃��[�J�����W�ɕϊ�
            Vector2 uiPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, null, out uiPosition);

            // UI�I�u�W�F�N�g�̈ʒu��ݒ�
            RectTransform rectTransform = lockOnUI.GetComponent<RectTransform>();
            rectTransform.localPosition = Vector2.Lerp(rectTransform.localPosition, uiPosition, Time.deltaTime * 100.0f);
        }
        else
        {
            ActiveLockUI(false);
        }
    }

    public void ActiveLockUI(bool active)
    {
        if (lockOnUI.activeSelf == active) { return; }
        lockOnUI.SetActive(active);
    }
}
