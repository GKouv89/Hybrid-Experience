using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UICreateLobby : MonoBehaviour
{
    [SerializeField] Button _createLobby;

    // Start is called before the first frame update
    void Start()
    {
        _createLobby.onClick.AddListener(CreateLobby);
    }

    private void CreateLobby() 
    {
        GameObject.Find("LobbyBehavior").GetComponent<LobbyCreation>().CreateLobby();
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.WaitingRoom);
    }
}
