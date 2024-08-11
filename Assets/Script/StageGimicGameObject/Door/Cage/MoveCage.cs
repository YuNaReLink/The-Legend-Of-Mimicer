using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCage : MonoBehaviour
{
    [SerializeField]
    private bool open = false;
    [SerializeField]
    private Vector3 goleCagePos = new Vector3(0, 7, 0);

    [SerializeField]
    private float openSpeed = 10f;

    [SerializeField]
    private OpenDoor door = null;

    [SerializeField]
    private SwitchGimic switchGimic = null;

    void Start()
    {
        open = false;
        if(door != null)
        {
            door.enabled = false;
        }
    }

    void Update()
    {
        if(switchGimic == null) { return; }
        if (switchGimic.IsSwitchFlag())
        {
            open = true;
        }
        Move();
    }

    private void Move()
    {
        if (!open) { return; }
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, goleCagePos, openSpeed * Time.deltaTime);
        Vector3 doorpos = transform.localPosition;
        if (doorpos.y >= goleCagePos.y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, goleCagePos.y, transform.localPosition.z);
            open = false;
            if(door != null)
            {
                door.enabled = true;
            }
            if(switchGimic != null)
            {
                switchGimic = null;
            }
        }
    }
}
