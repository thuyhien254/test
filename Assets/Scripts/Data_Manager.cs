using System.IO;
using UnityEngine;

public static class Data_Manager
{
    private static int highScore = 0; // Highest score
    private static string playerName = "Player"; // Default player name

    private static readonly string saveFilePath = Path.Combine(Application.persistentDataPath, "playerData.json"); // Path to the JSON file

    // Automatically load data on initialization
    static Data_Manager()
    {
        LoadData();
    }

    // Save player data to a JSON file
    public static void SaveData()
    {
        PlayerData data = new PlayerData
        {
            HighScore = highScore,
            PlayerName = playerName
        };

        try
        {
            string json = JsonUtility.ToJson(data, true); // Save in readable format (indentation)
            File.WriteAllText(saveFilePath, json);
            Debug.Log($"Data saved to: {saveFilePath}");
        }
        catch (IOException e)
        {
            Debug.LogError($"Error saving data: {e.Message}");
        }
    }

    // Load player data from a JSON file
    public static void LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);

                highScore = data.HighScore;
                playerName = data.PlayerName;

                Debug.Log("Data successfully loaded!");
            }
            catch (IOException e)
            {
                Debug.LogError($"Error reading data: {e.Message}");
            }
        }
        else
        {
            Debug.Log("Save file not found, creating default data...");
            SaveData(); // Create default data if file does not exist
        }
    }

    // Set a new high score if it is higher than the current one
    public static void SetHighScore(int score)
    {
        if (score > highScore)
        {
            highScore = score;
            Debug.Log($"New high score: {highScore}");
            SaveData(); // Automatically save when high score changes
        }
    }

    // Retrieve the current high score
    public static int GetHighScore()
    {
        return highScore;
    }

    // Set the player's name
    public static void SetPlayerName(string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            playerName = name;
            Debug.Log($"Player name updated to: {playerName}");
            SaveData(); // Automatically save when player name changes
        }
    }

    // Retrieve the player's name
    public static string GetPlayerName()
    {
        return playerName;
    }

    // Check if the save file exists
    public static bool IsDataFileExists()
    {
        return File.Exists(saveFilePath);
    }

    // Delete the save file
    public static void DeleteData()
    {
        if (IsDataFileExists())
        {
            File.Delete(saveFilePath);
            Debug.Log("Save data deleted.");
        }
    }


    // Nested class representing the player data structure
    [System.Serializable]
    private class PlayerData
    {
        public int HighScore;
        public string PlayerName;
    }
}
