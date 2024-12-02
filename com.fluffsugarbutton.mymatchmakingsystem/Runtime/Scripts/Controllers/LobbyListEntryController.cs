using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

namespace MatchMaking.LobbySetup.UI{
    public class LobbyListEntryController
    {
        Label lobbyNameLabel;
        Label hostUsernameLabel;
        Button joinButton;
        string lobbyId;
        MainManager m_MainManager = MainManager.Instance;
        // This function retrieves a reference to the 
        // lobby name label inside the UI element.

        public void SetVisualElement(VisualElement visualElement)
        {
            lobbyNameLabel = visualElement.Q<Label>("LobbyName");
            hostUsernameLabel = visualElement.Q<Label>("Host");
            joinButton = visualElement.Q<Button>("joinButton");
        }

        // This function receives the lobby whose info this list 
        // element displays. 

        public void SetLobbyInfoData(Lobby lobbyInfoData)
        {
            lobbyId = lobbyInfoData.Id;
            lobbyNameLabel.text = lobbyInfoData.Name;
            hostUsernameLabel.text = lobbyInfoData.Players[0].Data["playerName"].Value.ToString();
            joinButton.clickable.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
            joinButton.clicked += () => {
                m_MainManager.JoinLobby(lobbyId);
            };
        }

        public void SetEmptyListData(){
            joinButton.RemoveFromHierarchy();
            hostUsernameLabel.RemoveFromHierarchy();
            lobbyNameLabel.text = "No lobbies currently. Try refreshing.";
        }
    }

}
