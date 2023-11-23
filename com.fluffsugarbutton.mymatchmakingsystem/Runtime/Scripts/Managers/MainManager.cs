using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;
using UnityEngine.UIElements;
using MatchMaking.LobbySetup;
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

        vivox.VivoxSetup m_VivoxSetup = new vivox.VivoxSetup();
        private ConnectionManagement.RelayManager m_RelayManager = new ConnectionManagement.RelayManager();
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
            // m_VivoxSetup.Initialize();

            DontDestroyOnLoad(gameObject);
        }

        private void setDeviceType(){
            #if UNITY_ANDROID
                deviceType = "mobile";
            #elif UNITY_STANDALONE_WIN
                deviceType = "desktop";
            #endif
        }

        public async void CreateLobby(string lobbyName)
        {
            var m_LobbyManager = LobbyManager.Instance;

            string joinCode = await m_RelayManager.CreateAllocation();
            string allocationId = m_RelayManager.MyAllocationId;

            m_LobbyManager.LobbyName = lobbyName;
            await m_LobbyManager.CreateLobby(allocationId, joinCode);
            
            // m_VivoxSetup.JoinChannel(m_LobbyManager.MyLobby.Id);
            ScenesManager.Instance.LoadScene(ScenesManager.Scene.WaitingRoom);
            // LobbySetup.UI.PanelManager.Instance.ShowPanel("WaitingRoomPanel");
        }

        public async void JoinLobby(string lobbyId)
        {
            var m_LobbyManager = LobbyManager.Instance;
            // this is null at first
            string allocationId = m_RelayManager.MyAllocationId;
            string allocationJoinCode = await m_LobbyManager.JoinLobby(allocationId, lobbyId);
            
            await m_RelayManager.JoinRelay(allocationJoinCode);
            // now that we've joined the relay, read the actual allocation id, and update the player's info
            allocationId = m_RelayManager.MyAllocationId;
            m_LobbyManager.UpdatePlayerRelayStatus(allocationId);

            // m_VivoxSetup.JoinChannel(lobbyId);
            ScenesManager.Instance.LoadScene(ScenesManager.Scene.WaitingRoom);
            // LobbySetup.UI.PanelManager.Instance.ShowPanel("WaitingRoomPanel");
        }

        public async void LeaveLobby()
        {
            // Leave lobby
            await LobbyManager.Instance.LeaveLobby();
            // Leave Voice Channel
            // m_VivoxSetup.LeaveChannel();            
            // We're offline now, any client or host
            // must stop
            var m_ConnectionManager = ConnectionManagement.ConnectionManager.Instance;
            m_ConnectionManager.ChangeState(m_ConnectionManager._offlineState);
            // no relevant allocations any more, clean up references
            m_RelayManager.Cleanup();
            ScenesManager.Instance.LoadScene(ScenesManager.Scene.LobbyOptions);
            // LobbySetup.UI.PanelManager.Instance.ShowPanel("LobbyOptionsPanel");
        }

        public async Task<List<Lobby>> SearchForLobbies()
        {
            var res = await LobbyManager.Instance.SearchForLobbies();
            return res;
        }

        public void UpdatePlayerStatus()
        {
            LobbyManager.Instance.UpdatePlayerStatus();
        }

        public void UpdateGameStatus()
        {
            LobbyManager.Instance.UpdateGameStatus();
        }
    }
}