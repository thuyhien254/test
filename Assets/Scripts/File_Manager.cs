using System.IO;
using UnityEngine;

public class File_Manager : MonoBehaviour
{
    private static string filePath;

    // Structure for player data
    [System.Serializable]
    public class PlayerData
    {
        public string PlayerName;
        public string PlayerHash;
        public int HighScore;
    }

    void Awake()
    {
        // Set file path
        filePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");

        // Prevent screen from sleeping
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // Load player data
        LoadPlayerData();
    }

    public void LoadPlayerData()
    {
        if (File.Exists(filePath))
        {
            try
            {
                // Read JSON from file
                string json = File.ReadAllText(filePath);

                // Deserialize JSON into PlayerData
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);

                // Update Data_Manager with loaded data
                Data_Manager.SetPlayerName(data.PlayerName);
                Data_Manager.SetHighScore(data.HighScore);

                Debug.Log("Player data loaded successfully.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading player data: {e.Message}");
                ResetPlayerData(); // Reset to default data if an error occurs
            }
        }
        else
        {
            ResetPlayerData(); // Initialize default data if no save file exists
        }
    }

    public static void SavePlayerData()
    {
        try
        {
            PlayerData data = new PlayerData
            {
                PlayerName = Data_Manager.GetPlayerName(),
                HighScore = Data_Manager.GetHighScore()
            };

            // Convert to JSON and write to file
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);

            Debug.Log("Player data saved successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving player data: {e.Message}");
        }
    }

    private void ResetPlayerData()
    {
        Data_Manager.SetPlayerName("Player"); // Default player name
        Data_Manager.SetHighScore(0);

        Debug.Log("Player data reset to defaults.");
    }

    void OnApplicationQuit()
    {
        SavePlayerData(); // Save data when the application is about to quit
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SavePlayerData(); // Save data when the application is paused
        }
    }
}
