using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundController : SoundController
{
    public enum GameBGMSoundTag
    {
        Stage,
        Battle,
        BossBattle,
        GameOver,
        GameClear
    }

    public float BGMVolume
    {
        get
        {
            return audioSource.volume;
        }
        set
        {
            audioSource.volume = Mathf.Clamp01(value);
        }
    }

    private bool changeBGM = false;

    private AudioClip saveBGMClip = null;

    private const float volumSpeed = 0.01f;

    private const float BGMVolumData = 1.0f;
    public float GetBGMVolumData() { return BGMVolumData; }

    public override void AwakeInitilaize()
    {
        base.AwakeInitilaize();
        audioSource.volume = 0;
    }

    public void PlayBGM(int num)
    {
        if (audioSource == null) { return; }
        if (clipData == null) { return; }
        if (audioSource.clip == clipData.AudioClipList[num]) { return; }
        if (changeBGM) { return; }
        if (audioSource.clip == null)
        {
            audioSource.clip = clipData.AudioClipList[num];
            audioSource.Play();
        }
        else
        {
            saveBGMClip = clipData.AudioClipList[num];
            changeBGM = true;
        }
    }

    public void GameSoundUpdate()
    {
        ChangeBGMUpdtate();
        switch (GameManager.GameState)
        {
            case GameManager.GameStateEnum.Game:
            case GameManager.GameStateEnum.Pose:
                if (GameSceneSystemController.Instance.BossBattleStart)
                {
                    PlayBGM((int)GameBGMSoundTag.BossBattle);
                }
                else if (GameSceneSystemController.Instance.BattleStart)
                {
                    PlayBGM((int)GameBGMSoundTag.Battle);
                }
                else
                {
                    PlayBGM((int)GameBGMSoundTag.Stage);
                }
                break;
            case GameManager.GameStateEnum.GameOver:
                PlayBGM((int)GameBGMSoundTag.GameOver);
                break;
            case GameManager.GameStateEnum.GameClear:
                PlayBGM((int)GameBGMSoundTag.GameClear);
                break;
        }
    }
    public void ChangeBGMUpdtate()
    {
        if (changeBGM)
        {
            BGMVolume -= volumSpeed;
            if (BGMVolume <= 0)
            {
                BGMVolume = 0;
                changeBGM = false;
                audioSource.clip = saveBGMClip;
                audioSource.Play();
            }
        }
        else if (BGMVolume <= BGMVolumData)
        {
            BGMVolume += volumSpeed;
            if (BGMVolume >= BGMVolumData)
            {
                BGMVolume = BGMVolumData;
            }
        }
    }
}
