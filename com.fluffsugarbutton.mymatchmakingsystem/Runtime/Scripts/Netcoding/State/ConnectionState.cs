using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;

namespace MatchMaking.ConnectionManagement{
    public abstract class ConnectionState
    {

        protected ConnectionManager _ConnectionManager;
        protected NetworkManager _NetworkManager;
        protected ConnectionState(){
            _ConnectionManager = ConnectionManager.Instance;
            _NetworkManager = NetworkManager.Singleton;
        }

        public virtual void StartHosting(RelayServerData data) { }
        public virtual void StartClient(RelayServerData data) { }
        public virtual void Enter(){ }
        public virtual void Exit(){ }
        public virtual void Enter(RelayServerData data){ }
        public virtual void OnClientConnect(ulong ClientId){ }
        public virtual void OnClientDisconnect(ulong ClientId){ }
    }
}
