using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSoundController : SoundController
{
    public enum SpiderSoundTag
    {
        Foot,
        Damage,
    }

    private DeltaTimeCountDown moveSECoolDown = null;
    public DeltaTimeCountDown GetMoveSECoolDown() {  return moveSECoolDown; }

    public override void AwakeInitilaize()
    {
        base.AwakeInitilaize();
        moveSECoolDown = new DeltaTimeCountDown();
    }

    public void TimerUpdate()
    {
        moveSECoolDown.Update();
    }

    public void FixedPlaySESound(int num)
    {
        if (moveSECoolDown.IsEnabled()) { return; }
        if (audioSource == null) { return; }
        if (clipData == null) { return; }
        audioSource.PlayOneShot(clipData.AudioClipList[num]);
        moveSECoolDown.StartTimer(0.1f);
    }
}
