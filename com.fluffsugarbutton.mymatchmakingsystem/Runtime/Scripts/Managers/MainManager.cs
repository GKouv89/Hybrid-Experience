using UnityEngine;
using System.Diagnostics;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

// Implements some of the functionality of the GameManager from the Game Lobby sample repo 
// https://github.com/Unity-Technologies/com.unity.services.samples.game-lobby/tree/main
namespace MatchMaking{
    public class MainManager : MonoBehaviour
    {
        public static MainManager Instance; 
        public string username;
        public static string deviceType;
        // public bool hasGameStarted = false;
        // public string playerId;   

        vivox.VivoxSetup m_VivoxSetup = new vivox.VivoxSetup();
        private LobbySetup.LobbyManager m_LobbyManager;
        private ConnectionManagement.ConnectionManager m_ConnectionManager;
        private ConnectionManagement.RelayManager m_RelayManager;
        async void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            setDeviceType();
            Debug.Log(deviceType);

            await UnityServices.InitializeAsync();
            AuthenticationService.Instance.SignedIn += () => {
                Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
            };
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            m_VivoxSetup.Initialize();

            DontDestroyOnLoad(gameObject);
        }

        private void setDeviceType(){
            #if UNITY_ANDROID
                deviceType = "mobile";
            #elif UNITY_STANDALONE_WIN
                deviceType = "desktop";
            #endif
        }

        public async void CreateLobby()
        {
            if(m_RelayManager == null){
                m_RelayManager = ConnectionManagement.RelayManager.Instance;
            }
            if(m_LobbyManager == null){
                m_LobbyManager = LobbySetup.LobbyManager.Instance;
            }
            string joinCode = await m_RelayManager.CreateAllocation();
            string allocationId = m_RelayManager.MyAllocationId;
            await m_LobbyManager.CreateLobby(allocationId, joinCode);

            m_VivoxSetup.JoinChannel(m_LobbyManager.MyLobby.Id);
            ScenesManager.Instance.LoadScene(ScenesManager.Scene.WaitingRoom);
        }

        public async void JoinLobby(string lobbyId)
        {
            if(m_RelayManager == null){
                m_RelayManager = ConnectionManagement.RelayManager.Instance;
            }
            if(m_LobbyManager == null){
                m_LobbyManager = LobbySetup.LobbyManager.Instance;
            }
            // this is null at first
            string allocationId = m_RelayManager.MyAllocationId;
            string allocationJoinCode = await m_LobbyManager.JoinLobby(allocationId, lobbyId);

            await m_RelayManager.JoinRelay(allocationJoinCode);
            // now that we've joined the relay, read the actual allocation id, and update the player's info
            allocationId = m_RelayManager.MyAllocationId;
            m_LobbyManager.UpdatePlayerRelayStatus(allocationId);

            m_VivoxSetup.JoinChannel(lobbyId);
            ScenesManager.Instance.LoadScene(ScenesManager.Scene.WaitingRoom);
        }

        public async void LeaveLobby()
        {
            if(m_LobbyManager == null){
                m_LobbyManager = LobbySetup.LobbyManager.Instance;
            }
            if(m_ConnectionManager == null)
            {
                m_ConnectionManager = ConnectionManagement.ConnectionManager.Instance;
            }
            // Leave lobby
            await m_LobbyManager.LeaveLobby();
            // Leave Voice Channel
            m_VivoxSetup.LeaveChannel();            
            // We're offline now, any client or host
            // must stop
            m_ConnectionManager.ChangeState(m_ConnectionManager._offlineState);
            // no relevant allocations any more, clean up references
            ConnectionManagement.RelayManager.Instance.Cleanup();
            ScenesManager.Instance.LoadScene(ScenesManager.Scene.LobbyList);
        }
    }
}