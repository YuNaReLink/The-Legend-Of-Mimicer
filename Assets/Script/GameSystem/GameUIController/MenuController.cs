using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject menuImage = null;


    private void Awake()
    {
        menuImage = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        if (menuImage.activeSelf)
        {
            menuImage.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.MenuEnabled)
        {
            if (!menuImage.activeSelf)
            {
                menuImage.SetActive(true);
            }
        }
        else
        {
            if (menuImage.activeSelf)
            {
                menuImage.SetActive(false);
            }
        }
    }
}
