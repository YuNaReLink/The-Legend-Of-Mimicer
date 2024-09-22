using UnityEngine;

public class ButtonCommand : MonoBehaviour
{
    public void BackGame()
    {
        GameManager.GameState = GameManager.GameStateEnum.Game;
    }
}
