using UnityEngine;
using UnityEngine.UI;

public class ArrowCount : MonoBehaviour
{
    private PlayerController playerController;

    private Text arrowText = null;

    private int uiArrowCount = 20;

    public void AwakeInitilaize(PlayerController _playerController)
    {
        playerController = _playerController;
    }

    public void ArrowCountUpdate()
    {
        if (playerController.GetToolController().GetInventoryData().ToolItemList[(int)ToolInventoryController.ToolObjectTag.CrossBow] == null) { return; }
        if(playerController.GetToolController().GetQuiver() == null) { return; }
        if (arrowText == null)
        {
            arrowText = GetComponent<Text>();
            arrowText.text = playerController.GetToolController().GetQuiver().Count.ToString();
        }
        else
        {
            arrowText.text = playerController.GetToolController().GetQuiver().Count.ToString();
        }
    }
}
