using UnityEngine;

[System.Serializable]
public class CameraStatus
{
    [Header("�J�����ƃ^�[�Q�b�g�̏�������")]
    [SerializeField]
    private float           baseDistance = 10.0f;
    public float            BaseDistance => baseDistance;
    public void             SetBaseDistance(float distance) { baseDistance = distance; }
    [SerializeField]
    private float           maxDistance = 10.0f;
    public float            MaxDistance => maxDistance;
    [Header("�J������X�ړ��X�s�[�h")]
    [SerializeField]
    private float           mouseXSpeed = 5.0f;
    public float            MouseXSpeed => mouseXSpeed;
    public void             SetMouseXSpeed(float speed) { mouseXSpeed = speed; }
    [Header("�J������Y�ړ��X�s�[�h")]
    [SerializeField]
    private float           mouseYSpeed = 5.0f;
    public float            MouseYSpeed => mouseYSpeed;
    public void             SetMouseYSpeed(float speed) { mouseYSpeed = speed; }
    [Header("�J�����̉�]�����Z�b�g����鎞�̃X�s�[�h")]
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
