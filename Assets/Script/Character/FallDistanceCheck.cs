using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDistanceCheck
{
    private PlayerController controller;
    public FallDistanceCheck(PlayerController _controller)
    {
        controller = _controller;
    }
    //　落ちた場所
    private float fallenPosition = 0f;
    //落下してから地面に落ちるまでの距離
    private float fallenDistance = 0f;
    //どのぐらいの高さからダメージを与えるか
    private const float takeDamageDistance = 10f;
    /// <summary>
    /// 落ちてるかどうかのフラグ
    /// </summary>
    private bool fall = false;
    public bool Fall {  get { return fall; } set { fall = value; } }
    /// <summary>
    /// 落下ダメージを与えるかどうかのフラグ
    /// </summary>
    private bool fallDamage = false;
    public bool FallDamage {  get { return fallDamage; } set { fallDamage = value; } }

    public void SetController(PlayerController _controller) {  controller = _controller; }

    public void Initialize()
    {
        fallenDistance = 0f;
        fallenPosition = controller.transform.position.y;
        fall = false;
        fallDamage = false;
    }

    public void Execute()
    {
        if (!controller.Landing)
        {
            // 落下地点と現在地の距離を計算（ジャンプ等で上に飛んで落下した場合を考慮する為の処理）
			fallenPosition = Mathf.Max(fallenPosition, controller.transform.position.y);
        }
        else
        {
            fallenPosition = controller.transform.position.y;
            fallenDistance = 0;
        }
    }

    public void CollisionEnter()
    {
        //　落下距離を計算
        fallenDistance = fallenPosition - controller.transform.position.y;

        //　落下によるダメージが発生する距離を超える場合ダメージを与える
        if (fallenDistance >= takeDamageDistance)
        {
            fallDamage = true;
        }
    }

    public void CollisionExit()
    {
        //　最初の落下地点を設定
        fallenPosition = controller.transform.position.y;
        fallenDistance = 0;
    }
}
