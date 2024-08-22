using System.Collections.Generic;
using UnityEngine;

public class TitleUIController : MonoBehaviour
{
    public enum TitleUINumber
    {
        UIEvent01,
        UIEvent02,
    }

    private List<GameObject> titleSceneArray = new List<GameObject>();

    private List<BaseTitleUIConfiguration> titleUIConfigurations = new List<BaseTitleUIConfiguration>();

    private void Awake()
    {
        int childCount = transform.childCount;
        if (childCount != 0)
        {
            GameObject uiEvent = null;
            for (int i = 0; i < childCount; i++)
            {
                uiEvent = transform.GetChild(i).gameObject;
                titleSceneArray.Add(uiEvent);
                BaseTitleUIConfiguration baseTitleUIConfiguration = uiEvent.GetComponent<BaseTitleUIConfiguration>();
                if (baseTitleUIConfiguration != null)
                {
                    baseTitleUIConfiguration.Initilaize();
                    titleUIConfigurations.Add(baseTitleUIConfiguration);
                }
            }
        }
    }

    void Start()
    {
    }

    private void Update()
    {
        ChangeUIEventKeyInput();

        for(int i = 0;i < titleUIConfigurations.Count; i++)
        {
            titleUIConfigurations[i].ConfigurationUpdate();
        }
    }

    private void ChangeUIEventKeyInput()
    {
        if (titleSceneArray[(int)TitleUINumber.UIEvent01] == null) { return; }
        if (!titleUIConfigurations[(int)TitleUINumber.UIEvent01].IsFadeEnd()) { return; }
        if (!Input.anyKeyDown) { return; }
        if (titleSceneArray[(int)TitleUINumber.UIEvent01].activeSelf)
        {
            titleSceneArray[(int)TitleUINumber.UIEvent01].SetActive(false);
            titleSceneArray[(int)TitleUINumber.UIEvent02].SetActive(true);
        }
    }
}
