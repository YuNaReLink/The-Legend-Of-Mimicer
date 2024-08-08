using UnityEngine;

public class EnemyDamageDetection
{
    public EnemyDamageDetection(EnemyController _controller)
    {
        controller = _controller;
    }

    private EnemyController controller = null;

    private bool damageFlag = false;
    public bool DamageFlag {  get { return damageFlag; } set { damageFlag = value; } }

    private float damageValue = 0;
    public float DamageValue { get { return damageValue; } set { damageValue = value; } }

    public void Execute()
    {
        if (damageFlag)
        {
            controller.HP -= damageValue;
            if(controller.HP <= 0)
            {
                controller.GetMotion().ChangeMotion(CharacterTag.StateTag.Die);
                controller.DeathFlag = true;
                controller.GetTimer().GetTimerDie().StartTimer(1f);
            }
            damageValue = 0;
            damageFlag = false;
        }
    }

}
