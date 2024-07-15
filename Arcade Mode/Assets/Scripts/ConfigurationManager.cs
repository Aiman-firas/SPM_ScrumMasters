using UnityEngine;

public class ConfigurationManager : MonoBehaviour
{
    private static ConfigurationManager _instance;
    public static ConfigurationManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public string FilePath { get; set; }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);  // Persist this GameObject across scenes
        }
        else if (_instance != this)
        {
            Destroy(gameObject);  // Destroy if another instance is created mistakenly
        }
    }
}
