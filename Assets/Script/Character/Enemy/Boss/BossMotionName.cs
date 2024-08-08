using UnityEngine;

public class BossMotionName
{
    private string[] motionName = new string[]
    {
        "Idle",
        "walk",
        "stampAttack",
        "guard",
        "stun",
        "returnUp"
    };

    public string[] GetMotionName() {  return motionName; }
}
