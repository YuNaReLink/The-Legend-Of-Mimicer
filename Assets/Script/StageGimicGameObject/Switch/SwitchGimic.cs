using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGimic : MonoBehaviour
{
    /// <summary>
    /// �X�C�b�`��ON��OFF�����肷��t���O
    /// </summary>
    [SerializeField]
    private bool                switchFlag = false;
    public bool                 IsSwitchFlag() {  return switchFlag; }
    /// <summary>
    /// �X�C�b�`�I�u�W�F�N�g�̃J���[��ύX����N���X
    /// </summary>
    private ColorChange         colorChange = null;
    /// <summary>
    /// �X�C�b�`�̌��ʉ����Ǘ�����N���X
    /// </summary>
    private SoundController     soundController = null;
    /// <summary>
    /// �X�C�b�`�̃G�t�F�N�g���Ǘ�����N���X
    /// </summary>
    private EffectController    effectController = null;
    /// <summary>
    /// �G�t�F�N�g�̑傫����ݒ肷��ϐ�
    /// </summary>
    private const float         effectScale = 1.0f;
    /// <summary>
    /// ���ʉ��ƃG�t�F�N�g�𔭐�������̂��~�߂�^�C�}�[�N���X
    /// </summary>
    private DeltaTimeCountDown  hitCoolDownTimer = null;
    /// <summary>
    /// �^�C�}�[�N���X�Ɏg���J�E���g�ϐ�
    /// </summary>
    private const float         coolDownCount = 0.5f;

    private void Awake()
    {
        colorChange = GetComponentInChildren<ColorChange>();
        soundController = GetComponent<SoundController>();
        if(soundController != null)
        {
            soundController.AwakeInitilaize();
        }        
        effectController = GetComponent<EffectController>();
        hitCoolDownTimer = new DeltaTimeCountDown();
    }

    private void Update()
    {
        hitCoolDownTimer.Update();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (hitCoolDownTimer.IsEnabled()) { return; }
        if(other.tag != "Attack") { return; }
        if (!switchFlag)
        {
            switchFlag = true;
            colorChange.ChangeMaterial(ColorChange.MaterialTag.Two);
        }
        soundController.PlaySESound((int)SoundTagList.SwicthSoundTag.Hit);
        effectController.CreateVFX((int)EffectTagList.SwordHitTag.Hit, other.transform.position,effectScale, Quaternion.identity);
        hitCoolDownTimer.StartTimer(coolDownCount);
    }
}
