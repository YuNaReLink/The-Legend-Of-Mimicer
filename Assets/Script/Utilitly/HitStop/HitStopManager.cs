using UnityEngine;
using System.Collections;

public class HitStopManager : MonoBehaviour
{
    // �ǂ�����ł��Ăяo����悤�ɂ���
    public static HitStopManager instance;

    private bool hitStop = false;
    public bool IsHitStop() { return hitStop; }

    private void Start()
    {
        instance = this;
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
        Time.timeScale = 0f;

        // �w�肵�����Ԃ�����~
        yield return new WaitForSecondsRealtime(duration);
        hitStop = false;
        // �q�b�g�X�g�b�v�̏I��
        Time.timeScale = 1f;
    }
}
