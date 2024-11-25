using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game_Controller : MonoBehaviour
{
    private GameObject player; // Reference to the Player (Doodler)

    private float maxHeight = 0; // The highest height the player reaches
    public TextMeshProUGUI txtScore; // TextMeshPro reference for the score
    public TextMeshProUGUI txtGameOverScore; // TextMeshPro reference for Game Over score
    public TextMeshProUGUI txtGameOverHighScore; // TextMeshPro reference for Game Over high score

    private int score; // Current player score
    private Vector3 topLeft; // Top-left position of the screen
    private Vector3 cameraPos; // Current camera position

    private bool gameOver = false; // Game Over state

    [Header("Platform Generator")]
    public Platform_Generator platformGenerator; // Reference to the Platform_Generator

    void Awake()
    {
        // Find the Player object
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Ensure your Player GameObject has the 'Player' tag.");
        }

        // Find the Platform_Generator
        if (platformGenerator == null)
        {
            platformGenerator = FindObjectOfType<Platform_Generator>();
            if (platformGenerator == null)
            {
                Debug.LogError("Platform_Generator not found! Ensure it exists in the scene.");
            }
        }

        // Determine the top-left position of the screen
        cameraPos = Camera.main.transform.position;
        topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));

        // Load player data
        File_Manager fileManager = FindObjectOfType<File_Manager>();
        if (fileManager != null)
        {
            fileManager.LoadPlayerData(); // Call the public method to load data
        }
        else
        {
            Debug.LogError("File_Manager script not found in the scene!");
        }
    }

    void FixedUpdate()
    {
        if (!gameOver)
        {
            // Update the player's highest height
            if (player != null && player.transform.position.y > maxHeight)
            {
                maxHeight = player.transform.position.y;
            }

            // Check if the player falls below the screen
            if (player != null && player.transform.position.y - Camera.main.transform.position.y < GetDestroyDistance())
            {
                // Play the Game Over sound
                if (TryGetComponent(out AudioSource audioSource))
                {
                    audioSource.Play();
                }

                // Set the Game Over state
                SetGameOver();
                gameOver = true;
            }
        }
    }

    void Update()
    {
        // Update the score based on the maximum height
        score = (int)(maxHeight * 50);
        if (txtScore != null)
        {
            txtScore.text = score.ToString();
        }
    }

    public bool GetGameOver()
    {
        return gameOver;
    }

    public float GetDestroyDistance()
    {
        // Calculate the platform destroy distance based on the camera's position
        return cameraPos.y + topLeft.y;
    }

    void SetGameOver()
    {
        // Update the high score
        if (Data_Manager.GetHighScore() < score)
        {
            Data_Manager.SetHighScore(score);
        }

        // Update the Game Over UI
        if (txtGameOverScore != null)
        {
            txtGameOverScore.text = score.ToString();
        }
        if (txtGameOverHighScore != null)
        {
            txtGameOverHighScore.text = Data_Manager.GetHighScore().ToString();
        }

        // Display the Game Over Menu
        Button_OnClick buttonOnClick = FindObjectOfType<Button_OnClick>();
        if (buttonOnClick != null)
        {
            buttonOnClick.Set_GameOverMenu(score); // Pass the current score
        }
        else
        {
            Debug.LogError("Button_OnClick script not found in the scene!");
        }

        // Save player data to file
        File_Manager.SavePlayerData();
    }

    // Generate a number of platforms
    public void GeneratePlatform(int num)
    {
        if (platformGenerator != null)
        {
            platformGenerator.GeneratePlatform(num);
        }
        else
        {
            Debug.LogError("Platform_Generator is not assigned or missing in the scene.");
        }
    }
}
