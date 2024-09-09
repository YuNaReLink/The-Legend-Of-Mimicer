using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 原則
/// ハートは0.25f～整数値の間しか増減しない
/// </summary>
public class PlayerLifeUI : MonoBehaviour
{
    /// <summary>
    /// ハートの画像配列の要素数に使うenum
    /// </summary>
    public enum HeartState
    {
        Empty,
        Full,
        Three_Quarters,
        Half,
        A_Quarter,
    }
    /// <summary>
    /// ハートの画像配列
    /// </summary>
    [SerializeField]
    private Sprite[]            heartSprites = new Sprite[5];

    private static float        heartNum = 0;
    public static float         GetHeartNum() {  return heartNum; }

    [SerializeField]
    private GameObject          heartObject = null;
    [SerializeField]
    private List<GameObject>    heartArray = new List<GameObject>();
    private List<Image>         heartImages = new List<Image>();
    [SerializeField]
    private Transform           parentTransform = null;

    private PlayerConnectionUI  uiController = null;

    public void Initilaize(PlayerConnectionUI playerConnectionUI)
    {
        uiController = playerConnectionUI;
    }

    private void HeartInitialize(float hp)
    {
        InitilaizeAddHeart(hp);
        heartNum = hp;
    }

    private void InitilaizeAddHeart(float hp)
    {
        GameObject heart = null;
        Image heartimage = null;
        for (int i = 1;i < hp+1; i++)
        {
            //ハートオブジェクトを生成
            heart = Instantiate(heartObject,parentTransform);
            heartArray.Add(heart);
            //オブジェクトからImageクラスをアタッチ
            heartimage = heart.GetComponent<Image>();
            //Imageクラスがあれば
            if(heartimage != null)
            {
                heartimage.sprite = heartSprites[(int)HeartState.Full];
                //リストに追加
                heartImages.Add(heartimage);
            }
            RectTransform rect = heart.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(50 * i, -50);
        }
    }


    public void LifeUpdate(float hp)
    {
        if(heartArray.Count <= 0)
        {
            HeartInitialize(hp);
        }
        else
        {
            //引数と数値が同じならリターン
            if(heartNum == hp) { return; }
            //参照数値を範囲内に設定
            heartNum = Mathf.Clamp(hp, 0, uiController.GetPlayerController().CharacterStatus.GetMaxHP());
            //HP表示を変更
            ChangeLife();
        }

    }

    private void ChangeLife()
    {
        float tempHealth = heartNum;
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (tempHealth >= 1f)
            {
                //フルハート画像をセット
                heartImages[i].sprite = heartSprites[(int)HeartState.Full]; // full heart
                tempHealth -= 1f;
            }
            else if (tempHealth >= 0.75f)
            {
                //3/4ハート画像をセット
                heartImages[i].sprite = heartSprites[(int)HeartState.Three_Quarters]; // 3/4 heart
                tempHealth -= 0.75f;
            }
            else if (tempHealth >= 0.5f)
            {
                //1/2ハート画像をセット
                heartImages[i].sprite = heartSprites[(int)HeartState.Half]; // 2/4 heart
                tempHealth -= 0.5f;
            }
            else if (tempHealth >= 0.25f)
            {
                //1/4ハート画像をセット
                heartImages[i].sprite = heartSprites[(int)HeartState.A_Quarter]; // 1/4 heart
                tempHealth -= 0.25f;
            }
            else
            {
                //空ハート画像をセット
                heartImages[i].sprite = heartSprites[(int)HeartState.Empty]; // empty heart
            }
        }
    }
}
