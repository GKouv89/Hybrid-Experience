using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance; 
    public TMP_InputField lobbyName;
    private Lobby createdLobby;
    public Lobby CreatedLobby
    {
        get { return createdLobby; }
        set 
        {
            createdLobby = value;
            if(OnLobbyChange != null){ OnLobbyChange(); }
        }
    }

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
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateLobby() {
        try {
            int maxPlayers = 2;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions{
                IsPrivate = false,
                Player = GetPlayer()
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
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName.text, maxPlayers, createLobbyOptions);
            createdLobby = lobby;
            
            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["HostGameMode"].Value);
            ScenesManager.Instance.LoadScene(ScenesManager.Scene.WaitingRoom);
        }catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    private Player GetPlayer(){
        return new Player{
            Data = new Dictionary<string, PlayerDataObject>{
                {"playerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, MainManager.Instance.username)},
                {"deviceType", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, MainManager.deviceType)},
                {"ready", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "Not Ready")}
            }
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

            createdLobby = await LobbyService.Instance.UpdatePlayerAsync(createdLobby.Id, playerId, options);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void HandleLobbyPollForUpdates() {
        if(CreatedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if(lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(createdLobby.Id);
                CreatedLobby = lobby;
            }
        }
    }

    private async void HandleLobbyHeartbeat(){
        if(createdLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if(heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax;
                await LobbyService.Instance.SendHeartbeatPingAsync(createdLobby.Id);
            }
        }
    }

    private void Update(){
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }

    public async void SearchForLobbies() {
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
                )                
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(options);

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results){
                foreach(Player player in lobby.Players)
                {
                    Debug.Log(lobby.Name + " " + player.Data["playerName"].Value.ToString() + " " + lobby.Data["HostGameMode"].Value.ToString() + " " + MainManager.deviceType);
                }
            }
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }
}
