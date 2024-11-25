using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Button_OnClick : MonoBehaviour
{
    public GameObject gameOverMenu; // Game Over Menu
    public Text gameOverText; // Text UI to display the score
    private bool isGameOver = false; // Flag to track if the game is over

    // Show the Game Over Menu
    public void Set_GameOverMenu(int currentScore)
    {
        if (gameOverMenu != null)
        {
            isGameOver = true; // Mark the game as over
            gameOverMenu.SetActive(true); // Display the Game Over menu

            // Display current score and high score
            if (gameOverText != null)
            {
                gameOverText.text = "Game Over\n"
                                    + "Your Score: " + currentScore + "\n"
                                    + "High Score: " + Data_Manager.GetHighScore();
            }

            // Update the high score if the current score is higher
            Data_Manager.SetHighScore(currentScore);
        }
    }

    // Exit to the main menu
    public void ExitToMenu_OnClick()
    {
        if (isGameOver)
        {
            Debug.Log("Returning to the main menu...");
            SceneManager.LoadScene("Main_Menu");
        }
    }

    // Retry the game
    public void Retry_OnClick()
    {
        if (isGameOver)
        {
            Debug.Log("Retrying the game...");
            SceneManager.LoadScene("In_Game");
        }
    }
}
