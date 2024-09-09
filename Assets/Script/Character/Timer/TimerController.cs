using UnityEngine;

/// <summary>
/// �^�C�}�[�̊��N���X
/// ���̃N���X�����Ɍp�����
/// �^�C�}�[���܂Ƃ߂ď������s��
/// </summary>
public class TimerController
{
    /// <summary>
    /// �J�E���g�_�E���N���X
    /// </summary>
    protected InterfaceCountDown[] updateCountDowns;

    public virtual void InitializeAssignTimer() { }

    public void TimerUpdate()
    {
        if (Time.timeScale <= 0) { return; }
        foreach (var countdown in updateCountDowns)
        {
            if (countdown.IsEnabled())
            {
                countdown.Update();
            }
        }
    }
}
