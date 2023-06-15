using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;
public abstract class ConnectionState
{

    protected ConnectionManager _ConnectionManager;
    protected NetworkManager _NetworkManager;
    protected ConnectionState(){
        Debug.Log("Heyoo");
        _ConnectionManager = ConnectionManager.Instance;
        _NetworkManager = NetworkManager.Singleton;
    }

    public virtual void StartHosting(RelayServerData data) { }
    public virtual void StartClient(RelayServerData data) { }
    public virtual void Enter(RelayServerData data){ }
    public abstract void OnClientConnect(ulong ClientId);
    public abstract void OnClientDisconnect(ulong ClientId);
}