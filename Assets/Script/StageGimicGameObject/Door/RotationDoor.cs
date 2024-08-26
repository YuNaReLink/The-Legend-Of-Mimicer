using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotationDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject door = null;

    [SerializeField]
    private bool open = false;

    [SerializeField]
    private float openSpeed = 2.0f; // 開くスピードを調整するための変数
    private Quaternion closedRotation;
    private Quaternion openRotation;

    [SerializeField]
    private TriggerCheck triggerCheck = null;

    private SoundController soundController = null;

    private void Awake()
    {
        soundController = GetComponent<SoundController>();
        if(soundController != null)
        {
            soundController.AwakeInitilaize();
        }
    }
    void Start()
    {
        triggerCheck = GetComponent<TriggerCheck>();
        if (door != null)
        {
            closedRotation = door.transform.rotation;
            openRotation = Quaternion.Euler(0, 90, 0) * closedRotation;
        }
    }

    void Update()
    {
        OpenDoor();
        MoveDoor();
    }

    private void OpenDoor()
    {
        if (triggerCheck.GetController() == null) { return; }
        if (InputManager.GetItemButton())
        {
            // Fキーが押されたら開閉を切り替える
            open = true;
            soundController.PlaySESound((int)SoundTagList.OpenDoorSoundTag.Open);
        }
    }

    private void MoveDoor()
    {
        if (open)
        {
            door.transform.rotation = Quaternion.Lerp(door.transform.rotation, openRotation, Time.deltaTime * openSpeed);
        }
        else
        {
            door.transform.rotation = Quaternion.Lerp(door.transform.rotation, closedRotation, Time.deltaTime * openSpeed);
        }
    }
}
