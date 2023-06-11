using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class LobbyListController
{
    // UXML template for list entries
    VisualTreeAsset lobbyListEntryTemplate;

    // UI element references
    ListView LobbyList;

    public async void InitializeLobbyList(VisualElement root, VisualTreeAsset listElementTemplate, VisualTreeAsset emptyListElementTemplate)
    {
        await EnumerateAllLobbies();

        // Store a reference to the lobby list element
        LobbyList = root.Q<ListView>("listOfLobbies");
            
        // if(AllLobbies.Count != 0){
            // Store a reference to the template for the list entries
            lobbyListEntryTemplate = listElementTemplate;
            
            FillLobbyList();
        // }else{
        //     lobbyListEntryTemplate = emptyListElementTemplate;

        //     EmptyLobbyList();
        // }
    }

    List<Lobby> AllLobbies;
    async Task EnumerateAllLobbies()
    {
        AllLobbies = await LobbyManager.Instance.SearchForLobbies();
    }

    // void EmptyLobbyList()
    // {
    //     LobbyList.itemsSource = new List<string>(){"Empty List"};
        
    //     LobbyList.makeItem = () => {
    //         var newEmptyList = lobbyListEntryTemplate.Instantiate();
    //         return newEmptyList;
    //     };

    //     LobbyList.bindItem = (item, index) => {

    //     };

    //     LobbyList.fixedItemHeight = 70;
    // }

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



        LobbyList.fixedItemHeight = 70;

        // Set the actual item's source list/array
        if(AllLobbies.Count == 0){
            LobbyList.itemsSource = new List<string>(){"Empty List"};
            LobbyList.bindItem = (item, index) =>
            {
                (item.userData as LobbyListEntryController).SetEmptyListData();
            };
        }else
        {
            // Set up bind function for a specific list entry
            LobbyList.bindItem = (item, index) =>
            {
                (item.userData as LobbyListEntryController).SetLobbyInfoData(AllLobbies[index]);
            };
            LobbyList.itemsSource = AllLobbies;
        }
    }
}
