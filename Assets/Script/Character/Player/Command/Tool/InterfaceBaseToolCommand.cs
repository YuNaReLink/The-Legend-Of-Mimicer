
/// <summary>
/// 道具のタグ
/// Interface内で指定して道具を差別化する
/// </summary>
public enum ToolTag
{
    Null,
    Sword,
    Shield,
    CrossBow,
    Other,
    DataEnd
}
/// <summary>
/// 各道具にあった入力・実行を行うクラスのInterface
/// </summary>

public interface InterfaceBaseToolCommand
{
    public void Input();

    public void Execute();
}
