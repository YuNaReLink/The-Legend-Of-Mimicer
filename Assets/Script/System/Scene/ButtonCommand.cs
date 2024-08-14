using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonCommand : MonoBehaviour
{
    public void LoadScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
        Time.timeScale = 1f;
    }

    public void BackGame()
    {
        GameManager.MenuEnabled = false;
    }
}
