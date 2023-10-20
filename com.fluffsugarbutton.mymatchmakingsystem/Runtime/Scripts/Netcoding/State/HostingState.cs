using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace MatchMaking.ConnectionManagement
{
    class HostingState : ConnectionState
    {
        public override void Enter()
        {
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
                Debug.Log("It was the other client! What do I have to do now?");
            }
            else
            {
                Debug.Log("I disconnected! Need to do a whole lot of stuff...");
            }
        }
    }
}
