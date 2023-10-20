using UnityEngine;
using UnityEngine.UIElements;


namespace MatchMaking.LobbySetup.UI
{
    public class LobbyListView : MonoBehaviour
    {
        [SerializeField]
        VisualTreeAsset ListEntryTemplate;

        void OnEnable()
        {
            // The UXML is already instantiated by the UIDocument component
            var uiDocument = GetComponent<UIDocument>();

            // Initialize the character list controller
            var lobbyListController = new LobbyListController();
            lobbyListController.InitializeUtilityButtons(uiDocument.rootVisualElement);
            lobbyListController.InitializeLobbyList(uiDocument.rootVisualElement, ListEntryTemplate);
        }
    }
}
