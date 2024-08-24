using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : SoundController
{
    public override SoundManager.SoundType GetSoundType(){return SoundManager.SoundType.SE;}
    public enum PlayerSoundTag
    {
        Foot,
        Rolling,
        Damage,
        WeaponReceipt,
        FirstAttack,
        Sword
    }
}
