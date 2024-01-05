using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatController
{
    VisualTreeAsset firstMessageTemplate;
    VisualTreeAsset plainMessageTemplate;
    VisualTreeAsset plainHintTemplate;
    ScrollView messageList;
    public void Initialize(VisualElement root, Conversation convo)
    {
        var characterName = root.Q<Label>("characterName");
        characterName.text = convo.Sender.charName;
    }

    public void InitializeMessageList(VisualElement root, VisualTreeAsset firstMsgTemplate, VisualTreeAsset plainMsgTemplate, VisualTreeAsset pHintTemplate, List<Message> messages, Character sender)
    {
        messageList = root.Q<ScrollView>("messages");
        firstMessageTemplate = firstMsgTemplate;
        plainMessageTemplate = plainMsgTemplate;
        plainHintTemplate = pHintTemplate;
        FillMessageList(messages, sender);
    }

    public void FillMessageList(List<Message> messages, Character sender)
    {
        bool isFirstMsg = true;
        IUIController newListEntryLogic;
        TemplateContainer newListEntry;
        foreach (Message msg in messages)
        {
            if(msg.hasHint)
            {
                newListEntry = plainHintTemplate.Instantiate();
                newListEntryLogic = new HintController();
            }
            else
            {
                newListEntryLogic = isFirstMsg ? new FirstMessageController() : new MessageController();
                newListEntry = isFirstMsg ? firstMessageTemplate.Instantiate() : plainMessageTemplate.Instantiate();
            }
            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);
            newListEntryLogic.SetMessageData(msg);
            if(isFirstMsg)
                ((FirstMessageController)newListEntryLogic).SetSenderData(sender);
            messageList.Add(newListEntry);
            isFirstMsg = false;
        }
    }
}
