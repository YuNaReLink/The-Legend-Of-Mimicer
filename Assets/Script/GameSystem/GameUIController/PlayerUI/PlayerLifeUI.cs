using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����
/// �n�[�g��0.25f�`�����l�̊Ԃ����������Ȃ�
/// </summary>
public class PlayerLifeUI : MonoBehaviour
{
    /// <summary>
    /// �n�[�g�̉摜�z��̗v�f���Ɏg��enum
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
    /// �n�[�g�̉摜�z��
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
            //�n�[�g�I�u�W�F�N�g�𐶐�
            heart = Instantiate(heartObject,parentTransform);
            heartArray.Add(heart);
            //�I�u�W�F�N�g����Image�N���X���A�^�b�`
            heartimage = heart.GetComponent<Image>();
            //Image�N���X�������
            if(heartimage != null)
            {
                heartimage.sprite = heartSprites[(int)HeartState.Full];
                //���X�g�ɒǉ�
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
            //�����Ɛ��l�������Ȃ烊�^�[��
            if(heartNum == hp) { return; }
            //�Q�Ɛ��l��͈͓��ɐݒ�
            heartNum = Mathf.Clamp(hp, 0, uiController.GetPlayerController().CharacterStatus.GetMaxHP());
            //HP�\����ύX
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
                //�t���n�[�g�摜���Z�b�g
                heartImages[i].sprite = heartSprites[(int)HeartState.Full]; // full heart
                tempHealth -= 1f;
            }
            else if (tempHealth >= 0.75f)
            {
                //3/4�n�[�g�摜���Z�b�g
                heartImages[i].sprite = heartSprites[(int)HeartState.Three_Quarters]; // 3/4 heart
                tempHealth -= 0.75f;
            }
            else if (tempHealth >= 0.5f)
            {
                //1/2�n�[�g�摜���Z�b�g
                heartImages[i].sprite = heartSprites[(int)HeartState.Half]; // 2/4 heart
                tempHealth -= 0.5f;
            }
            else if (tempHealth >= 0.25f)
            {
                //1/4�n�[�g�摜���Z�b�g
                heartImages[i].sprite = heartSprites[(int)HeartState.A_Quarter]; // 1/4 heart
                tempHealth -= 0.25f;
            }
            else
            {
                //��n�[�g�摜���Z�b�g
                heartImages[i].sprite = heartSprites[(int)HeartState.Empty]; // empty heart
            }
        }
    }
}
