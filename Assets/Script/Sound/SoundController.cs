using UnityEngine;

/// <summary>
/// SoundControllerを継承するほどの変更がない時に
/// SOundControllerのまま使うときに使うタグをまとめたnamespace
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
/// ゲームオブジェクトにアタッチして効果音、BGMを再生するための基底クラス
/// 効果音、音楽を管理し再生の制御を行うことからSoundControllr
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
                Debug.LogWarning("AudioSourceがアタッチされていない");
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
