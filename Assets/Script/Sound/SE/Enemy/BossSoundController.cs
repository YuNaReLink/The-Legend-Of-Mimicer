using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSoundController : SoundController
{
    public enum BossSoundTag
    {
        Foot,
        Attack,
        Damage,
        WeakPointsDamage,
    }

    private DeltaTimeCountDown moveSECoolDown = null;
    public DeltaTimeCountDown GetMoveSECoolDown() { return moveSECoolDown; }
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
