using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreLoader : MonoBehaviour
{
    public TMP_Text scoreDisplay; // Reference to the Text prefab
    private TMP_Text scoreDisplayInstance; // Instance of the Text
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
        LoadTopScores();
    }

    private void EnsureDisplayIsCreated()
    {
        if (scoreDisplay && scoreDisplayInstance == null)
        {
            GameObject instance = Instantiate(scoreDisplay.gameObject, transform);
            scoreDisplayInstance = instance.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogError("Score Display Prefab is not assigned or already instantiated.");
        }
    }

    public void LoadTopScores()
    {
        string filePath = Application.persistentDataPath + "/AllScores.json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            AllScores loadedScores = JsonUtility.FromJson<AllScores>(json);
            loadedScores.allScores.Sort((s1, s2) => s2.score.CompareTo(s1.score));

            scoreDisplay.text = "";
            int count = Math.Min(10, loadedScores.allScores.Count);
            for (int i = 0; i < count; i++)
            {
                scoreDisplay.text += $"{i + 1}. {loadedScores.allScores[i].playerName}: {loadedScores.allScores[i].score}\n";
            }
        }
        else
        {
            Debug.LogError("Score file not found.");
            scoreDisplay.text = "Score not available.";
        }
    }
}
