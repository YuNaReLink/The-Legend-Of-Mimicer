using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUIConfiguration01 : BaseTitleUIConfiguration
{
    private enum GUINumber
    {
        Title,
        Button,
        DataEnd
    }

    Image titleImage = null;
    Color titleColor;

    Text  startText = null;
    Color startColor;

    public override void Initilaize()
    {
        base.Initilaize();

        titleImage = uiObjectArray[(int)GUINumber.Title].GetComponent<Image>();
        if(titleImage != null)
        {
            titleColor = titleImage.color;
        }

        startText = uiObjectArray[(int)GUINumber.Button].GetComponent<Text>();
        if(startText != null)
        {
            startColor = startText.color;
        }
    }

    public override void ConfigurationUpdate()
    {
        if (!fadeEnd)
        {
            FadeTitleImage(1f);
            FadeSkip();
        }
        else
        {
            FlashingStartText();
        }
    }

    private void FadeTitleImage(float targetAlpha)
    {
        if(!Mathf.Approximately(titleImage.color.a, targetAlpha))
        {
            float changePerFrame = Time.deltaTime / 2f;
            titleColor.a = Mathf.MoveTowards(titleImage.color.a, targetAlpha, changePerFrame);
            titleImage.color = titleColor;
        }
        else
        {
            FadeStartButtonText(targetAlpha);
        }
    }

    private void FadeStartButtonText(float targetAlpha)
    {
        if (!Mathf.Approximately(startText.color.a, targetAlpha))
        {
            float changePerFrame = Time.deltaTime / 2f;
            startColor.a = Mathf.MoveTowards(startText.color.a, targetAlpha, changePerFrame);
            startText.color = startColor;
        }
        else
        {
            fadeEnd = true;
        }
    }

    private void FadeSkip()
    {
        if (Input.anyKeyDown||Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1)|| Input.GetMouseButtonDown(2))
        {
            fadeEnd = true;
            titleImage.color = new Color(1f, 1f, 1f, 1f);
            startText.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    private void FlashingStartText()
    {
        float alphaSin = FlashCalculate(0.7f);
        startColor.a = alphaSin;
        startText.color = startColor;
    }

    private float FlashCalculate(float interval)
    {
        return Mathf.Sin(Time.time) / 2 + interval;
    }
}
