using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    /// <summary>
    /// �W�̃I�u�W�F�N�g�̓��ꕨ
    /// </summary>
    [SerializeField]
    private GameObject lidObject = null;
    /// <summary>
    /// �����̊p�x��ۑ�
    /// </summary>
    [SerializeField]
    private Vector3 baseRotate = Vector3.zero;
    /// <summary>
    /// �J���p�x��ۑ��������
    /// </summary>
    [SerializeField]
    private Vector3 openRotate = Vector3.zero;
    /// <summary>
    /// �󔠂��J����]�p�xX
    /// </summary>
    [SerializeField]
    private float openRotateX = -100;
    /// <summary>
    /// �󔠂̊J���X�s�[�h
    /// </summary>
    [SerializeField]
    private float openSpeed = 5f;
    /// <summary>
    /// �v���C���[���󔠎��ӂ̓����蔻��ɓ������Ă��邩���ׂ邽�߂̃N���X
    /// </summary>
    [SerializeField]
    private TriggerCheck triggerCheck = null;
    /// <summary>
    /// �󔠂��J�����߂̃t���O
    /// </summary>
    [SerializeField]
    private bool open = false;
    /// <summary>
    /// �󔠂��J���I��������̃t���O
    /// </summary>
    [SerializeField]
    private bool stop = false;
    /// <summary>
    /// �A�C�e�����v���C���[�Ɏ擾������N���X
    /// </summary>
    [SerializeField]
    private GetChestItem getItem = null;
    private void Start()
    {
        triggerCheck = GetComponentInChildren<TriggerCheck>();

        getItem = GetComponent<GetChestItem>();

        open = false;
        stop = false;

        baseRotate = lidObject.transform.localRotation.eulerAngles;
        openRotate = baseRotate;
        openRotate.x = openRotateX;
    }

    private void Update()
    {
        if (!triggerCheck.IsHitPlayer()) { return; }
        if (stop) { return; }
        OpenInput();
        OpenCover();
    }

    private void OpenInput()
    {
        if (open) { return; }
        if (!triggerCheck.GetController().GetKeyInput().IsGetButton()) { return; }
        open = true;
    }

    private void OpenCover()
    {
        if (!open) { return; }
        if(lidObject == null) { return; }
        Vector3 sub = openRotate - baseRotate;
        Vector3 normal = sub.normalized;
        Vector3 addRotate = lidObject.transform.localRotation.eulerAngles;
        addRotate += normal * openSpeed;
        lidObject.transform.localRotation = Quaternion.Euler(addRotate);
        float currentRotate = lidObject.transform.localRotation.eulerAngles.x;
        currentRotate -= 360f;
        if(currentRotate <= openRotateX + 10f)
        {
            Vector3 rotate = Vector3.zero;
            rotate.x = openRotateX;
            lidObject.transform.localRotation = Quaternion.Euler(rotate);
            stop = true;
            //�W���󂫐؂������ɃA�C�e���N���X�Ńv���C���[�Ɏw�肳�ꂽ�A�C�e�����擾������
            getItem.Get();
        }
    }
}
