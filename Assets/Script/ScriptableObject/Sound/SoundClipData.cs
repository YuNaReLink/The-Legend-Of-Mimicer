using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SoundClipData : ScriptableObject
{
    [SerializeField]
    private List<AudioClip> audioClipList = new List<AudioClip>();
    public List<AudioClip> AudioClipList {  get { return audioClipList; } }
}
