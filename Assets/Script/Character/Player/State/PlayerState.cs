using UnityEngine;

public class PlayerState
{
    public PlayerState(PlayerController _controller)
    {
        controller = _controller;
    }

    private PlayerController controller = null;

    private IdleState idleState = null;

    private MoveState moveState = null;

    private RollingState rollingState = null;

    private FallState fallState = null;

    private PushState pushState = null;

    private ModeChangeState modeChangeState = null;

    private DamageState damageState = null;

    /// <summary>
    /// 右手に設定する道具のクラス
    /// </summary>
    private RightHandState rightHandState = null;

    /// <summary>
    /// 左手に設定する道具のクラス
    /// </summary>
    private LeftHandState leftHandState = null;

    private InterfaceState[] interfaceState;

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

    public void StateUpdate()
    {
        if (controller.CharacterStatus.DeathFlag) { return; }
        if (controller.GetTimer().GetTimerNoAccele().IsEnabled()) { return; }
        for(int i = 0; i < interfaceState.Length; i++)
        {
            interfaceState[i].DoUpdate();
        }
    }
}
