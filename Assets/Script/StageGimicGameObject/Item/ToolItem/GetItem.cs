using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// アイテムを取得するクラス
/// </summary>
public class GetItem : MonoBehaviour
{
    [SerializeField]
    protected GameObject item = null;

    [SerializeField]
    protected PlayerToolController.ToolObjectTag toolTag = PlayerToolController.ToolObjectTag.Null;

    [SerializeField]
    protected PlayerController controller = null;

    [SerializeField]
    private bool getFlag = false;

    private void Update()
    {
        PassingAnItem();
    }

    private void PassingAnItem()
    {
        if (!getFlag) { return; }
        if(controller == null) { return; }
        if(item == null) { return; }
        if (!InputManager.PushFKey()) { return; }
        Get();
    }

    public virtual void Get()
    {
        PlayerToolController tool = controller.GetToolController();
        tool.GetToolSetting(toolTag, item);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") { return; }
        controller = other.GetComponent<PlayerController>();
        getFlag = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player") { return; }
        controller = null;
        getFlag = false;
    }
}
