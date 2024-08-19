using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDecorationController : MonoBehaviour
{
    [SerializeField]
    private PlayerController controller;
    public void SetController(PlayerController _controller) { controller = _controller; }
    public enum DecorationObjectTag
    {
        Null = -1,
        Scabbard,
        DataEnd
    }

    [SerializeField]
    private List<GameObject> decorationObjects = new List<GameObject>();
    public List<GameObject> GetDecorationObjects() {  return decorationObjects; }

    void Update()
    {
        SetActiveObject(ToolInventoryController.ToolObjectTag.Sword, DecorationObjectTag.Scabbard);
    }

    private void SetActiveObject(ToolInventoryController.ToolObjectTag tootag, DecorationObjectTag decotag)
    {
        if (controller.GetToolController().GetInventoryData().ToolItemList[(int)tootag] == null)
        {
            if(decorationObjects[(int)decotag].activeSelf == false) { return; }
            decorationObjects[(int)decotag].SetActive(false);
        }
        else
        {
            if(decorationObjects[(int)decotag].activeSelf == true) { return; }
            decorationObjects[(int)decotag].SetActive(true);
        }
    }
}
