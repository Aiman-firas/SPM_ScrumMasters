using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private static SceneChanger _instance;
    public static SceneChanger Instance
    {
        get
        {
            if (_instance == null)
            {
                // Log error if the instance is accessed but not yet set up.
                Debug.LogError("[SceneChanger] Attempted to access instance but it's null.");
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            Debug.Log("[SceneChanger] Singleton instance set and marked to not destroy on load.");
        }
        else if (_instance != this)
        {
            Debug.Log("[SceneChanger] Duplicate instance found, destroying this one.");
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log($"[SceneChanger] Loading scene: {sceneName} with path: {ConfigurationManager.Instance.FilePath}");
        SceneManager.LoadScene(sceneName);
    }


public void PlayArcade()
    {
        Debug.Log("Arcade Mode Start!");
        SceneManager.LoadScene("Arcade");
    }


    public void PlayFreePlay()
    {
        Debug.Log("Free Play Start!");
        SceneManager.LoadScene("FreePlay");
    }

    public void ChangeToleaderboardPage()
    {
        Debug.Log("Leaderboard Page");
        SceneManager.LoadScene("Leaderboard");
    }

    public void ChangeToLoadGamePage()
    {
        Debug.Log("Load Game Page");
        SceneManager.LoadScene("Load");
    }
    
    // Scene Changer to Go Back //

    public void BackToMainMenu()
    {
        Debug.Log("Going Back to Main Menu");
        SceneManager.LoadScene("MainMenu");
    }


    // Scene Changer to Quit Game //
    public void QuitGame()
    {
        Debug.Log("QUIT!!!");
        Application.Quit();
    }
}
