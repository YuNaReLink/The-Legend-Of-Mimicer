using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGimicController
{
    [SerializeField]
    private static bool actionF = false;
    public static bool ActionF {  get { return actionF; } set { actionF = value; } }
}
