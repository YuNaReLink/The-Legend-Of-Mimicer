using System.Collections.Generic;
using UnityEngine;

public class GameClearUIController : MonoBehaviour
{
    private List<GameObject>        gameClearUIObjectList = new List<GameObject>();

    private FadeIn                  panelFadeIn = null;

    private FadeInText              gameClearText = null;

    private MoverButton             gameClearButtons = null;

    public GameObject               SelfObject() { return gameObject; }

    public bool                     IsActiveObject() { return gameObject.activeSelf; }

    public void                     SetActiveObject(bool enabled) { gameObject.SetActive(enabled); }

    public void AwakeInitilaize()
    {
        gameClearUIObjectList.Clear();

        panelFadeIn = GetComponentInChildren<FadeIn>();
        if (panelFadeIn != null)
        {
            panelFadeIn.AwakeInitilaize();
            gameClearUIObjectList.Add(panelFadeIn.SelfObject());
        }

        gameClearText = GetComponentInChildren<FadeInText>();
        if (gameClearText != null)
        {
            gameClearText.AwakeInitilaize();
            gameClearUIObjectList.Add(gameClearText.SelfObject());
        }

        gameClearButtons = GetComponentInChildren<MoverButton>();
        if(gameClearButtons != null)
        {
            gameClearButtons.AwakeInitilaize();
            gameClearUIObjectList.Add(gameClearButtons.SelfObject());
        }
    }

    public void StartInitialize()
    {
        panelFadeIn.StartInitilaize();
        gameClearText.StartInitilaize();
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
