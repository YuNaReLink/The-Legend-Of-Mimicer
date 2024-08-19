using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverButtons : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private float moveTime = 10f;

    public void MoveButtons(Vector2 pos)
    {
        if (rectTransform == null) { return; }
        float changePerFrame = Time.deltaTime * moveTime;
        rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, pos, changePerFrame);
    }
}
