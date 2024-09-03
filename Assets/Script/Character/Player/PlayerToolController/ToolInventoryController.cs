using System.Collections.Generic;
using UnityEngine;

public class ToolInventoryController : MonoBehaviour
{
    private PlayerController        controller;

    public void SetController(PlayerController _controller) { controller = _controller; }
    public enum ToolObjectTag
    {
        Null = -1,
        Sword,
        Shild,
        CrossBow,
        DataEnd
    }

    private ToolObjectTag           currentToolTag = ToolObjectTag.Null;
    private bool                    currentToolChange = false;
    public ToolObjectTag            CurrentToolTag => currentToolTag;
    public bool                     IsCurrentToolChange => currentToolChange;
    public void ChangeToolTag(ToolObjectTag tooltag)
    {
        currentToolTag = tooltag;
        currentToolChange = true;
    }

    private InventoryData           inventoryData = null;
    public InventoryData GetInventoryData() {  return inventoryData; }

    /// <summary>
    /// 引数の道具が道具リストにあるかチェック
    /// </summary>
    /// <param name="tool"></param>
    /// <returns></returns>
    public bool CheckNullToolObject(GameObject tool)
    {
        if(inventoryData.ToolItemList.Count == 0) {  return true; }
        for(int i = 0; i < inventoryData.ToolItemList.Count; i++)
        {
            if (inventoryData.ToolItemList[i] == null) {  continue; }
            if (inventoryData.ToolItemList[i] == tool) { return false; }
        }
        return true;
    }

    public bool CheckNullTool(ToolObjectTag toolTag) =>
        CheckNullToolObject(inventoryData.ToolItemList[(int)toolTag]);

    /// <summary>
    /// 道具の管理クラスのリスト
    /// </summary>
    [SerializeField]
    private List<ToolController>    toolControllers = new List<ToolController>();

    private Quiver                  quiver = null;
    public Quiver                   GetQuiver() { return quiver; }

    //剣をしまっている時の位置・回転
    [SerializeField]
    private Transform               swordTransform;

    [SerializeField]
    private Transform               crossBowTransform;

    [SerializeField]
    private Vector3                 localCrossBowPos = new Vector3(1, -0.8f, 1);


    //盾を背負う位置・回転
    [SerializeField]
    private Transform               shieldTransform;

    [SerializeField]
    private Transform               rightHandTransform;

    [SerializeField]
    private Transform               leftHandTransform;


    /// <summary>
    /// エフェクト関係の変数
    /// </summary>
    private SwordEffectController   swordEffect = null;

    public void Initilaize()
    {
        inventoryData = GetComponent<InventoryData>();
        InitializeToolSetting();
    }


    public void UpdateTool()
    {
        currentToolChange = false;
        ChangeSwordTransform();
        ChangeCrossBowTransform();
        ChangeShieldTransform();
    }


    /// <summary>
    /// ゲーム開始時、道具を持っていた時に行う処理
    /// </summary>
    public void InitializeToolSetting()
    {
        for (int i = 0; i < inventoryData.ToolItemList.Count; i++)
        {
            if (inventoryData.ToolItemList[i] == null) { continue; }
            toolControllers[i] = inventoryData.ToolItemList[i].GetComponent<ToolController>();
            toolControllers[i].SetController(controller);
        }
    }

    /// <summary>
    /// 途中、道具をゲットした時の処理
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="tool"></param>
    public void GetToolSetting(ToolObjectTag tag,GameObject tool)
    {
        if (inventoryData.ToolItemList[(int)tag] != null) { return; }
        GameObject toolObject = Instantiate(tool);
        //道具Listに登録
        //tools.Add(toolObject);
        inventoryData.ToolItemList[(int)tag] = toolObject;
        //道具のコントローラーを登録
        toolControllers[(int)tag] = toolObject.GetComponent<ToolController>();
        toolControllers[(int)tag].SetController(controller);
        Transform parent = null;
        switch (tag)
        {
            case ToolObjectTag.Sword:
                parent = swordTransform;
                break;
            case ToolObjectTag.CrossBow:
                parent = crossBowTransform;
                gameObject.AddComponent<Quiver>();
                quiver = GetComponent<Quiver>();
                break;
            case ToolObjectTag.Shild:
                parent = shieldTransform;
                break;
        }
        toolObject.transform.SetParent(parent);
        toolObject.transform.position = parent.position;
        toolObject.transform.rotation = parent.rotation;
        toolObject.transform.localScale = parent.localScale;
    }

    //道具の位置を入れ替える処理
    private void SetToolPosition(ToolObjectTag tool,Transform transform)
    {
        int t = (int)tool;
        if (inventoryData.ToolItemList[t].transform.parent == transform) { return; }
        inventoryData.ToolItemList[t].transform.SetParent(null);
        inventoryData.ToolItemList[t].transform.SetParent(transform);
        inventoryData.ToolItemList[t].transform.position = transform.position;
        inventoryData.ToolItemList[t].transform.rotation = transform.rotation;
        inventoryData.ToolItemList[t].transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void ChangeSwordTransform()
    {
        if(inventoryData.ToolItemList[(int)ToolObjectTag.Sword] == null) { return; }
        if (currentToolTag == ToolObjectTag.Sword)
        {
            SetToolPosition(ToolObjectTag.Sword, rightHandTransform);
        }
        else
        {
            SetToolPosition(ToolObjectTag.Sword, swordTransform);
        }
    }

    private void ChangeCrossBowTransform()
    {
        int c = (int)ToolObjectTag.CrossBow;
        if (inventoryData.ToolItemList[c] == null) { return; }
        if (currentToolTag == ToolObjectTag.CrossBow)
        {
            SetToolPosition(ToolObjectTag.CrossBow, controller.GetCameraController().transform);
            inventoryData.ToolItemList[c].transform.localPosition = localCrossBowPos;
        }
        else
        {
            SetToolPosition(ToolObjectTag.CrossBow, crossBowTransform);
        }
    }

    private void ChangeShieldTransform()
    {
        if (inventoryData.ToolItemList[(int)ToolObjectTag.Shild] == null) { return; }
        if (controller.BattleMode||controller.GetKeyInput().GuardHoldButton)
        {
            SetToolPosition(ToolObjectTag.Shild, leftHandTransform);
        }
        else
        {
            SetToolPosition(ToolObjectTag.Shild, shieldTransform);
        }
    }

}
