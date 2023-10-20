using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace MatchMaking.ConnectionManagement{
    class ConnectedClientState : ConnectionState
    {
        public override void Enter()
        {
            _ConnectionManager.CurrentState = ConnectionManager.State.ConnectedClient;
        }

        public override void Exit()
        {
            
        }
        public override void OnClientConnect(ulong ClientId)
        { 
            Debug.Log("A client has connected.");
        }
        public override void OnClientDisconnect(ulong ClientId)
        { 
            Debug.Log("I have disconnected! What should I do?");
            // if reason for disconnect is null, bad internet connection
            // _ConnectionManager.CurrentState = ConnectionManager.State.Reconnecting;
        }
    }
}
