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
        [SerializeField] LobbyManager m_LobbyManager;
        // private LobbyManager m_LobbyManager = LobbyManager.Instance;
        private MainManager m_mainManager = MainManager.Instance;
        // Start is called before the first frame update
        void Awake()
        {
            lobbyName = GetComponentInChildren<TMP_InputField>(true);
        }

        void Start()
        {
            _createLobby.onClick.AddListener(CreateLobby);
            _searchForLobby.onClick.AddListener(SearchForLobby);
        }

        private void CreateLobby() 
        {
            m_LobbyManager.LobbyName = lobbyName.text;
            m_mainManager.CreateLobby();
        }

        private void SearchForLobby()
        {
            ScenesManager.Instance.LoadScene(ScenesManager.Scene.LobbyList);
        }
    }
}
