using UnityEngine;
using System.Collections;

/// <summary>
/// �q�b�g�X�g�b�v���s���V���O���g���N���X
/// �V�[���̃q�G�����L�[�̃I�u�W�F�N�g�ɃA�^�b�`���Ďg�p
/// </summary>
public class HitStopManager : MonoBehaviour
{
    // �ǂ�����ł��Ăяo����悤�ɂ���
    public static HitStopManager    instance;

    private bool                    hitStop = false;
    public bool                     IsHitStop() { return hitStop; }

    private const float             hitStopCount = 0.5f;

    public float                    HitStopCount => hitStopCount;

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
        instance.StartCoroutine(instance.HitStopCoroutine(duration));
    }

    // �R���[�`���̓��e
    private IEnumerator HitStopCoroutine(float duration)
    {
        hitStop = true;
        // �q�b�g�X�g�b�v�̊J�n
        Time.timeScale = 0.1f;

        // �w�肵�����Ԃ�����~
        yield return new WaitForSecondsRealtime(duration);
        hitStop = false;
        // �q�b�g�X�g�b�v�̏I��
        Time.timeScale = 1f;
    }
}
