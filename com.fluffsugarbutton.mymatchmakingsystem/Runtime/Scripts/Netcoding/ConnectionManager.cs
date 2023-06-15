using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public enum State {
        Offline,
        // StartingHosting,
        Connecting,
        Hosting,
        ConnectedClient,
        Reconnecting
    };
    private State currentState = State.Offline;
    public State CurrentState {
        get {
            return currentState; 
        }
        set {
            currentState = value;
        }
    }
    private ConnectionState connectionState;
    internal HostingState _hostingState; 
    internal ConnectedClientState _connectedClientState;

    public static ConnectionManager Instance; 
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

    private void Start()
    {
        connectionState = new OfflineState();
        _hostingState = new HostingState();
        _connectedClientState = new ConnectedClientState();

        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
    }

    private void OnClientConnect(ulong clientId)
    {
        connectionState.OnClientConnect(clientId);
    }

    private void OnClientDisconnect(ulong clientId)
    {
        connectionState.OnClientDisconnect(clientId);
    }

    public void StartHosting(RelayServerData data){
        connectionState.StartHosting(data);
    }

    public void StartClient(RelayServerData data){
        connectionState.StartClient(data);
    }

    // public void ChangeState(ConnectionState nextState)
    // {
    //     if(connectionState != null){
    //         // exit code for previous case does not apply yet
    //         connectionState = nextState;
    //         connectionState.Enter();
    //     }
    // }

    public void ChangeState(ConnectionState nextState, RelayServerData data)
    {
        if(connectionState != null){
            // exit code for previous case does not apply yet
            connectionState = nextState;
            connectionState.Enter(data);
        }
    }


    public void test(){
        Debug.Log("test");
    }
}