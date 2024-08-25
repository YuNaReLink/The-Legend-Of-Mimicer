using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public virtual SoundManager.SoundType GetSoundType() { return SoundManager.SoundType.Null; }
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
            Debug.LogWarning("AudioSourceがアタッチされていない");
        }
    }

    public void PlaySESound(int num)
    {
        if(audioSource == null) { return; }
        if(clipData == null) { return; }
        audioSource.PlayOneShot(clipData.AudioClipList[num]);
    }
}
