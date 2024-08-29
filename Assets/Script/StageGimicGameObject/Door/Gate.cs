using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    /// <summary>
    /// �Q�[�g�I�u�W�F�N�g�̕ϐ�
    /// </summary>
    [SerializeField]
    private GameObject          gateGameObject = null;
    /// <summary>
    /// �ŏ��̃Q�[�g�̈ʒu��ۑ�����ϐ�
    /// </summary>
    [SerializeField]
    private Vector3             baseGatePosition = Vector3.zero;
    /// <summary>
    /// �Q�[�g�̈ړ���ʒu��ۑ�����ϐ�
    /// </summary>
    [SerializeField]
    private Vector3             goleGatePosition = Vector3.zero;
    /// <summary>
    /// �ړ����鑬�x
    /// </summary>
    [SerializeField]
    private float               golePositionY = 10f;
    /// <summary>
    /// �����J�����߂̃X�C�b�`�N���X���i�[���郊�X�g
    /// </summary>
    [SerializeField]
    private List<SwitchGimic>   switchGimics = new List<SwitchGimic>();
    /// <summary>
    /// �����J���邽�߂̃t���O
    /// </summary>
    [SerializeField]
    private bool                openGate = false;
    /// <summary>
    /// ���̊J�����x
    /// </summary>
    [SerializeField]
    private float               openSpeed = 5f;
    /// <summary>
    /// ���̌��ʉ����Ǘ�����N���X
    /// </summary>
    private SoundController     soundController = null;

    private void Awake()
    {
        soundController = GetComponent<SoundController>();
        if(soundController != null)
        {
            soundController.AwakeInitilaize();
        }
    }

    private void Start()
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


    private void Update()
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
