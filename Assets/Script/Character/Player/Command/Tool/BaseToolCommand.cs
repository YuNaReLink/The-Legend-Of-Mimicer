using UnityEngine;

/// <summary>
/// ����̃^�O
/// Interface���Ŏw�肵�ē�������ʉ�����
/// </summary>
public enum ToolTag
{
    Null,
    Sword,
    Shield,
    CrossBow,
    DataEnd
}
/// <summary>
/// �e����ɂ��������́E���s���s���N���X��Interface
/// </summary>

public interface BaseToolCommand
{
    public ToolTag GetToolTag();
    public void Input();

    public void Execute();
}
