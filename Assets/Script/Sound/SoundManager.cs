using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum SoundType
    {
        Null,
        SE,
        BGM
    }

    private List<SoundController> soundControllerList = new List<SoundController>();
    [SerializeField]
    private List<AudioSource> seAudioSourceList = new List<AudioSource>();
    [SerializeField]
    private List<AudioSource> bgmAudioSourceList = new List<AudioSource>();

    private void Awake()
    {
        // ÉVÅ[Éìì‡ÇÃÇ∑Ç◊ÇƒÇÃGameObjectÇéÊìæ
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        SoundController soundController = null;
        AudioSource audioSource = null;
        foreach (GameObject obj in allObjects)
        {
            soundController = obj.GetComponent<SoundController>();
            audioSource = obj.GetComponent<AudioSource>();
            if(soundController != null&&audioSource != null)
            {
                List<AudioSource> list = new List<AudioSource>();
                switch (soundController.GetSoundType())
                {
                    case SoundType.SE:
                        list = seAudioSourceList;
                        break;
                    case SoundType.BGM:
                        list = bgmAudioSourceList;
                        break;
                }
                list.Add(audioSource);
            }
        }
    }


}
