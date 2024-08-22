using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearUIController : MonoBehaviour
{
    private List<GameObject> gameClearUIObjectList = new List<GameObject>();

    private FadeIn panelFadeIn = null;

    private FadeInText gameClearText = null;

    private MoverButton gameClearButtons = null;

    public void AwakeInitilaize()
    {
        gameClearUIObjectList.Clear();

        panelFadeIn = GetComponentInChildren<FadeIn>();
        gameClearUIObjectList.Add(panelFadeIn.gameObject);

        gameClearText = GetComponentInChildren<FadeInText>();
        if (gameClearText != null)
        {
            gameClearText.AwakeInitilaize();
            gameClearUIObjectList.Add(gameClearText.gameObject);
        }

        gameClearButtons = GetComponentInChildren<MoverButton>();
        gameClearUIObjectList.Add(gameClearButtons.gameObject);
    }

    public void StartInitialize()
    {
        gameObject.SetActive(false);
    }

    public void GameClearUIUpdate()
    {
        if (!gameClearText.IsTextFadeEnd())
        {
            gameClearText.GameOverTextUpdate();
        }
        else
        {
            if (!panelFadeIn.IsFadeEnd())
            {
                panelFadeIn.StartFadeIn();
            }
            else
            {
                gameClearButtons.MoveButtons(new Vector2());
            }
        }
    }
}
