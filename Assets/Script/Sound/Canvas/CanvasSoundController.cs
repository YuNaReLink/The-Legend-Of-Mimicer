using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSoundController : SoundController
{
    public enum CanvasSoundTag
    {
        Select,
        Deside,
        Cancel
    }

    public void PlaySelectSound()
    {
        PlaySESound((int)CanvasSoundTag.Select);
    }
}
