using UnityEngine;

public class TitleSoundController : SoundController
{
    public enum TitleBGMTag
    {
        Title
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

    private bool        changeBGM = false;

    private AudioClip   saveBGMClip = null;

    private const float VolumSpeed = 0.01f;

    private const float BGMVolumData = 1.0f;
    public float        GetBGMVolumData() { return BGMVolumData; }

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

    public void TitleSoundUpdate()
    {
        ChangeBGMUpdtate();
        switch (GameManager.GameState)
        {
            case GameManager.GameStateEnum.Game:
            case GameManager.GameStateEnum.Pose:
                if (GameSceneSystemController.Instance.BossBattleStart)
                {
                    PlayBGM((int)GameSoundController.GameBGMSoundTag.BossBattle);
                }
                else if (GameSceneSystemController.Instance.BattleStart)
                {
                    PlayBGM((int)GameSoundController.GameBGMSoundTag.Battle);
                }
                else
                {
                    PlayBGM((int)GameSoundController.GameBGMSoundTag.Stage);
                }
                break;
            case GameManager.GameStateEnum.GameOver:
                PlayBGM((int)GameSoundController.GameBGMSoundTag.GameOver);
                break;
            case GameManager.GameStateEnum.GameClear:
                PlayBGM((int)GameSoundController.GameBGMSoundTag.GameClear);
                break;
        }
    }
    public void ChangeBGMUpdtate()
    {
        if (changeBGM)
        {
            BGMVolume -= VolumSpeed;
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
            BGMVolume += VolumSpeed;
            if (BGMVolume >= BGMVolumData)
            {
                BGMVolume = BGMVolumData;
            }
        }
    }
}
