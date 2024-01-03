using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MessageView : MonoBehaviour
{
    [SerializeField]
    // VisualTreeAsset MessageTemplate;

    void OnEnable()
    {
        // // Generate a sample msg
        // Message sample = new("This is a very veryveryveryveryveryveryveryveryveryveryveryveryveryveryveryveryveryveryveryveryveryveryveryveryveryveryvery long sample message");
        // Character sender = (Character) AssetDatabase.LoadAssetAtPath($"Assets/Scripts/DialogueSystem/Characters/Alan.asset", typeof(Character));
        Conversation convo = (Conversation) AssetDatabase.LoadAssetAtPath("Assets/Scripts/DialogueSystem/Conversations/IntroductionTest.asset", typeof(Conversation));
        Message sample = convo.messages[0];
        Character sender = convo.Sender;
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
        Debug.Log("UIDOcument: " + uiDocument);

        var profilePicture = uiDocument.rootVisualElement.Q<VisualElement>("pfp");
        profilePicture.style.backgroundImage = new StyleBackground(sender.charSprite);
        Label senderName = uiDocument.rootVisualElement.Q<Label>("Sender");
        senderName.text = sender.charName;
        Label messageBody = uiDocument.rootVisualElement.Q<Label>("Body");
        messageBody.text = sample.messageBody;
    }
}
