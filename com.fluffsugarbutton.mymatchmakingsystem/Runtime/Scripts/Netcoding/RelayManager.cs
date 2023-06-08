using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class RelayManager : MonoBehaviour
{
    public static RelayManager Instance; 

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public async Task<string> CreateAllocation() 
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId); 

            // RelayServerEndpoint data = new RelayServerEndpoint(
            //     "dtls",
            //     RelayServerEndpoint.NetworkOptions.Udp,
            //     true,
            //     false,
            //     allocation.RelayServer.IpV4,
            //     allocation.RelayServer.Port
            // );

            RelayServerData data = new RelayServerData(
                allocation,
                "dtls"
            );

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(data);
            NetworkManager.Singleton.StartHost();
            
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

            // RelayServerEndpoint data = new RelayServerEndpoint(
            //     "dtls",
            //     RelayServerEndpoint.NetworkOptions.Udp,
            //     true,
            //     false,
            //     joinAllocation.RelayServer.IpV4,
            //     joinAllocation.RelayServer.Port
            // );

            RelayServerData data = new RelayServerData(
                joinAllocation,
                "dtls"
            );

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(data);
            NetworkManager.Singleton.StartClient();

        } catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

}
