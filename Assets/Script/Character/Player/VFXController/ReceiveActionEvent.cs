using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveActionEvent : MonoBehaviour
{
    [SerializeField]
    private CreateSwordTrail swordTrail;

    public void StartSwordTrail()
    {
        swordTrail.SetSwordTrail(true);
    }

    public void EndSwordTrail()
    {
        swordTrail.SetSwordTrail(false);
    }
}
