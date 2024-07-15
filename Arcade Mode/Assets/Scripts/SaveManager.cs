using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



[Serializable]
public class SaveData
{
    public List<BuildingData> buildings = new List<BuildingData>();
    public int score;
    public int coins;
    public int turnCount;
}

[Serializable]
public class BuildingData
{
    public Vector2 position;
    public int buildingType; // This should match your building type enumeration
}

[Serializable]
public class ScoreData
{
    public string playerName;
    public int score;
}

[Serializable]
public class AllScores
{
    public List<ScoreData> allScores = new List<ScoreData>();
}


public class SaveManager : MonoBehaviour
{
    public TMP_InputField filenameInput;  // Reference to the InputField
    public TMP_Text feedbackMessage; // Reference to a Text component used for feedback
    public TMP_InputField playernameInput;  // Reference to the InputField
    public TMP_Text scoreFeedbackMessage; // Reference to a Text component used for feedback


    public void HandleSaveButton()
    {
        if (!string.IsNullOrEmpty(filenameInput.text))
        {
            SaveGame(filenameInput.text.Trim());  // Save using the input field's text
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

    public void SaveGame(string saveFileName)
    {
        SaveData saveData = new SaveData
        {
            score = GridManager.Instance.scoreCount,
            coins = GridManager.Instance.coinCount,
            turnCount = GridManager.Instance.turnCount
        };

        foreach (var building in GridManager.Instance._buildings)
        {
            saveData.buildings.Add(new BuildingData
            {
                position = building.Key,
                buildingType = (int)building.Value.GetComponent<Building>().buildingType
            });
        }

        string json = JsonUtility.ToJson(saveData, true);
        string filePath = Application.persistentDataPath + "/" + saveFileName + ".json";
        File.WriteAllText(filePath, json);
        Debug.Log("Saved to: " + filePath);
    }

    public void SavePlayerScore()
    {
        if (!string.IsNullOrEmpty(playernameInput.text))
        {
            string playerName = playernameInput.text.Trim();
            int currentScore = 0; // Default to 0 in case neither instance is available

            if (GridManager.Instance != null)
            {
                currentScore = GridManager.Instance.scoreCount;
            }
            else if (GridManagerForLoading.Instance != null)
            {
                currentScore = GridManagerForLoading.Instance.scoreCount;
            }
            else
            {
                Debug.LogError("No active Grid Manager found.");
                scoreFeedbackMessage.text = "Error: No game data found!";
                scoreFeedbackMessage.color = Color.red;
                return;
            }
            string filePath = Application.persistentDataPath + "/AllScores.json";
            AllScores scores = new AllScores();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                scores = JsonUtility.FromJson<AllScores>(json);
            }

            ScoreData newScore = new ScoreData { playerName = playerName, score = currentScore };
            scores.allScores.Add(newScore);

            string newJson = JsonUtility.ToJson(scores, true);
            File.WriteAllText(filePath, newJson);

            scoreFeedbackMessage.text = "Score Saved Successfully!";
            scoreFeedbackMessage.color = Color.green;
        }
        else
        {
            Debug.LogError("Player Name is empty!");
            scoreFeedbackMessage.text = "Player Name is empty!";
            scoreFeedbackMessage.color = Color.red;
        }
    }

}
