using UnityEngine;

public class GameManager
{
    public enum GameStateEnum
    {
        Null,
        Title,
        Game,
        Pose,
        GameClear,
        GameOver,
    }
    private static GameStateEnum    gameState = GameStateEnum.Null;
    public static GameStateEnum     GameState { get { return gameState; } set { gameState = value; } }

    /// <summary>
    /// ƒƒjƒ…[‚Ì•\¦
    /// </summary>
    private static bool             menuEnabled = false;
    public static bool              MenuEnabled { get { return menuEnabled; } set { menuEnabled = value; } }

    private static int              frameRate = 120;
    public static int               FrameRate => frameRate;
    public static void              SetFrameRate(int _frameRate) { Application.targetFrameRate  = _frameRate; }
}
