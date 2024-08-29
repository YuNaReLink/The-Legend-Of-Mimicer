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
            // 3Dオブジェクトのワールド座標をスクリーン座標に変換
            Vector3 screenPosition = camera.WorldToScreenPoint(CameraController.LockObject.transform.position);

            // CanvasのRectTransformを取得
            Canvas canvas = playerConnectionUI.GetGameUIController().GetComponent<Canvas>();
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            // スクリーン座標をキャンバスのローカル座標に変換
            Vector2 uiPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, null, out uiPosition);

            // UIオブジェクトの位置を設定
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
