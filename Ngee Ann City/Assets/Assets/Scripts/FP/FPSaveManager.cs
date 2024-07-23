using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSaveManager : SaveManager
{
    public new void HandleSaveButton()
    {
        if (!string.IsNullOrEmpty(filenameInput.text))
        {
            SaveGame(filenameInput.text.Trim());
            feedbackMessage.text = "Game Saved!";
            feedbackMessage.color = Color.green;
        }
        else
        {
            Debug.LogError("Filename is empty!");
            feedbackMessage.text = "Filename is empty!";
            feedbackMessage.color = Color.red;
        }
    }

    public new void SaveGame(string saveFileName)
    {
        SaveData saveData = new SaveData
        {
            score = FPGridManager.Instance.scoreCount,
            turnCount = FPGridManager.Instance.turnCount,
            gridWidth = FPGridManager.Instance.GridWidth,
            gridHeight = FPGridManager.Instance.GridHeight
        };

        foreach (var building in FPGridManager.Instance._buildings)
        {
            saveData.buildings.Add(new BuildingData
            {
                position = building.Key,
                buildingType = (int)building.Value.GetComponent<Building>().buildingType
            });
        }

        string json = JsonUtility.ToJson(saveData, true);
        string filePath = Application.persistentDataPath + "/" + saveFileName + "FP.json";
        File.WriteAllText(filePath, json);
        Debug.Log("Saved to: " + filePath);
    }

    public new void SavePlayerScore()
    {
        string playerName = playernameInput.text.Trim();
        int currentScore = 0;

        if (FPGridManager.Instance != null)
        {
            currentScore = FPGridManager.Instance.scoreCount;
        }
        else
        {
            Debug.LogError("No active Grid Manager found.");
            scoreFeedbackMessage.text = "Error: No game data found!";
            scoreFeedbackMessage.color = Color.red;
            return;
        }

        string filePath = Application.persistentDataPath + "/FPScores.json";
        AllScores scores = new AllScores();

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            scores = JsonUtility.FromJson<AllScores>(json);
        }

        ScoreData newScore = new ScoreData { score = currentScore };

        // Include the player name only if it's not empty
        if (!string.IsNullOrEmpty(playerName))
        {
            newScore.playerName = playerName;
        }

        scores.allScores.Add(newScore);

        string newJson = JsonUtility.ToJson(scores, true);
        File.WriteAllText(filePath, newJson);

        scoreFeedbackMessage.text = "Score Saved Successfully!";
        scoreFeedbackMessage.color = Color.green;
    }

}
