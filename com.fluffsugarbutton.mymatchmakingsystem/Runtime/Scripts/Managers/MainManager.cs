using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance; 
    public string username;
    public static string deviceType = "mobile";
    // public string playerId;    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
