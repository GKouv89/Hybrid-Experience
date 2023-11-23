using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;

namespace MatchMaking.ConnectionManagement{
    public class RelayManager
    {
        private Allocation myAllocation;
        private JoinAllocation myJoinAllocation;
        public string MyAllocationId {
            get
            { 
                if(myAllocation != null)
                {
                    return myAllocation.AllocationId.ToString(); 
                }else if(myJoinAllocation != null)
                {
                    return myJoinAllocation.AllocationId.ToString(); 
                }else{
                    return null;
                }
            }
        }
        
        public async Task<string> CreateAllocation() 
        {
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);
                myAllocation = allocation;
                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId); 
                RelayServerData data = new RelayServerData(
                    myAllocation,
                    "dtls"
                );
                ConnectionManager.Instance.StartHosting(data);
                return joinCode;
            } catch (RelayServiceException e)
            {
                Debug.Log(e);
                return null;
            }
        }

        public async Task JoinRelay(string joinCode)
        {
            try
            {
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                myJoinAllocation = joinAllocation;
                RelayServerData data = new RelayServerData(
                    myJoinAllocation,
                    "dtls"
                );
                ConnectionManager.Instance.StartClient(data);
            } catch (RelayServiceException e)
            {
                Debug.Log(e);
            }
        }

        public void Cleanup()
        {
            // If we previously joined a lobby,
            // then we're holding a reference to the allocation
            // that we need to let go of.
            myAllocation = null;
            myJoinAllocation = null;
        }

    }
}
