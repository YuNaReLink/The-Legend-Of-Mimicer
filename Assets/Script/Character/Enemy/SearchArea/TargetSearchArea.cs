using UnityEngine;

public class TargetSearchArea : MonoBehaviour
{
    [SerializeField]
    private EnemyController controller = null;
    public float angle = 130f;

    private void Awake()
    {
        controller = GetComponentInParent<EnemyController>();
    }

    private void OnTriggerStay(Collider other)
    {
        //‹ŠE‚Ì”ÍˆÍ“à‚Ì“–‚½‚è”»’è
        if (other.gameObject.tag != "Player") { return; }
        //‹ŠE‚ÌŠp“x“à‚Éû‚Ü‚Á‚Ä‚¢‚é‚©
        Vector3 posDelta = other.transform.position - transform.position;
        float target_angle = Vector3.Angle(transform.forward, posDelta);
        //target_angle‚ªangle‚Éû‚Ü‚Á‚Ä‚¢‚é‚©‚Ç‚¤‚©
        if (target_angle < angle) 
        {
            if (Physics.Raycast(transform.position, posDelta, out RaycastHit hit)) //Ray‚ğg—p‚µ‚Ätarget‚É“–‚½‚Á‚Ä‚¢‚é‚©”»•Ê
            {
                if (hit.collider == other)
                {
                    if (controller.Target == null)
                    {
                        controller.Target = other.GetComponent<PlayerController>();
                    }
                    Debug.Log("range of view");
                }
                else
                {
                    controller.Target = null;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController == null) { return; }
        //“G‚Ìó‘Ô‚ğ•ÏX
        controller.Target = null;
    }
}
