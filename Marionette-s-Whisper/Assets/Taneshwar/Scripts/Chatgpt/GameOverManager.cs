using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;

    private void Start()
    {
        // Ensure the Game Over screen is disabled at the beginning
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    public void ShowGameOverScreen()
    {
        // Trigger the Game Over screen and pause the game
        Time.timeScale = 0f;
        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        // Implement the logic to restart the game (e.g., reloading the scene or resetting game state)
        // For example, you can use SceneManager.LoadScene(0); to reload the first scene.
        // Make sure to re-enable the Time.timeScale after restarting the game.
        Time.timeScale = 1f;
        gameOverScreen.SetActive(false);
    }
}
