using UnityEngine;


public class CharacterScriptableObject : ScriptableObject
{
    /// <summary>
    /// ���[�J���ϐ�
    /// </summary>
    [Header("�ő�HP"), SerializeField]
    protected float maxHP;
    [Header("�ړ�������"), SerializeField]
    protected float acceleration;
    [Header("�ړ��ő呬�x"), SerializeField]
    protected float maxSpeed;

    public float MaxHP { get { return maxHP; } }
    public float Acceleration { get { return acceleration; } }
    public float MaxSpeed { get { return maxSpeed; } }
}
