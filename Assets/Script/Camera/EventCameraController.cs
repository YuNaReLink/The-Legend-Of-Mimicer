using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCameraController : MonoBehaviour
{
    [SerializeField]
    private PlayerCameraController playerCamera = null;

    [SerializeField]
    private List<Transform> focusEventTransform = new List<Transform>();

    [SerializeField]
    private List<Transform> moveTransforms = new List<Transform>();

    public enum EventNumber
    {
        Null = -1,
        event1,
        event2,
        event3,
        event4,
        DataEnd
    }

    [SerializeField]
    private EventNumber eventNumber = EventNumber.Null;

    [SerializeField]
    private Vector3 movePos = Vector3.zero;

    [SerializeField]
    private Quaternion moveRot = Quaternion.identity;

    private void Start()
    {
        eventNumber = EventNumber.event1;
    }

    void Update()
    {
        EventExecute();
    }

    private void EventExecute()
    {
        switch (eventNumber)
        {
            case EventNumber.event1:
                Event1Run();
                break;
            case EventNumber.event2:
                Event2Run();
                break;
        }
    }
    //  イージング関数
    private float Ease(float x)
    {
        return x * x * x;
    }

    private void Event1Run()
    {
        if (playerCamera.enabled)
        {
            playerCamera.enabled = false;
            transform.position = focusEventTransform[(int)EventNumber.event1].position;
            transform.rotation = focusEventTransform[(int)EventNumber.event1].rotation;
            movePos = transform.position+new Vector3(-11f,0,0);
            moveRot.eulerAngles = transform.rotation.eulerAngles + new Vector3(0,-90f,0);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, moveRot, Time.deltaTime);
        
        //float x = Mathf.Lerp(transform.position.x, movePos.x, Time.deltaTime);
        //float z = Mathf.Lerp(transform.position.z, movePos.z, Time.deltaTime);
        //float y = Mathf.Lerp(transform.position.y, movePos.y, Time.deltaTime);
        //transform.position = new Vector3(x, y, z);
        Vector3 sub = transform.position - movePos;
        Vector3 normal = sub.normalized;
        transform.position -= normal * 0.2f;
        float dis = sub.magnitude;
        if(dis <= 0.1f)
        {
            eventNumber = EventNumber.event2;
            transform.position = focusEventTransform[(int)EventNumber.event2].position;
            transform.rotation = focusEventTransform[(int)EventNumber.event2].rotation;
            movePos = transform.position + (transform.forward * 5f);
        }
    }

    private void Event2Run()
    {
        float x = Mathf.Lerp(transform.position.x, movePos.x, Time.deltaTime);
        float z = Mathf.Lerp(transform.position.z, movePos.z, Time.deltaTime);
        float y = Mathf.Lerp(transform.position.y, movePos.y, Time.deltaTime);
        transform.position = new Vector3(x, y, z);
        Vector3 sub = transform.position - movePos;
        float dis = sub.magnitude;
        if (dis <= 0.1f)
        {
            eventNumber = EventNumber.Null;
            playerCamera.enabled = true;
        }
    }
}
