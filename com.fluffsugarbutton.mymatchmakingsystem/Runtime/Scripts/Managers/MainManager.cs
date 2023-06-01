using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance; 
    public string username;
    public static string deviceType;
    // public bool hasGameStarted = false;
    // public string playerId;    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        setDeviceType();
        Debug.Log(deviceType);

        DontDestroyOnLoad(gameObject);
    }

    private void setDeviceType(){
        #if UNITY_ANDROID
            deviceType = "mobile";
        #elif UNITY_STANDALONE_WIN
            deviceType = "desktop";
        #endif
    }
}
