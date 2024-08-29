
/// <summary>
/// �G�Ŏg���^�C�}�[���܂Ƃ߂ď�������N���X
/// </summary>
public class EnemyTimer : TimerController
{
    //�ҋ@�^�C�}�[
    private DeltaTimeCountDown      timerIdle = null;
    public DeltaTimeCountDown       GetTimerIdle() {  return timerIdle; }
    //�U����̃N�[���_�E��
    private DeltaTimeCountDown      timerAttackCoolDown = null;
    public DeltaTimeCountDown       GetTimerAttackCoolDown() {  return timerAttackCoolDown; }
    //�_���[�W������̃N�[���_�E��
    private DeltaTimeCountDown      timerDamageCoolDown = null;
    public DeltaTimeCountDown       GetTimerDamageCoolDown() { return timerDamageCoolDown; }
    //���ݏ�Ԃ��ێ�����^�C�}�[(�g�p:Boss)
    private DeltaTimeCountDown      timerStun = null;
    public DeltaTimeCountDown       GetTimerStun() { return timerStun; }
    //�G��HP��0�ɂȂ��Ă��班���Ԃ��J����Destroy���G�t�F�N�g�𔭐������邽�߂̃^�C�}�[
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
