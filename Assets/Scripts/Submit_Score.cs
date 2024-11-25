using UnityEngine;

public class Submit_Score : MonoBehaviour
{
    public GameObject highScoreCanvas; // UI for displaying the high score board

    public void PostScore(string name, int score)
    {
        // Update player name and high score
        Data_Manager.SetPlayerName(name);
        Data_Manager.SetHighScore(score);

        // Display updated player name and high score in the console
        Debug.Log("Player Name: " + Data_Manager.GetPlayerName());
        Debug.Log("High Score: " + Data_Manager.GetHighScore());
    }

    public void ShowTopScore()
    {
        // Display the current top score
        Debug.Log("Player Name: " + Data_Manager.GetPlayerName());
        Debug.Log("High Score: " + Data_Manager.GetHighScore());
    }
}
