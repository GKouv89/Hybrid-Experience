using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using TMPro;

namespace MatchMaking.LobbySetup{
    public class LobbyManager : MonoBehaviour
    {
        public static LobbyManager Instance; 
        private string lobbyName;
        public string LobbyName
        {
            get { return lobbyName; }
            set
            {
                lobbyName = value;
                Debug.Log("Change in lobbyName: " + lobbyName);
            }
        }
        public bool isHost = false;
        private Lobby myLobby;
        public Lobby MyLobby
        {
            get { return myLobby; }
            set 
            {
                myLobby = value;
                if(OnLobbyChange != null){ OnLobbyChange(); }
            }
        }

        private string gamehasStarted = "false";
        public string GameHasStarted
        {
            get { return gamehasStarted; }
            set
            {
                gamehasStarted = value;
                if(OnGameStatusChange != null){ OnGameStatusChange(); }
            }
        }
        public delegate void OnGameStatusChangeDelegate();
        public event OnGameStatusChangeDelegate OnGameStatusChange;

        public delegate void OnLobbyChangeDelegate();
        public event OnLobbyChangeDelegate OnLobbyChange;

        public Player player;
        private bool playerReady = false;
        private float heartbeatTimer;
        private float lobbyUpdateTimer;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("WARNING");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            this.OnGameStatusChange += ScenesManager.Instance.startGame;
        }

        private void DisconnectionCallback()
        {
            Debug.Log("LOBBY: A client disconnect was detected.");
        }

        public async Task CreateLobby(string allocationId, string joinCode) {
            try {
                int maxPlayers = 2;

                CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions{
                    IsPrivate = false,
                    Player = GetPlayer(allocationId),
                };
                player = createLobbyOptions.Player;

                createLobbyOptions.Data = new Dictionary<string, DataObject>()
                {
                    {
                        "HostGameMode", new DataObject(
                            visibility: DataObject.VisibilityOptions.Public, // Visible publicly.
                            value: MainManager.deviceType,
                            index: DataObject.IndexOptions.S1)
                    },
                    {
                        "HasGameStarted", new DataObject(
                            visibility: DataObject.VisibilityOptions.Public,
                            value: "false",
                            index: DataObject.IndexOptions.S2)
                    },
                    {
                        "AllocationJoinCode", new DataObject(
                            visibility: DataObject.VisibilityOptions.Member,
                            value: joinCode,
                            index: DataObject.IndexOptions.S3)
                    },
                };
                isHost = true;
                Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
                myLobby = lobby;

                // LobbyService.Instance.SubscribeToLobbyEventsAsync(lobbyId, callback);

                Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["HostGameMode"].Value);
            }catch (LobbyServiceException e){
                Debug.Log(e);
            }
        }

        public async Task<string> JoinLobby(string allocationId, string lobbyId){
            JoinLobbyByIdOptions options = new JoinLobbyByIdOptions
            {
                Player = GetPlayer(allocationId)
            };
            player = options.Player; 
            Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId, options);
            
            myLobby = lobby;

            return myLobby.Data["AllocationJoinCode"].Value.ToString();
        }

        private Player GetPlayer(string allocationId){
            return new Player (
                                id: AuthenticationService.Instance.PlayerId, 
                                allocationId: allocationId
                ) {
                    Data = new Dictionary<string, PlayerDataObject>{
                        {"playerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, MainManager.Instance.username)},
                        {"deviceType", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, MainManager.deviceType)},
                        {"ready", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "Not Ready")}
                    },
                };
        }

        public async void UpdatePlayerStatus(){
            try 
            {
                playerReady = !playerReady;
                string readiness;
                if(playerReady)
                {
                    readiness = "Ready";
                }
                else
                {
                    readiness = "Not Ready";
                }

                UpdatePlayerOptions options = new UpdatePlayerOptions();
                options.Data = new Dictionary<string, PlayerDataObject>()
                {
                    {"ready", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, readiness)},
                };
                // Updating local player copy

                player.Data["ready"].Value = readiness;

                string playerId = AuthenticationService.Instance.PlayerId;

                myLobby = await LobbyService.Instance.UpdatePlayerAsync(myLobby.Id, playerId, options);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        public async void UpdatePlayerRelayStatus(string allocationId){
            try{
                UpdatePlayerOptions options = new UpdatePlayerOptions();
                options.AllocationId = allocationId;

                Lobby newLobby = await LobbyService.Instance.UpdatePlayerAsync(myLobby.Id, player.Id, options);
                
                // Updating local player copy
                foreach(Player tempPlayer in newLobby.Players)
                {                
                    Debug.Log(tempPlayer.Id + " " + tempPlayer.Data["playerName"].Value + " " + tempPlayer.AllocationId);
                    if(player.Id == tempPlayer.Id)
                    {
                        Debug.Log("Player found. Updating local player copy...");
                        player = tempPlayer;
                        // break;
                    }
                }
                myLobby = newLobby;

            }catch (LobbyServiceException e){
                Debug.Log(e);
            }
        }

        public async void UpdateGameStatus(){
            try 
            {
                string newGameStatus;
                if(myLobby.Data["HasGameStarted"].Value.ToString().Equals("false"))
                {
                    newGameStatus = "true";
                }
                else
                {
                    newGameStatus = "false";
                }
                UpdateLobbyOptions options = new UpdateLobbyOptions();
                options.Data = new Dictionary<string, DataObject>()
                {
                    {
                        "HasGameStarted", new DataObject(
                            DataObject.VisibilityOptions.Public, newGameStatus
                        )
                    },
                };

                myLobby = await LobbyService.Instance.UpdateLobbyAsync(myLobby.Id, options);
                GameHasStarted = newGameStatus;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        private async void HandleLobbyPollForUpdates() {
            if(MyLobby != null)
            {
                lobbyUpdateTimer -= Time.deltaTime;
                if(lobbyUpdateTimer < 0f)
                {
                    float lobbyUpdateTimerMax = 1.1f;
                    lobbyUpdateTimer = lobbyUpdateTimerMax;
                    Lobby lobby = await LobbyService.Instance.GetLobbyAsync(myLobby.Id);
                    
                    string newGameStatus = lobby.Data["HasGameStarted"].Value.ToString();
                    string oldGameStatus = GameHasStarted;
                    if(!oldGameStatus.Equals(newGameStatus))
                    {
                        GameHasStarted = newGameStatus;
                    }
                    MyLobby = lobby;
                }
            }
        }

        // private async Task Reconnect()
        // {
        //     await LobbyService.Instance.ReconnectToLobbyAsync(MyLobby.Id);
        //     Debug.Log("Reconnected");
        // }

        private async void HandleLobbyHeartbeat(){
            if(myLobby != null && isHost)
            {
                heartbeatTimer -= Time.deltaTime;
                if(heartbeatTimer < 0f)
                {
                    float heartbeatTimerMax = 15;
                    heartbeatTimer = heartbeatTimerMax;
                    await LobbyService.Instance.SendHeartbeatPingAsync(myLobby.Id);
                }
            }
        }

        private void Update(){
            HandleLobbyHeartbeat();
            if(myLobby != null && !GameHasStarted.Equals("true"))
            {
                HandleLobbyPollForUpdates();
            }
        }

        public async Task<List<Lobby>> SearchForLobbies() {
            List<Lobby> result = new List<Lobby>();
            try {
                QueryLobbiesOptions options = new QueryLobbiesOptions();
                options.Count = 25;

                options.Filters = new List<QueryFilter>
                {
                    new QueryFilter(
                        field: QueryFilter.FieldOptions.S1,
                        op: QueryFilter.OpOptions.NE,
                        value: MainManager.deviceType
                    ),
                    new QueryFilter(
                        field: QueryFilter.FieldOptions.AvailableSlots,
                        op: QueryFilter.OpOptions.EQ,
                        value: "1"
                    ),
                    new QueryFilter(
                        field: QueryFilter.FieldOptions.S2,
                        op: QueryFilter.OpOptions.EQ,
                        value: "false"
                    ),
                };

                QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(options);
                Debug.Log("Lobbies found: " + queryResponse.Results.Count);
                
                foreach (Lobby lobby in queryResponse.Results){
                    foreach(Player player in lobby.Players)
                    {
                        Debug.Log(lobby.Name + " " + player.Data["playerName"].Value.ToString() + " " + lobby.Data["HostGameMode"].Value.ToString() + " " + MainManager.deviceType);
                    }
                    result.Add(lobby);
                }
            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
            return result;
        }

        public async Task LeaveLobby()
        {
            try
            {
                string playerId = AuthenticationService.Instance.PlayerId;
                await LobbyService.Instance.RemovePlayerAsync(MyLobby.Id, playerId);

                Cleanup();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        public void Cleanup()
        {
            // Clean up refs to Lobby and Player
            myLobby = null;
            player = null;
            playerReady = false;
            isHost = false;
            LobbyName = null;
            gamehasStarted = "false";
        }
    }
}