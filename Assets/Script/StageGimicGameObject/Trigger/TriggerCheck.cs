using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーが当たったどうかを調べるためだけのクラス
/// 追加で当たったらプレイヤーの情報を取得する
/// 当たり終わったら情報を削除する
/// </summary>
public class TriggerCheck : MonoBehaviour
{
    [SerializeField]
    private GameSystemController.TriggerTag myTrigger = GameSystemController.TriggerTag.Null;
    [SerializeField]
    private bool hitPlayer = false;
    public bool IsHitPlayer() {  return hitPlayer; }

    private GameObject player;
    public GameObject GetPlayer() { return player; }
    private PlayerController controller;
    public PlayerController GetController() { return controller; }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") { return; }
        hitPlayer = true;
        player = other.gameObject;
        controller = player.GetComponent<PlayerController>();
        GameSystemController.KeyTriggerTag = myTrigger;
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player") { return; }
        hitPlayer = false;
        player = null;
        controller = null;
        GameSystemController.KeyTriggerTag = GameSystemController.TriggerTag.Null;
    }
}
