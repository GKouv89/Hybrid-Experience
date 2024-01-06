using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatView : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset firstMessageTemplate;
    [SerializeField]
    VisualTreeAsset plainMessageTemplate;
    [SerializeField]
    VisualTreeAsset hintTemplate;
    [SerializeField]
    Conversation aConvo;
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        ChatController controller = new();
        controller.Initialize(uiDocument.rootVisualElement, aConvo, firstMessageTemplate, plainMessageTemplate, hintTemplate);
        StartCoroutine(controller.TypeMessages());
    }
}
