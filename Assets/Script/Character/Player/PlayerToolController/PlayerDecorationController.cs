using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーが持つ道具とは別の装飾品を管理するクラス
/// 現在は剣の鞘のみ
/// </summary>
[System.Serializable]
public class PlayerDecorationController
{
    [SerializeField]
    private PlayerController        controller;
    public void                     SetController(PlayerController _controller) { controller = _controller; }
    public enum DecorationObjectTag
    {
        Null = -1,
        Scabbard,
        DataEnd
    }

    [SerializeField]
    private List<GameObject>        decorationObjects = new List<GameObject>();

    public void Setup(PlayerController c)
    {
        controller = c;
    }

    public void Update()
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
