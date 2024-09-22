using UnityEngine;
using System.Collections;

/// <summary>
/// �q�b�g�X���[���s���V���O���g���N���X
/// �V�[���̃q�G�����L�[�̃I�u�W�F�N�g�ɃA�^�b�`���Ďg�p
/// </summary>
public class HitThrowManager : MonoBehaviour
{
    // �ǂ�����ł��Ăяo����悤�ɂ���
    public static HitThrowManager    instance;

    private bool                    hitStop = false;
    public bool                     IsHitThrow() { return hitStop; }

    private const float             HitStopCount = 0.5f;

    public float                    GetHitStopCount => HitStopCount;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // �q�b�g�X�g�b�v���J�n����֐�
    public void StartHitStop(float duration)
    {
        instance.StartCoroutine(instance.HitThrowCoroutine(duration));
    }

    // �R���[�`���̓��e
    private IEnumerator HitThrowCoroutine(float duration)
    {
        hitStop = true;
        // �q�b�g�X���[�̊J�n
        Time.timeScale = 0.1f;
        // �w�肵�����Ԃ�����~
        yield return new WaitForSecondsRealtime(duration);
        hitStop = false;
        // �q�b�g�X���[�̏I��
        Time.timeScale = 1f;
    }
}
