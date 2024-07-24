using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// The ScoreLoader class is responsible for loading and displaying scores for the Arcade and FreePlay modes.
/// </summary>
public class ScoreLoader : MonoBehaviour
{
    public TMP_Text arcadeScoreDisplay; // Reference to the Text prefab for Arcade scores
    private TMP_Text arcadeScoreDisplayInstance; // Instance of the Text for Arcade scores
    public TMP_Text freePlayScoreDisplay; // Reference to the Text prefab for FreePlay scores
    private TMP_Text freePlayScoreDisplayInstance; // Instance of the Text for FreePlay scores

    public static ScoreLoader Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        EnsureDisplayIsCreated();
        LoadArcadeScores();
        LoadFreePlayScores();
    }

    private void EnsureDisplayIsCreated()
    {
        if (arcadeScoreDisplay && arcadeScoreDisplayInstance == null)
        {
            GameObject arcadeInstance = Instantiate(arcadeScoreDisplay.gameObject, transform);
            arcadeScoreDisplayInstance = arcadeInstance.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogError("Arcade Score Display Prefab is not assigned or already instantiated.");
        }

        if (freePlayScoreDisplay && freePlayScoreDisplayInstance == null)
        {
            GameObject freePlayInstance = Instantiate(freePlayScoreDisplay.gameObject, transform);
            freePlayScoreDisplayInstance = freePlayInstance.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogError("FreePlay Score Display Prefab is not assigned or already instantiated.");
        }
    }

    public void LoadArcadeScores()
    {
        LoadScoresFromFile(Application.persistentDataPath + "/ArcadeScores.json", arcadeScoreDisplayInstance, "Arcade");
    }

    public void LoadFreePlayScores()
    {
        LoadScoresFromFile(Application.persistentDataPath + "/FPScores.json", freePlayScoreDisplayInstance, "FreePlay");
    }

    private void LoadScoresFromFile(string filePath, TMP_Text scoreDisplay, string mode)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            AllScores loadedScores = JsonUtility.FromJson<AllScores>(json);
            loadedScores.allScores.Sort((s1, s2) => s2.score.CompareTo(s1.score));

            scoreDisplay.text = $"{mode} High Scores:\n";
            int count = Math.Min(10, loadedScores.allScores.Count);
            for (int i = 0; i < count; i++)
            {
                string playerName = string.IsNullOrEmpty(loadedScores.allScores[i].playerName) ? "Anonymous" : loadedScores.allScores[i].playerName;
                scoreDisplay.text += $"{i + 1}. {playerName}: {loadedScores.allScores[i].score}\n";
            }
        }
        else
        {
            Debug.LogError($"{mode} Score file not found.");
            scoreDisplay.text = $"{mode} Scores not available.";
        }
    }
}
