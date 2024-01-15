using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HomeScreenView : MonoBehaviour
{
    [SerializeField]
    List<Conversation> chats;
    [SerializeField]
    VisualTreeAsset chatPreviewTemplate;
    [SerializeField]
    List<GameObject> panels; // the panels correspond one to one to the chats.
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        HomeScreenController controller = new();
        controller.Initialize(uiDocument.rootVisualElement, chats, panels, chatPreviewTemplate);
        controller.FillHomeScreen();
    }
}
