using UnityEngine;

/// <summary>
/// Manages the music in the game.
/// </summary>
public class MusicManager : MonoBehaviour
{
    private static MusicManager instance = null;

    /// <summary>
    /// Gets the instance of the MusicManager.
    /// </summary>
    public static MusicManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        Debug.Log("MusicManager Awake called.");
        if (instance != null && instance != this)
        {
            Debug.Log("MusicManager instance already exists, destroying new one.");
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Debug.Log("MusicManager instance set.");
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

}
