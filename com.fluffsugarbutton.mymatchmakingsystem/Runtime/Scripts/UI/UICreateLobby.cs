using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

namespace MatchMaking.LobbySetup.UI {
    public class UICreateLobby : MonoBehaviour
    {
        [SerializeField] Button _createLobby;
        [SerializeField] Button _searchForLobby;
        private TMP_InputField lobbyName;

        void Awake()
        {
            lobbyName = GetComponentInChildren<TMP_InputField>(true);
        }
        // Start is called before the first frame update
        void Start()
        {
            _createLobby.onClick.AddListener(CreateLobby);
            _searchForLobby.onClick.AddListener(SearchForLobby);
        }

        private void CreateLobby() 
        {
            MainManager.Instance.CreateLobby(lobbyName.text);
        }

        private void SearchForLobby()
        {
            ScenesManager.Instance.LoadScene(ScenesManager.Scene.LobbyList);
        }
    }
}
