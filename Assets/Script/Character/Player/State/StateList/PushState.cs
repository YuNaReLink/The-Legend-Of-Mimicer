

public class PushState : InterfaceState
{
    private PlayerController controller = null;
    public PushState(PlayerController _controller)
    {
        controller = _controller;
    }

    public void DoUpdate()
    {
        if (CameraController.Instance.IsFPSMode()) { return; }
        if (!controller.CharacterStatus.Landing) { return; }
        if (controller.PushTag == CharacterTagList.PushTag.Null) { return; }
        controller.GetMotion().ChangeMotion(CharacterTagList.StateTag.Push);
    }
}
