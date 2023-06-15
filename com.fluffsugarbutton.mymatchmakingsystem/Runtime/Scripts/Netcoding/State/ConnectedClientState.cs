
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
// using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;
class ConnectedClientState : ConnectionState
{
    public override void Enter(RelayServerData data)
    {
        _NetworkManager.GetComponent<UnityTransport>().SetRelayServerData(data);
        _NetworkManager.StartClient();        
        _ConnectionManager.CurrentState = ConnectionManager.State.ConnectedClient;
    }
    public override void OnClientConnect(ulong ClientId)
    { 
        Debug.Log("A client has connected.");
    }
    public override void OnClientDisconnect(ulong ClientId)
    { 
        Debug.Log("I have disconnected! What should I do?");
        // if reason for disconnect is null, bad internet connection
        _ConnectionManager.CurrentState = ConnectionManager.State.Reconnecting;
    }
}