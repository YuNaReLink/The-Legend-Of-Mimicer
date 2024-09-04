

public class PushState : InterfaceState
{
    private PlayerController controller = null;
    public PushState(PlayerController _controller)
    {
        controller = _controller;
    }

    public void DoUpdate()
    {
        if (controller.GetCameraController().IsFPSMode()) { return; }
        if (!controller.Landing) { return; }
        if (controller.PushTag == CharacterTagList.PushTag.Null) { return; }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Push);
    }
}
