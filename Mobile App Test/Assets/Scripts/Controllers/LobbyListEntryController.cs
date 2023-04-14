using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyListEntryController
{
    Label lobbyNameLabel;
    Label hostUsernameLabel;

    // This function retrieves a reference to the 
    // lobby name label inside the UI element.

    public void SetVisualElement(VisualElement visualElement)
    {
        lobbyNameLabel = visualElement.Q<Label>("LobbyName");
        hostUsernameLabel = visualElement.Q<Label>("Host");
    }

    // This function receives the lobby whose info this list 
    // element displays. 

    public void SetLobbyInfoData(LobbyListData lobbyInfoData)
    {
        lobbyNameLabel.text = lobbyInfoData.lobbyName;
        hostUsernameLabel.text = lobbyInfoData.hostUsername;
    }
}
