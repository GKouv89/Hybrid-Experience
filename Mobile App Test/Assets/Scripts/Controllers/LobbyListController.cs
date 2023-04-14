using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyListController
{
    // UXML template for list entries
    VisualTreeAsset lobbyListEntryTemplate;

    // UI element references
    ListView LobbyList;

    public void InitializeLobbyList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        EnumerateAllLobbies();

        // Store a reference to the template for the list entries
        lobbyListEntryTemplate = listElementTemplate;

        // Store a reference to the lobby list element
        LobbyList = root.Q<ListView>("listOfLobbies");
        
        FillLobbyList();
    }

    List<LobbyListData> AllLobbies;

    void EnumerateAllLobbies()
    {
        AllLobbies = new List<LobbyListData>();
        AllLobbies.AddRange(Resources.LoadAll<LobbyListData>("Lobbies"));
    }

    void FillLobbyList()
    {
        // Set up a make item function for a list entry
        LobbyList.makeItem = () =>
        {
            // Instantiate the UXML template for the entry
            var newListEntry = lobbyListEntryTemplate.Instantiate();

            // Instantiate a controller for the data
            var newListEntryLogic = new LobbyListEntryController();

            // Assign the controller script to the visual element
            newListEntry.userData = newListEntryLogic;

            // Initialize the controller script
            newListEntryLogic.SetVisualElement(newListEntry);

            // Return the root of the instantiated visual tree
            return newListEntry;
        };

        // Set up bind function for a specific list entry
        LobbyList.bindItem = (item, index) =>
        {
            (item.userData as LobbyListEntryController).SetLobbyInfoData(AllLobbies[index]);
        };

        LobbyList.fixedItemHeight = 70;

        // Set the actual item's source list/array
        LobbyList.itemsSource = AllLobbies;
    }
}
