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
}
