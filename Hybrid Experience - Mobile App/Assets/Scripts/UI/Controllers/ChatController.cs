using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatController
{
    VisualTreeAsset messageTemplate;
    ScrollView messageList;
    public void Initialize(VisualElement root, Conversation convo)
    {
        var characterName = root.Q<Label>("characterName");
        characterName.text = convo.Sender.charName;
    }

    public void InitializeMessageList(VisualElement root, VisualTreeAsset msgTemplate, List<Message> messages, Character sender)
    {
        messageList = root.Q<ScrollView>("messages");
        messageTemplate = msgTemplate;
        FillMessageList(messages, sender);
    }

    public void FillMessageList(List<Message> messages, Character sender)
    {
        for (var i = 0; i < messages.Count; i++)
        {
            var newListEntry = messageTemplate.Instantiate();
            var newListEntryLogic = new MessageController();
            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);
            newListEntryLogic.SetMessageData(sender);
            if(i == 2)
            {
                // Testing how a long message looks like
                var msgBody = newListEntry.Q<Label>("Body");
                msgBody.text = "This is a very very very very very very very very very very long message.";
            }
            messageList.Add(newListEntry);
        }
    }
}
