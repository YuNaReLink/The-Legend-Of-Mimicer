using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotationDoor : MonoBehaviour
{
    private GameObject door = null;

    [SerializeField]
    private bool open = false;

    // 開くスピードを調整するための変数
    [SerializeField]
    private float moveSpeed = 2.0f; 
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private TriggerCheck triggerCheck = null;

    private SoundController soundController = null;

    private DeltaTimeCountDown closeTimer = null;

    private void Awake()
    {
        door = transform.GetChild(0).gameObject;
        triggerCheck = GetComponent<TriggerCheck>();
        soundController = GetComponent<SoundController>();
        if(soundController != null)
        {
            soundController.AwakeInitilaize();
        }
        closeTimer = new DeltaTimeCountDown();
    }
    void Start()
    {
        if (door != null)
        {
            closedRotation = door.transform.rotation;
            openRotation = Quaternion.Euler(0, 90, 0) * closedRotation;
        }
    }

    void Update()
    {
        closeTimer.Update();
        if (closeTimer.IsEnabled()) { return; }
        OpenInput();
        if (open)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    private void OpenInput()
    {
        if (triggerCheck.GetController() == null) { return; }
        if (InputManager.GetItemButton()&& !open)
        {
            // Fキーが押されたら開閉を切り替える
            open = true;
            soundController.PlaySESound((int)SoundTagList.OpenDoorSoundTag.Open);
        }
    }

    private void Open()
    {
        door.transform.rotation = Quaternion.Lerp(door.transform.rotation, openRotation, Time.deltaTime * moveSpeed);
        Vector3 sub = door.transform.rotation.eulerAngles - openRotation.eulerAngles;
        if(sub.magnitude < 0.1f)
        {
            closeTimer.StartTimer(5f);
            open = false;
        }
    }

    private void Close()
    {
        door.transform.rotation = Quaternion.Lerp(door.transform.rotation, closedRotation, Time.deltaTime * moveSpeed);
    }
}
