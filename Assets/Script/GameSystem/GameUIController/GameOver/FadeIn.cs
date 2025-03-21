using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField]
    private Image       panel = null;

    [SerializeField]
    private float       fadeTime = 2f;
    private bool        fadeEnd = false;
    public bool         IsFadeEnd() { return fadeEnd; }
    public GameObject   SelfObject() { return gameObject; }
    public bool         IsActiveObject() { return gameObject.activeSelf; }

    public void         SetActiveObject(bool enabled) { gameObject.SetActive(enabled); }

    public void AwakeInitilaize()
    {
        panel = GetComponent<Image>();
    }

    public void StartInitilaize()
    {
        fadeEnd = false;
    }

    public void StartFadeIn()
    {
        if (fadeEnd) { return;}
        if(panel == null) { return; }
        Color panelColor = panel.color;
        if(!Mathf.Approximately(panel.color.a, 1f))
        {
            float changePerFrame = Time.deltaTime / fadeTime;
            panelColor.a = Mathf.MoveTowards(panel.color.a, 1f,changePerFrame);
            panel.color = panelColor;
        }
        else
        {
            fadeEnd = true;
        }
    }
}
