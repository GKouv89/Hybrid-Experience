using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatController
{
    VisualTreeAsset firstMessageTemplate;
    VisualTreeAsset plainMessageTemplate;
    VisualTreeAsset plainHintTemplate;
    Queue<Message> listEntries;
    Character sender;
    ScrollView messageList;
    bool waitingForHint = false; 
    public void Initialize(VisualElement root, Conversation convo, VisualTreeAsset firstMsgTemplate, VisualTreeAsset plainMsgTemplate, VisualTreeAsset pHintTemplate)
    {
        var characterName = root.Q<Label>("characterName");
        characterName.text = convo.Sender.charName;
        messageList = root.Q<ScrollView>("messages");
        firstMessageTemplate = firstMsgTemplate;
        plainMessageTemplate = plainMsgTemplate;
        plainHintTemplate = pHintTemplate;
        listEntries = new Queue<Message>(convo.messages);
        sender = convo.Sender;
    }

    public IEnumerator TypeMessages()
    {
        bool isFirstMsg = true;
        IUIController newListEntryLogic;
        TemplateContainer newListEntry;

        while(listEntries.Count > 0){
            if(!waitingForHint){
                Message msg = listEntries.Dequeue();
                if(msg.hasHint)
                {
                    newListEntry = plainHintTemplate.Instantiate();
                    newListEntryLogic = new HintController();
                    waitingForHint = true;
                    ((HintController)newListEntryLogic).OnHintStatusChange += () => {
                        waitingForHint = !waitingForHint;
                    };
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
            yield return new WaitForSeconds(1f);
        }
    }
}
