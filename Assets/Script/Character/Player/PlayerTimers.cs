
//プレイヤーで使うタイマーを使用する分だけ記述する
public class PlayerTimers : TimerController
{

    private DeltaTimeCountDown timerRolling;
    public DeltaTimeCountDown GetTimerRolling() { return timerRolling; }

    private DeltaTimeCountDown timerNoAccele;
    public DeltaTimeCountDown GetTimerNoAccele() { return timerNoAccele; }

    private DeltaTimeCountDown timerWallActionStop;
    public DeltaTimeCountDown GetTimerWallActionStop() { return timerWallActionStop; }

    private DeltaTimeCountDown timerJumpAttackAccele;
    public DeltaTimeCountDown GetTimerJumpAttackAccele() { return timerJumpAttackAccele; }

    public override void InitializeAssignTimer()
    {
        timerRolling = new DeltaTimeCountDown();
        timerNoAccele = new DeltaTimeCountDown();
        timerWallActionStop = new DeltaTimeCountDown();
        timerJumpAttackAccele = new DeltaTimeCountDown();

        updateCountDowns = new InterfaceCountDown[]
        {
            timerRolling,
            timerNoAccele,
            timerWallActionStop,
            timerJumpAttackAccele,
        };
    }
}
