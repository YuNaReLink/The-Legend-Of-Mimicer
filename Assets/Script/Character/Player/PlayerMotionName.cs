using UnityEngine;

public class PlayerMotionName
{
    private string[] rollingName = new string[]
    {
        "rolling",
        "backFlip",
        "leftRolling",
        "rightRolling"
    };

    public string[] GetRollingName() {  return rollingName; }

    private string[] motionName = new string[]
    {
        "idle",
        "weaponIdle",
        "getUp",
        "attack1",
        "attack2",
        "attack3",
        "jumpAttack",
        "spinAttack",
        "storing",
        "posture",
        "DamageLanding",
        "damageImpact"
    };

    public string[] GetMotionName() { return motionName; }

    private string[] guardEnabledMotionName = new string[]
    {
        "idle",
        "weaponIdle",
        "forwardRun",
        "backRun",
        "leftStrafe",
        "rightStrafe"
    };

    public string[] GetGuardEnabledMotionName() {return guardEnabledMotionName; }
}
