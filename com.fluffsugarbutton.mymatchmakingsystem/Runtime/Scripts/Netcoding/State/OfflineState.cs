using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
// using Unity.Netcode;
// using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;

class OfflineState : ConnectionState
{
    public override void OnClientConnect(ulong ClientId){ }
    public override void OnClientDisconnect(ulong ClientId){ }

    public override void StartHosting(RelayServerData data){
        ConnectionManager.Instance.test();
        _ConnectionManager.ChangeState(_ConnectionManager._hostingState, data);
    }

    public override void StartClient(RelayServerData data){
        _ConnectionManager.ChangeState(_ConnectionManager._connectedClientState, data);
    }
}