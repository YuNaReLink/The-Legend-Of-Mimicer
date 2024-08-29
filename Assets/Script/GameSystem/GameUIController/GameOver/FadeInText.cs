using UnityEngine;
using UnityEngine.UI;

public class FadeInText : MonoBehaviour
{
    private Text gameOverText = null;

    private Color textColor;

    [SerializeField]
    private float fadeTime = 2f;

    private bool textFadeEnd = false;
    public bool IsTextFadeEnd() { return textFadeEnd; }
    public GameObject SelfObject() { return gameObject; }
    public bool IsActiveObject() { return gameObject.activeSelf; }
    public void SetActiveObject(bool enabled) { gameObject.SetActive(enabled); }
    public void AwakeInitilaize()
    {
        gameOverText = GetComponent<Text>();
        textColor = gameOverText.color;
        if(textColor != null)
        {
            textColor.a = 0;
        }
    }

    public void StartInitilaize()
    {
        textFadeEnd = false;
    }

    public void GameOverTextUpdate()
    {
        if (textFadeEnd) { return; }
        Color color = gameOverText.color;
        if (!Mathf.Approximately(gameOverText.color.a, 1f))
        {
            float changePerFrame = Time.deltaTime / fadeTime;
            color.a = Mathf.MoveTowards(gameOverText.color.a, 1f, changePerFrame);
            gameOverText.color = color;
        }
        else
        {
            textFadeEnd = true;
        }
    }


}
