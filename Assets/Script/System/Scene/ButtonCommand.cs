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
        GameManager.GameState = GameManager.GameStateEnum.Game;
    }
}
