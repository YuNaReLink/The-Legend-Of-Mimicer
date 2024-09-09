using UnityEngine;

public class MoverButton : MonoBehaviour
{
    [SerializeField]
    private RectTransform       rectTransform = null;

    [SerializeField]
    private float               moveTime = 10f;

    public GameObject           SelfObject() { return gameObject; }

    public void AwakeInitilaize()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void MoveButtons(Vector2 pos)
    {
        if (rectTransform == null) { return; }
        float changePerFrame = Time.deltaTime * moveTime;
        rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, pos, changePerFrame);
    }
}
