using System.Collections.Generic;
using UnityEngine;

public class GameOverUIController : MonoBehaviour
{
    private List<GameObject>        gameOverUIObjectList = new List<GameObject>();

    private FadeIn                  panelFadeIn = null;

    private FadeInText              gameOverText = null;

    private MoverButton             gameOverButtons = null;

    public GameObject               SelfObject() { return gameObject; }
    public bool                     IsActiveObject() { return gameObject.activeSelf; }

    public void                     SetActiveObject(bool enabled) {  gameObject.SetActive(enabled); }

    public void AwakeInitilaize()
    {
        gameOverUIObjectList.Clear();

        panelFadeIn = GetComponentInChildren<FadeIn>();
        if(panelFadeIn != null)
        {
            panelFadeIn.AwakeInitilaize();
            gameOverUIObjectList.Add(panelFadeIn.SelfObject());
        }

        gameOverText = GetComponentInChildren<FadeInText>();
        if(gameOverText != null)
        {
            gameOverText.AwakeInitilaize();
            gameOverUIObjectList.Add(gameOverText.SelfObject());
        }

        gameOverButtons = GetComponentInChildren<MoverButton>();
        if(gameOverButtons != null)
        {
            gameOverButtons.AwakeInitilaize();
            gameOverUIObjectList.Add(gameOverButtons.SelfObject());
        }
    }

    public void StartInitialize()
    {
        panelFadeIn.StartInitilaize();
        gameOverText.StartInitilaize();
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
