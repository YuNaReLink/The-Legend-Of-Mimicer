using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUIController : MonoBehaviour
{
    private List<GameObject> gameOverUIObjectList = new List<GameObject>();

    private FadeIn panelFadeIn = null;

    private GameOverText gameOverText = null;

    private GameOverButtons gameOverButtons = null;

    public void AwakeInitilaize()
    {
        gameOverUIObjectList.Clear();

        panelFadeIn = GetComponentInChildren<FadeIn>();
        gameOverUIObjectList.Add(panelFadeIn.gameObject);

        gameOverText = GetComponentInChildren<GameOverText>();
        if(gameOverText != null)
        {
            gameOverText.AwakeInitilaize();
            gameOverUIObjectList.Add(gameOverText.gameObject);
        }

        gameOverButtons = GetComponentInChildren<GameOverButtons>();
        gameOverUIObjectList.Add(gameOverButtons.gameObject);
    }

    public void StartInitialize()
    {
        gameObject.SetActive(false);
    }


    public void GameOverUIUpdate()
    {
        if (!gameOverText.IsTextFadeEnd())
        {
            gameOverText.GameOverTextUpdate();
        }
        else
        {
            if (!panelFadeIn.IsFadeEnd())
            {
                panelFadeIn.StartFadeIn();
            }
            else
            {
                gameOverButtons.MoveButtons(new Vector2());
            }
        }
    }
}
