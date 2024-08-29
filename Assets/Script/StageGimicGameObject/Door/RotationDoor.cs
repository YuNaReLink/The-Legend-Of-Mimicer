using UnityEngine;

public class RotationDoor : MonoBehaviour
{
    /// <summary>
    /// �J�����̃I�u�W�F�N�g
    /// </summary>
    private GameObject          door = null;
    /// <summary>
    /// �����J�����߂̃t���O
    /// </summary>
    [SerializeField]
    private bool                open = false;

    //�J���X�s�[�h�𒲐����邽�߂̕ϐ�
    [SerializeField]
    private float               moveSpeed = 2.0f; 
    /// <summary>
    /// �܂������̉�]�p�x
    /// </summary>
    private Quaternion          closedRotation;
    /// <summary>
    /// �J�������̉�]�p�x
    /// </summary>
    private Quaternion          openRotation;
    /// <summary>
    /// �����J�����߂Ƀv���C���[�Ƃ̓����蔻����s���Ă���N���X
    /// </summary>
    private TriggerCheck        triggerCheck = null;
    /// <summary>
    /// ���̌��ʉ��̊Ǘ����s���N���X
    /// </summary>
    private SoundController     soundController = null;
    /// <summary>
    /// �����܂�܂ł̃J�E���g
    /// </summary>
    private DeltaTimeCountDown  closeTimer = null;

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
            // F�L�[�������ꂽ��J��؂�ւ���
            open = true;
            soundController.PlaySESound((int)SoundTagList.OpenDoorSoundTag.Open);
            triggerCheck.SetTriggerTag(false);
        }
    }

    private void Open()
    {
        door.transform.rotation = Quaternion.Lerp(door.transform.rotation, openRotation, Time.deltaTime * moveSpeed);
        Vector3 sub = door.transform.rotation.eulerAngles - openRotation.eulerAngles;
        if(sub.magnitude < 0.1f)
        {
            open = false;
            closeTimer.StartTimer(5f);
            closeTimer.OnCompleted += () =>
            {
                if(triggerCheck.GetController() != null)
                {
                    triggerCheck.SetTriggerTag(true);
                }
            };
        }
    }

    private void Close()
    {
        door.transform.rotation = Quaternion.Lerp(door.transform.rotation, closedRotation, Time.deltaTime * moveSpeed);
    }
}
