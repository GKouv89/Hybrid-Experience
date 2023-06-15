using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;

class HostingState : ConnectionState 
{
    public override void Enter(RelayServerData data)
    {
        _NetworkManager.GetComponent<UnityTransport>().SetRelayServerData(data);
        _NetworkManager.StartHost();
        _ConnectionManager.CurrentState = ConnectionManager.State.Hosting;
    }

    public override void OnClientConnect(ulong clientId)
    { 
        Debug.Log("A client has connected.");
    }

    public override void OnClientDisconnect(ulong clientId)
    { 
        if(clientId != _NetworkManager.LocalClientId)
        {
            Debug.Log("I disconnected! Need to do a whole lot of stuff...");
        }
        else
        {
            Debug.Log("It was the other client! What do I have to do now?");
        }
    }
}