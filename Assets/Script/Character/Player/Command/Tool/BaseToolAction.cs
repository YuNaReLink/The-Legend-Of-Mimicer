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
    Other,
    DataEnd
}
/// <summary>
/// �e����ɂ��������́E���s���s���N���X��Interface
/// </summary>

public interface BaseToolAction
{
    public ToolTag GetToolTag();
    public void Input();

    public void Execute();
}
