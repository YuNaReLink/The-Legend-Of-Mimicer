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
    private GameSceneSystemController.TriggerTag myTrigger = GameSceneSystemController.TriggerTag.Null;
    [SerializeField]
    private bool hitPlayer = false;
    public bool IsHitPlayer() {  return hitPlayer; }

    private GameObject player;
    public GameObject GetPlayer() { return player; }
    private PlayerController controller;
    public PlayerController GetController() { return controller; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") { return; }
        hitPlayer = true;
        player = other.gameObject;
        controller = player.GetComponent<PlayerController>();
        GameSceneSystemController.KeyTriggerTag = myTrigger;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player") { return; }
        hitPlayer = false;
        player = null;
        controller = null;
        GameSceneSystemController.KeyTriggerTag = GameSceneSystemController.TriggerTag.Null;
    }

    private void OnDestroy()
    {
        hitPlayer = false;
        player = null;
        controller = null;
        GameSceneSystemController.KeyTriggerTag = GameSceneSystemController.TriggerTag.Null;
    }
}
