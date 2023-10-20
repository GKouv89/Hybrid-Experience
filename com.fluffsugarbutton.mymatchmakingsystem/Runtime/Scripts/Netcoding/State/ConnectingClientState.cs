
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
// using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;

namespace MatchMaking.ConnectionManagement{
    class ConnectingClientState : ConnectionState
    {
        public override void Enter(RelayServerData data)
        {
            _ConnectionManager.CurrentState = ConnectionManager.State.Connecting;
            _NetworkManager.GetComponent<UnityTransport>().SetRelayServerData(data);
            _NetworkManager.StartClient();
            _ConnectionManager.ChangeState(_ConnectionManager._connectedClientState);
        }
    }
}
