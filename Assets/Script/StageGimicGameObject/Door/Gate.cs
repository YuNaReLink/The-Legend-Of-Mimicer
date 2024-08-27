using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField]
    private GameObject gateGameObject = null;

    [SerializeField]
    private Vector3 baseGatePosition = Vector3.zero;
    [SerializeField]
    private Vector3 goleGatePosition = Vector3.zero;
    [SerializeField]
    private float golePositionY = 10f;

    [SerializeField]
    private List<SwitchGimic> switchGimics = new List<SwitchGimic>();


    [SerializeField]
    private bool openGate = false;

    [SerializeField]
    private float openSpeed = 5f;

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
        for(int i = 0;i < transform.childCount; i++)
        {
            SwitchGimic switchGimic = transform.GetChild(i).GetComponent<SwitchGimic>();
            if(switchGimic == null) { continue; }
            switchGimics.Add(switchGimic);
        }
        Vector3 baseGate = gateGameObject.transform.position;
        goleGatePosition = new Vector3(baseGate.x, baseGate.y + golePositionY, baseGate.z);
    }


    void Update()
    {
        CheckSwitch();

        Open();
    }

    private void CheckSwitch()
    {
        if (openGate) { return; }
        int truecount = 0;
        for (int i = 0; i < switchGimics.Count; i++)
        {
            if (switchGimics[i].IsSwitchFlag())
            {
                truecount++;
            }
        }

        if (truecount >= switchGimics.Count)
        {
            openGate = true;
            soundController.PlaySESound((int)SoundTagList.OpenDoorSoundTag.Open);
        }
    }

    private void Open()
    {
        if (!openGate) { return; }
        gateGameObject.transform.position = Vector3.Lerp(gateGameObject.transform.position,goleGatePosition, Time.deltaTime * openSpeed);
    }
}
