using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UICreateLobby : MonoBehaviour
{
    [SerializeField] Button _createLobby;
    [SerializeField] Button _searchForLobby;

    // Start is called before the first frame update
    void Start()
    {
        _createLobby.onClick.AddListener(CreateLobby);
        _searchForLobby.onClick.AddListener(SearchForLobby);
    }

    private void CreateLobby() 
    {
        LobbyManager.Instance.CreateLobby();
    }

    private void SearchForLobby()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.Scene.LobbyList);
    }
}
