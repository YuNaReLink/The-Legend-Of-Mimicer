using UnityEngine;

/// <summary>
/// プレイヤーの落下処理を行うクラス
/// </summary>
public class FallDistanceCheck : InterfaceBaseCommand
{
    //参照用プレイヤーコントローラー
    private PlayerController    controller = null;
    //コンストラクタから取得
    public FallDistanceCheck(PlayerController _controller)
    {
        controller = _controller;
    }
    //　落ちた場所
    private float               fallenPosition = 0f;
    //落下してから地面に落ちるまでの距離
    private float               fallenDistance = 0f;
    //どのぐらいの高さからダメージを与えるか
    private const float         TakeDamageDistance = 10f;
    /// <summary>
    /// 落下ダメージを与えるかどうかのフラグ
    /// </summary>
    private bool                fallDamage = false;
    public bool                 FallDamage  => fallDamage;
    public void                 SetFallDamage(bool flag) {  fallDamage = flag; }

    //初期化
    public void Initialize()
    {
        fallenDistance = 0f;
        fallenPosition = controller.transform.position.y;
        fallDamage = false;
    }
    /// <summary>
    /// 落下の計算を行う関数
    /// </summary>
    public void Execute()
    {
        if (!controller.CharacterStatus.Landing)
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
    /// <summary>
    /// 落下から着地した時に呼び出す関数
    /// </summary>
    public void CollisionEnter()
    {
        //　落下距離を計算
        fallenDistance = fallenPosition - controller.transform.position.y;

        //　落下によるダメージが発生する距離を超える場合ダメージを与える
        if (fallenDistance >= TakeDamageDistance)
        {
            fallDamage = true;
        }
    }
    /// <summary>
    /// 着地フラグがfalseになった時に呼び出す関数
    /// </summary>
    public void CollisionExit()
    {
        //　最初の落下地点を設定
        fallenPosition = controller.transform.position.y;
        fallenDistance = 0;
    }
}
