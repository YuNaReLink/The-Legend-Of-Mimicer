using System.Collections.Generic;
using UnityEngine;

public class PlayerToolController : MonoBehaviour
{
    [SerializeField]
    private PlayerController controller;

    public void SetController(PlayerController _controller) { controller = _controller; }
    public enum ToolObjectTag
    {
        Null = -1,
        Sword,
        Shild,
        Bow,
        DataEnd
    }

    [SerializeField]
    private ToolObjectTag currentToolTag = ToolObjectTag.Null;
    public ToolObjectTag CurrentToolTag { get{ return currentToolTag; }set { currentToolTag = value; } }

    [SerializeField]
    private List<GameObject> tools = new List<GameObject>();
    public List<GameObject> Tools { get { return tools; } }

    /// <summary>
    /// 引数の道具が道具リストにあるかチェック
    /// </summary>
    /// <param name="tool"></param>
    /// <returns></returns>
    public bool CheckNullToolObject(GameObject tool)
    {
        if(tools.Count == 0) {  return true; }
        for(int i = 0; i < tools.Count; i++)
        {
            if (tools[i] == null) {  continue; }
            if (tools[i] == tool)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 道具の管理クラスのリスト
    /// </summary>
    [SerializeField]
    private List<ToolController> toolControllers = new List<ToolController>();
    public List<ToolController> ToolControllers { get { return toolControllers; } set { toolControllers = value; } }

    //剣をしまっている時の位置・回転
    [SerializeField]
    private Transform swordTransform;

    [SerializeField]
    private Transform crossBowTransform;

    [SerializeField]
    private Vector3 localCrossBowPos = new Vector3(1, -0.8f, 1);


    //盾を背負う位置・回転
    [SerializeField]
    private Transform shieldTransform;

    [SerializeField]
    private Transform rightHandTransform;

    [SerializeField]
    private Transform leftHandTransform;


    /// <summary>
    /// エフェクト関係の変数
    /// </summary>
    private SwordEffectController swordEffect = null;

    public void Initilaize()
    {
        InitializeToolSetting();
    }


    public void UpdateProps()
    {
        ChangeSwordTransform();
        ChangeCrossBowTransform();
        ChangeShieldTransform();
    }


    /// <summary>
    /// ゲーム開始時、道具を持っていた時に行う処理
    /// </summary>
    public void InitializeToolSetting()
    {
        for (int i = 0; i < tools.Count; i++)
        {
            if (tools[i] == null) { continue; }
            toolControllers[i] = tools[i].GetComponent<ToolController>();
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
        if (tools[(int)tag] != null) { return; }
        GameObject toolObject = Instantiate(tool);
        //道具Listに登録
        //tools.Add(toolObject);
        tools[(int)tag] = toolObject;
        //道具のコントローラーを登録
        toolControllers[(int)tag] = toolObject.GetComponent<ToolController>();
        toolControllers[(int)tag].SetController(controller);
        Transform parent = null;
        switch (tag)
        {
            case ToolObjectTag.Sword:
                parent = swordTransform;
                break;
            case ToolObjectTag.Bow:
                parent = crossBowTransform;
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
        if (tools[(int)tool].transform.parent == transform) { return; }
        tools[(int)tool].transform.SetParent(null);
        tools[(int)tool].transform.SetParent(transform);
        tools[(int)tool].transform.position = transform.position;
        tools[(int)tool].transform.rotation = transform.rotation;
        tools[(int)tool].transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void ChangeSwordTransform()
    {
        if(tools[(int)ToolObjectTag.Sword] == null) { return; }
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
        if (tools[(int)ToolObjectTag.Bow] == null) { return; }
        if (currentToolTag == ToolObjectTag.Bow)
        {
            SetToolPosition(ToolObjectTag.Bow, controller.GetCameraObject().transform);
            tools[(int)ToolObjectTag.Bow].transform.localPosition = localCrossBowPos;
        }
        else
        {
            SetToolPosition(ToolObjectTag.Bow, crossBowTransform);
        }
    }

    private void ChangeShieldTransform()
    {
        if (tools[(int)ToolObjectTag.Shild] == null) { return; }
        if (controller.BattleMode||controller.GetKeyInput().RightMouseClick)
        {
            SetToolPosition(ToolObjectTag.Shild, leftHandTransform);
        }
        else
        {
            SetToolPosition(ToolObjectTag.Shild, shieldTransform);
        }
    }

}
