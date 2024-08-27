using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundTagList
{
    public enum GetToolSoundTag
    {
        Get
    }

    public enum OpenDoorSoundTag
    {
        Open
    }
}

public class SoundController : MonoBehaviour
{
    [SerializeField]
    protected SoundManager.SoundType soundType = SoundManager.SoundType.Null;
    public SoundManager.SoundType GetSoundType() { return soundType; }
    [SerializeField]
    protected AudioSource audioSource = null;
    [SerializeField]
    protected SoundClipData clipData = null;
    public SoundClipData GetClipData { get { return clipData; } }

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
