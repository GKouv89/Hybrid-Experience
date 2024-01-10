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
    VisualTreeAsset hintConfirmationTemplate;
    [SerializeField]
    Conversation aConvo;
    [SerializeField]
    Character self;
    [SerializeField]
    VisualTreeAsset firstMessageRightTemplate;
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        ChatController controller = new();
        controller.Initialize(uiDocument.rootVisualElement, aConvo, self, firstMessageTemplate, plainMessageTemplate, firstMessageRightTemplate, hintTemplate, hintConfirmationTemplate);
        StartCoroutine(controller.TypeMessages());
    }
}
