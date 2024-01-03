using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatView : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset messageTemplate;
    [SerializeField]
    Conversation aConvo;
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        ChatController controller = new();
        controller.Initialize(uiDocument.rootVisualElement, aConvo);
        controller.InitializeMessageList(uiDocument.rootVisualElement, messageTemplate, aConvo.messages, aConvo.Sender);
    }
}
