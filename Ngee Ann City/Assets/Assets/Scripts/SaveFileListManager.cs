using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class SaveFileListManager : MonoBehaviour
{
    public GameObject buttonPrefab;  // Assign this prefab in the Unity Inspector
    public Transform buttonContainer;  // Assign a specific container for buttons

    void Start()
    {
        UpdateSaveFileList();
    }

    public void UpdateSaveFileList()
    {
        Debug.Log("Updating save file list...");
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
            Debug.Log("Destroying button: " + child.gameObject.name);
        }

        Debug.Log("ConfigurationManager instance: " + ConfigurationManager.Instance);
        Debug.Log("SceneChanger instance: " + SceneChanger.Instance);

        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.json");
        Debug.Log("Found files: " + files.Length);

        float yPosition = 200; // Start y-position for the first button
        float spacing = 175; // Space between buttons

        foreach (string file in files)
        {
            if (Path.GetFileName(file) == "ArcadeScores.json")
            {
                continue;  // Skip the ArcadeScores.json file
            }

            else if (Path.GetFileName(file) == "FPScores.json")
            {
                continue;  // Skip the FPScores.json file
            }

            GameObject buttonGO = Instantiate(buttonPrefab, buttonContainer);
            buttonGO.GetComponentInChildren<TMP_Text>().text = Path.GetFileNameWithoutExtension(file);
            Debug.Log("Created button for file: " + file);

            // Set the position
            RectTransform rectTransform = buttonGO.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPosition);
            yPosition -= spacing; // Move up for the next button

            // Update listener to set the file path before loading the scene
            buttonGO.GetComponent<Button>().onClick.AddListener(() => {
                if (ConfigurationManager.Instance != null && SceneChanger.Instance != null)
                {
                    ConfigurationManager.Instance.FilePath = file;  // Set the file path safely
                    if (file.Contains("Arcade.json"))
                    {
                        SceneChanger.Instance.LoadScene("Arcade"); // Load Arcade scene
                    }
                    else if (file.Contains("FP.json"))
                    {
                        SceneChanger.Instance.LoadScene("FreePlay"); // Load FreePlay scene
                    }
                    else
                    {
                        SceneChanger.Instance.LoadScene("LoadedSavedGame"); // Load default scene
                    }
                    Debug.Log("ConfigurationManager instance: " + ConfigurationManager.Instance);
                    Debug.Log("SceneChanger instance: " + SceneChanger.Instance);
                }
                else
                {
                    Debug.LogError("ConfigurationManager or SceneChanger is not found.");
                }
            });
        }
    }
}
