

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
