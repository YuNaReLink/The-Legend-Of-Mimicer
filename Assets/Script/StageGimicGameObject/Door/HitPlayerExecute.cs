using UnityEngine;


/// <summary>
/// ステージ上のプレイヤーの情報が必要なギミックオブジェクトにアタッチし
/// プレイヤーがそのギミックオブジェクトに触れているか判別、情報を取得するクラス
/// </summary>
public class HitPlayerExecute : MonoBehaviour
{
    [SerializeField]
    private bool playerHit = false;

    public bool PlayerHit { get { return playerHit; } set { playerHit = value; } }

    public void OnTriggerEnter(Collider other)
    {
        playerHit = false;
        if (other.tag != "Player") { return; }
        playerHit = true;
    }
}
