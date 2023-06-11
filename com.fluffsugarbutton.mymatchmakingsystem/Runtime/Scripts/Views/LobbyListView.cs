using UnityEngine;
using UnityEngine.UIElements;

public class LobbyListView : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset ListEntryTemplate;
    [SerializeField]
    VisualTreeAsset EmptyListEntryTemplate;

    void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        // Initialize the character list controller
        var lobbyListController = new LobbyListController();
        lobbyListController.InitializeLobbyList(uiDocument.rootVisualElement, ListEntryTemplate, EmptyListEntryTemplate);
    }
}
