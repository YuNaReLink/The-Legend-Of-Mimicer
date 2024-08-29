
/// <summary>
/// 敵で使うタイマーをまとめて処理するクラス
/// </summary>
public class EnemyTimer : TimerController
{
    //待機タイマー
    private DeltaTimeCountDown      timerIdle = null;
    public DeltaTimeCountDown       GetTimerIdle() {  return timerIdle; }
    //攻撃後のクールダウン
    private DeltaTimeCountDown      timerAttackCoolDown = null;
    public DeltaTimeCountDown       GetTimerAttackCoolDown() {  return timerAttackCoolDown; }
    //ダメージ発生後のクールダウン
    private DeltaTimeCountDown      timerDamageCoolDown = null;
    public DeltaTimeCountDown       GetTimerDamageCoolDown() { return timerDamageCoolDown; }
    //怯み状態を維持するタイマー(使用:Boss)
    private DeltaTimeCountDown      timerStun = null;
    public DeltaTimeCountDown       GetTimerStun() { return timerStun; }
    //敵のHPが0になってから少し間を開けてDestroy＆エフェクトを発生させるためのタイマー
    private DeltaTimeCountDown      timerDie = null;
    public DeltaTimeCountDown       GetTimerDie() { return timerDie; }

    public override void InitializeAssignTimer()
    {
        timerIdle =             new DeltaTimeCountDown();
        timerAttackCoolDown =   new DeltaTimeCountDown();
        timerDamageCoolDown =   new DeltaTimeCountDown();
        timerStun =             new DeltaTimeCountDown();
        timerDie =              new DeltaTimeCountDown();

        updateCountDowns = new InterfaceCountDown[]
        {
            timerIdle,
            timerAttackCoolDown,
            timerDamageCoolDown,
            timerStun,
            timerDie,
        };
    }
}
