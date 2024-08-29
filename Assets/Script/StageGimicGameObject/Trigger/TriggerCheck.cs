using UnityEngine;

/// <summary>
/// �v���C���[�����������ǂ����𒲂ׂ邽�߂����̃N���X
/// �ǉ��œ���������v���C���[�̏����擾����
/// ������I�����������폜����
/// </summary>
public class TriggerCheck : MonoBehaviour
{
    [SerializeField]
    private GameSceneSystemController.TriggerTag    myTrigger = GameSceneSystemController.TriggerTag.Null;
    public void SetTriggerTag(bool enabled) 
    {
        if (enabled)
        {
            GameSceneSystemController.Instance.KeyTriggerTag = myTrigger;
        }
        else
        {
            GameSceneSystemController.Instance.KeyTriggerTag = GameSceneSystemController.TriggerTag.Null;
        }
    }
    [SerializeField]
    private bool                                    hide = false;
    public void                                     SetHide(bool enabled) { hide = enabled; }

    [SerializeField]
    private bool                                    hitPlayer = false;
    public bool                                     IsHitPlayer() {  return hitPlayer; }

    private GameObject                              player = null;
    public GameObject                               GetPlayer() { return player; }
    private PlayerController                        controller = null;
    public PlayerController                         GetController() { return controller; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") { return; }
        if (hide) { return; }
        hitPlayer = true;
        player = other.gameObject;
        controller = player.GetComponent<PlayerController>();
        SetTriggerTag(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player") { return; }
        hitPlayer = false;
        player = null;
        controller = null;
        SetTriggerTag(false);
    }

    private void OnDestroy()
    {
        hitPlayer = false;
        player = null;
        controller = null;
        SetTriggerTag(false);
    }
}
