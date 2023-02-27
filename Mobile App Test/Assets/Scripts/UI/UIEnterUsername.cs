using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIEnterUsername : MonoBehaviour
{
    [SerializeField] Button _joinRoom;
    private static string baseURL = "http://192.168.1.11:8000/players/";

    // Start is called before the first frame update
    void Start()
    {
        _joinRoom.onClick.AddListener(CreateJoinRoom);
    }

    void CreateJoinRoom()
    {
        StartCoroutine(CreatePlayer());
    }

    IEnumerator CreatePlayer()
    {
        // Create a Web Form
        WWWForm form = new WWWForm();
        form.AddField("username", "blah"); // THIS IS TEMPORARY, MUST READ INPUT OF TEXTBOX
        form.AddField("device", "MO");
        
        using (UnityWebRequest www = UnityWebRequest.Post(baseURL, form))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                // ScenesManager.Instance.LoadScene(ScenesManager.Scene.CreateJoinRoomScene);
                ScenesManager.Instance.LoadCreateRoom();
            }
        }
    }
}
