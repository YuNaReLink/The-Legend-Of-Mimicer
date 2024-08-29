using UnityEngine;

/// <summary>
/// SoundController���p������قǂ̕ύX���Ȃ�����
/// SOundController�̂܂܎g���Ƃ��Ɏg���^�O���܂Ƃ߂�namespace
/// </summary>
namespace SoundTagList
{
    public enum PlayerSoundTag
    {
        Foot,
        Rolling,
        Jump,
        Grab,
        Climb,
        Damage,
        WeaponReceipt,
        FirstAttack,
        Sword1,
        Sword2,
        Sword3,
        SpinAttack,
        Shot,
        ShildPosture,
        Guard,
        GetHeart,
        GetItem
    }
    public enum GetToolSoundTag
    {
        Get
    }

    public enum OpenDoorSoundTag
    {
        Open
    }
    public enum ChestSoundTag
    {
        Open
    }
    public enum SwicthSoundTag
    {
        Hit
    }
    public enum BreakSoundTag
    {
        Break,
    }
}
/// <summary>
/// �Q�[���I�u�W�F�N�g�ɃA�^�b�`���Č��ʉ��ABGM���Đ����邽�߂̊��N���X
/// ���ʉ��A���y���Ǘ����Đ��̐�����s�����Ƃ���SoundControllr
/// </summary>
public class SoundController : MonoBehaviour
{
    [SerializeField]
    protected SoundManager.SoundType    soundType = SoundManager.SoundType.Null;
    public SoundManager.SoundType       GetSoundType() { return soundType; }
    [SerializeField]
    protected AudioSource               audioSource = null;
    [SerializeField]
    protected SoundClipData             clipData = null;
    public SoundClipData                GetClipData { get { return clipData; } }

    public virtual void AwakeInitilaize()
    {
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = GetComponentInParent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogWarning("AudioSource���A�^�b�`����Ă��Ȃ�");
            }
        }
    }

    public void PlaySESound(int num)
    {
        if(audioSource == null) { return; }
        if(clipData == null) { return; }
        audioSource.PlayOneShot(clipData.AudioClipList[num]);
    }
}
