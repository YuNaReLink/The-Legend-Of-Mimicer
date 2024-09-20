/// <summary>
/// プレイヤーの複数あるStateを一括で管理するクラス
/// </summary>
public class PlayerState
{
    public PlayerState(PlayerController _controller)
    {
        controller = _controller;
    }

    private PlayerController    controller = null;

    private IdleState           idleState = null;

    private MoveState           moveState = null;

    private RollingState        rollingState = null;

    private FallState           fallState = null;

    private PushState           pushState = null;

    private ModeChangeState     modeChangeState = null;

    private DamageState         damageState = null;

    /// <summary>
    /// 右手に設定する道具のクラス
    /// </summary>
    private RightHandState      rightHandState = null;

    /// <summary>
    /// 左手に設定する道具のクラス
    /// </summary>
    private LeftHandState       leftHandState = null;

    private InterfaceState[]    interfaceState;
    /// <summary>
    /// Awakeで各Stateを生成
    /// </summary>
    public void AwakeInitilaize()
    {
        idleState =         new IdleState(controller);
        moveState =         new MoveState(controller);
        rollingState =      new RollingState(controller);
        fallState =         new FallState(controller);
        pushState =         new PushState(controller);
        modeChangeState =   new ModeChangeState(controller);
        damageState =       new DamageState(controller);
        rightHandState =    new RightHandState(controller);
        leftHandState =     new LeftHandState(controller);
        interfaceState =    new InterfaceState[]
        {
            pushState,
            rollingState,
            idleState,
            moveState,
            modeChangeState,
            rightHandState,
            leftHandState,
            fallState,
            damageState
        };
    }
    /// <summary>
    /// 複数あるState処理をfor文で処理
    /// </summary>
    public void StateUpdate()
    {
        if (controller.CharacterStatus.DeathFlag) { return; }
        if (controller.GetTimer().GetTimerNoAccele().IsEnabled()) { return; }
        if(controller.CharacterStatus.CurrentState == CharacterTagList.StateTag.Damage) { return; }
        if (!controller.CharacterRB.useGravity) { return; }
        for(int i = 0; i < interfaceState.Length; i++)
        {
            interfaceState[i].DoUpdate();
        }
    }
}
