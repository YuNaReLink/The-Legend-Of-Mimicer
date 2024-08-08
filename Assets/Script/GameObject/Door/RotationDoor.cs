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
    private PlayerController controller = null;

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
        OpenDoor();
        MoveDoor();
    }

    private void OpenDoor()
    {
        if (controller == null) { return; }
        if (controller.GetKeyInput().IsFKey())
        {
            // Fキーが押されたら開閉を切り替える
            open = true;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") { return; }
        controller = other.GetComponent<PlayerController>();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player") { return; }
        open = false;
        controller = null;
    }
}
