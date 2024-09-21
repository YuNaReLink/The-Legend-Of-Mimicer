
/// <summary>
/// プレイヤーの実行クラスをまとめて処理するクラス
/// </summary>
public class PlayerCommands
{
    public PlayerCommands(PlayerController _controller)
    {
        controller = _controller;
    }

    private PlayerController controller = null;
    /// <summary>
    /// プレイヤーの落下処理をまとめたクラス
    /// </summary>
    private FallDistanceCheck fallDistanceCheck = null;
    public FallDistanceCheck GetFallDistanceCheck() { return fallDistanceCheck; }
    /// <summary>
    /// プレイヤーの回避行動を処理するクラス
    /// </summary>
    private RollingCommand rolling = null;

    private PlayerDamageCommand damage = null;
    public PlayerDamageCommand Damage => damage;
    private InterfaceBaseCommand[] interfaceCommand;
    public void AwakeInitilaize()
    {
        fallDistanceCheck = new FallDistanceCheck(controller);
        rolling =           new RollingCommand(controller);
        damage =            new PlayerDamageCommand(controller);

        interfaceCommand = new InterfaceBaseCommand[]
        {
            fallDistanceCheck,
            rolling,
            damage
        };

        fallDistanceCheck.Initialize();
    }

    public void DoUpdate()
    {
        if (controller.CharacterStatus.DeathFlag) { return; }
        for (int i = 0; i < interfaceCommand.Length; i++)
        {
            interfaceCommand[i].Execute();
        }
    }
}
