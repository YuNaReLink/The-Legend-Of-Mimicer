using UnityEngine;

[System.Serializable]
public class CameraStatus
{
    [Header("カメラとターゲットの初期距離")]
    [SerializeField]
    private float           baseDistance = 10.0f;
    public float            BaseDistance => baseDistance;
    public void             SetBaseDistance(float distance) { baseDistance = distance; }
    [SerializeField]
    private float           maxDistance = 10.0f;
    public float            MaxDistance => maxDistance;
    [Header("カメラのX移動スピード")]
    [SerializeField]
    private float           mouseXSpeed = 5.0f;
    public float            MouseXSpeed => mouseXSpeed;
    public void             SetMouseXSpeed(float speed) { mouseXSpeed = speed; }
    [Header("カメラのY移動スピード")]
    [SerializeField]
    private float           mouseYSpeed = 5.0f;
    public float            MouseYSpeed => mouseYSpeed;
    public void             SetMouseYSpeed(float speed) { mouseYSpeed = speed; }
    [Header("カメラの回転がリセットされる時のスピード")]
    [SerializeField]
    private float           resetCameraSpeed = 5.0f;
    public float            ResetCameraSpeed => resetCameraSpeed;

    public void StartInitialize()
    {
        MouseSensitivityManager.Instance.SetMouseXSpeed(mouseXSpeed);
        MouseSensitivityManager.Instance.SetMouseYSpeed(mouseYSpeed);

        baseDistance = maxDistance;
    }

    public void PoseCameraControl()
    {
        if (MouseSensitivityManager.Instance.GetMouseXSpeed != mouseXSpeed)
        {
            mouseXSpeed = MouseSensitivityManager.Instance.GetMouseXSpeed;
        }
        if (MouseSensitivityManager.Instance.GetMouseYSpeed != mouseYSpeed)
        {
            mouseYSpeed = MouseSensitivityManager.Instance.GetMouseYSpeed;
        }
    }
}
