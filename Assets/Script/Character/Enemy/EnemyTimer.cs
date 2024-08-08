using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTimer : TimerController
{
    private DeltaTimeCountDown timerIdle = null;
    public DeltaTimeCountDown GetTimerIdle() {  return timerIdle; }
    private DeltaTimeCountDown timerAttackCoolDown = null;
    public DeltaTimeCountDown GetTimerAttackCoolDown() {  return timerAttackCoolDown; }
    private DeltaTimeCountDown timerDamageCoolDown = null;
    public DeltaTimeCountDown GetTimerDamageCoolDown() { return timerDamageCoolDown; }
    private DeltaTimeCountDown timerDie = null;
    public DeltaTimeCountDown GetTimerDie() { return timerDie; }

    public override void InitializeAssignTimer()
    {
        timerIdle = new DeltaTimeCountDown();
        timerAttackCoolDown = new DeltaTimeCountDown();
        timerDamageCoolDown = new DeltaTimeCountDown();
        timerDie = new DeltaTimeCountDown();

        updateCountDowns = new InterfaceCountDown[]
        {
            timerIdle,
            timerAttackCoolDown,
            timerDamageCoolDown,
            timerDie,
        };
    }
}
