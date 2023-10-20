using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;

namespace MatchMaking.ConnectionManagement
{
    class StartingHostingState : ConnectionState 
    {
        public override void Enter(RelayServerData data)
        {
            _ConnectionManager.CurrentState = ConnectionManager.State.StartingHosting;
            _NetworkManager.GetComponent<UnityTransport>().SetRelayServerData(data);
            _NetworkManager.StartHost();
            _ConnectionManager.ChangeState(_ConnectionManager._hostingState);
        }

    }
}
