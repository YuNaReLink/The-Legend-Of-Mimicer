using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    /// <summary>
    /// ゲートオブジェクトの変数
    /// </summary>
    [SerializeField]
    private GameObject          gateGameObject = null;
    /// <summary>
    /// 最初のゲートの位置を保存する変数
    /// </summary>
    [SerializeField]
    private Vector3             baseGatePosition = Vector3.zero;
    /// <summary>
    /// ゲートの移動先位置を保存する変数
    /// </summary>
    [SerializeField]
    private Vector3             goleGatePosition = Vector3.zero;
    /// <summary>
    /// 移動する速度
    /// </summary>
    [SerializeField]
    private float               golePositionY = 10f;
    /// <summary>
    /// 扉を開くためのスイッチクラスを格納するリスト
    /// </summary>
    [SerializeField]
    private List<SwitchGimic>   switchGimics = new List<SwitchGimic>();
    /// <summary>
    /// 扉を開けるためのフラグ
    /// </summary>
    [SerializeField]
    private bool                openGate = false;
    /// <summary>
    /// 扉の開く速度
    /// </summary>
    [SerializeField]
    private float               openSpeed = 5f;
    /// <summary>
    /// 扉の効果音を管理するクラス
    /// </summary>
    private SoundController     soundController = null;

    private void Awake()
    {
        soundController = GetComponent<SoundController>();
        
        if(soundController == null)
        {
            Debug.LogError("SoundControllerがアタッチされていません");
        }
        else
        {
            soundController.AwakeInitilaize();
        }
    }

    private void Start()
    {
        for(int i = 0;i < transform.childCount; i++)
        {
            SwitchGimic switchGimic = transform.GetChild(i).GetComponent<SwitchGimic>();
            if(switchGimic == null) { continue; }
            switchGimics.Add(switchGimic);
        }
        Vector3 baseGate = gateGameObject.transform.position;
        goleGatePosition = new Vector3(baseGate.x, baseGate.y + golePositionY, baseGate.z);
    }


    private void Update()
    {
        //ゲートを開くためのスイッチが全てONになってるか確認
        CheckSwitch();
        //ゲートを開ける処理
        Open();
    }
    //ゲートを開くためのスイッチが全てONになってるか確認
    private void CheckSwitch()
    {
        if (openGate) { return; }
        int truecount = 0;
        for (int i = 0; i < switchGimics.Count; i++)
        {
            if (switchGimics[i].IsSwitchFlag())
            {
                truecount++;
            }
        }

        if (truecount < switchGimics.Count) { return; }
        openGate = true;
        soundController.PlaySESound((int)SoundTagList.OpenDoorSoundTag.Open);
    }
    //ゲートを開ける処理
    private void Open()
    {
        if (!openGate) { return; }
        gateGameObject.transform.position = Vector3.Lerp(gateGameObject.transform.position,goleGatePosition, Time.deltaTime * openSpeed);
    }
}
