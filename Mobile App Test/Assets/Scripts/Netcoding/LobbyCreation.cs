using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using TMPro;

public class LobbyCreation : MonoBehaviour
{
    public TMP_InputField lobbyName;

    // Start is called before the first frame update
    async void Start()
    {
        Debug.Log(MainManager.Instance.username);

        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateLobby() {
        try {
            // string lobbyName = "MyLobby";
            int maxPlayers = 2;
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName.text, maxPlayers);
            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers);
        }catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    // public async void ListLobbies() {
    //     try {
    //         QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

    //         Debug.Log("Lobbies found: " + queryResponse.Results.Count);
    //         foreach (Lobby lobby in queryResponse.Results){
    //             Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
    //         }
    //     } catch (LobbyServiceException e) {
    //         Debug.Log(e);
    //     }
    // }
}
