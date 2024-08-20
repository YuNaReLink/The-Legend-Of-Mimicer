using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    /// <summary>
    /// 蓋のオブジェクトの入れ物
    /// </summary>
    [SerializeField]
    private GameObject lidObject = null;
    /// <summary>
    /// 初期の角度を保存
    /// </summary>
    [SerializeField]
    private Vector3 baseRotate = Vector3.zero;
    /// <summary>
    /// 開く角度を保存するもの
    /// </summary>
    [SerializeField]
    private Vector3 openRotate = Vector3.zero;
    /// <summary>
    /// 宝箱が開く回転角度X
    /// </summary>
    [SerializeField]
    private float openRotateX = -100;
    /// <summary>
    /// 宝箱の開くスピード
    /// </summary>
    [SerializeField]
    private float openSpeed = 5f;
    /// <summary>
    /// プレイヤーが宝箱周辺の当たり判定に当たっているか調べるためのクラス
    /// </summary>
    [SerializeField]
    private TriggerCheck triggerCheck = null;
    /// <summary>
    /// 宝箱を開くためのフラグ
    /// </summary>
    [SerializeField]
    private bool open = false;
    /// <summary>
    /// 宝箱を開き終わった時のフラグ
    /// </summary>
    [SerializeField]
    private bool stop = false;
    /// <summary>
    /// アイテムをプレイヤーに取得させるクラス
    /// </summary>
    [SerializeField]
    private GetChestItem getItem = null;
    private void Start()
    {
        triggerCheck = GetComponentInChildren<TriggerCheck>();

        getItem = GetComponent<GetChestItem>();

        open = false;
        stop = false;

        baseRotate = lidObject.transform.localRotation.eulerAngles;
        openRotate = baseRotate;
        openRotate.x = openRotateX;
    }

    private void Update()
    {
        if (!triggerCheck.IsHitPlayer()) { return; }
        if (stop) { return; }
        OpenInput();
        OpenCover();
    }

    private void OpenInput()
    {
        if (open) { return; }
        if (!triggerCheck.GetController().GetKeyInput().IsGetButton()) { return; }
        open = true;
    }

    private void OpenCover()
    {
        if (!open) { return; }
        if(lidObject == null) { return; }
        Vector3 sub = openRotate - baseRotate;
        Vector3 normal = sub.normalized;
        Vector3 addRotate = lidObject.transform.localRotation.eulerAngles;
        addRotate += normal * openSpeed;
        lidObject.transform.localRotation = Quaternion.Euler(addRotate);
        float currentRotate = lidObject.transform.localRotation.eulerAngles.x;
        currentRotate -= 360f;
        if(currentRotate <= openRotateX + 10f)
        {
            Vector3 rotate = Vector3.zero;
            rotate.x = openRotateX;
            lidObject.transform.localRotation = Quaternion.Euler(rotate);
            stop = true;
            //蓋が空き切った時にアイテムクラスでプレイヤーに指定されたアイテムを取得させる
            getItem.Get();
        }
    }
}
