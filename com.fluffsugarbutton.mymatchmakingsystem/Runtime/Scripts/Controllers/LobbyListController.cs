using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;


namespace MatchMaking.LobbySetup.UI
{
    public class LobbyListController
    {
        // UXML template for list entries
        VisualTreeAsset lobbyListEntryTemplate;

        // UI element references
        ListView LobbyList;

        Button backButton;
        Button refreshButton;
        List<Lobby> AllLobbies;
        public void InitializeUtilityButtons(VisualElement root)
        {
            backButton = root.Q<Button>("backButton");

            backButton.clickable.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
            backButton.clicked += () => {
                ScenesManager.Instance.LoadScene(ScenesManager.Scene.LobbyOptions);
            };

            refreshButton = root.Q<Button>("refreshButton");
            refreshButton.clickable.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
            refreshButton.clicked += async () => {
                // Apparently, there is no need to call the clear function
                // or destroy the list items in any way. 
                // makeItem and bindItem take care of that
                // just reassign the itemSource and refresh et voila!
                // Make sure to also change the bindItem,
                // in case the list was previously empty and is now not,
                // and vice versa.
                await EnumerateAllLobbies();
                Debug.Log("Lobbies: " + AllLobbies.Count);
                if(AllLobbies.Count == 0)
                {
                    Debug.Log("Empty list...");
                    HandleEmptyList();
                }else{
                    Debug.Log("Non empty list...");
                    HandleNonEmptyList();
                }
                LobbyList.Rebuild();
            };
        }

        public async void InitializeLobbyList(VisualElement root, VisualTreeAsset listElementTemplate)
        {
            // Store a reference to the lobby list element
            LobbyList = root.Q<ListView>("listOfLobbies");
            lobbyListEntryTemplate = listElementTemplate;        

            await EnumerateAllLobbies();
            
            FillLobbyList();
        }

        async Task EnumerateAllLobbies()
        {
            AllLobbies = await MainManager.Instance.SearchForLobbies();
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

            LobbyList.fixedItemHeight = 70;

            // Set the actual item's source list/array
            if(AllLobbies.Count == 0){
                HandleEmptyList();
            }else
            {
                HandleNonEmptyList();
            }
        }

        void HandleEmptyList()
        {
            
            LobbyList.bindItem = (item, _) =>
            {
                (item.userData as LobbyListEntryController).SetEmptyListData();
            };
            LobbyList.itemsSource = new List<string>(){"Empty List"};        
        }

        void HandleNonEmptyList()
        {
            LobbyList.bindItem = (item, index) =>
            {
                (item.userData as LobbyListEntryController).SetLobbyInfoData(AllLobbies[index]);
            };
            LobbyList.itemsSource = AllLobbies;
        }
    }
}
