using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public GridManagerForLoading griddy;

    void Start()
    {
        string filePath = ConfigurationManager.Instance.FilePath;
        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            Debug.Log("[GameManager] File found, proceeding to load.");
            LoadSelectedGame(filePath);
        }
        else
        {
            Debug.LogError("[GameManager] No file path set or file does not exist.");
        }
    }

    public void LoadSelectedGame(string filePath)
    {
        Debug.Log($"[GameManager] Attempting to load game from path: {filePath}");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log($"[GameManager] File found. Reading data: {json}");
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            LoadGame(saveData);
        }
        else
        {
            Debug.LogError($"[GameManager] Failed to load game. File does not exist: {filePath}");
        }
    }

    private void LoadGame(SaveData saveData)
    {
        if (griddy != null)
        {
            Debug.Log("[GameManager] GridManagerForLoading is set. Resetting game.");
            ApplyLoadedGameData(saveData);
        }
        else
        {
            Debug.LogError("[GameManager] GridManagerForLoading not set in the GameManager.");
        }
    }

    private void ApplyLoadedGameData(SaveData saveData)
    {
        griddy.ResetGame();  // Make sure the game grid is cleared before loading new data

        // Now apply the loaded game data
        griddy.scoreCount = saveData.score;
        griddy.coinCount = saveData.coins;
        griddy.turnCount = saveData.turnCount;

        griddy.UpdateTurnCountUI();
        griddy.UpdateCoinCountUI();
        griddy.UpdateScoreUI();

        foreach (BuildingData buildingData in saveData.buildings)
        {
            griddy.BuildWithoutConditions((int)buildingData.position.x, (int)buildingData.position.y, buildingData.buildingType);
        }
    }
}
