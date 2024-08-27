using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObjectResetPosition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") { return; }
        ResetPosition(other);
    }

    private void ResetPosition(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        controller.GetKeyInput().GetUpInput();
        controller.CharacterRB.velocity = controller.StopMoveVelocity();
        controller.Velocity = controller.StopMoveVelocity();
        other.transform.position = controller.GetLandingPosition();
    }
}
