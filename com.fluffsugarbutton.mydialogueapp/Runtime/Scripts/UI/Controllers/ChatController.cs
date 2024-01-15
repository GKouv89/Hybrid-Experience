using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueApp
{
    public class ChatController
    {
        VisualTreeAsset firstMessageTemplate;
        VisualTreeAsset firstMessageRightTemplate;
        VisualTreeAsset plainMessageTemplate;
        VisualTreeAsset plainHintTemplate;
        VisualTreeAsset hintConfirmationTemplate;
        Queue<Message> listEntries;
        Character sender;
        Character self;
        ScrollView messageList;
        bool waitingForHint = false; 
        bool isFirstMsg = true;
        bool isActive = true; // if the screen is active, then the character will keep sending messages, if there are messages to send. 
        // otherwise, if for example we're back to the home screen, this will pause and start again once opening this chat's preview.

        public void Initialize(ChatView view, VisualElement root, Conversation convo, Character me, VisualTreeAsset firstMsgTemplate, VisualTreeAsset plainMsgTemplate, VisualTreeAsset firstMsgRightTemplate, VisualTreeAsset pHintTemplate, VisualTreeAsset hintConfTemplate)
        {
            view.ChatState += (state) => {
                isActive = state;
            };
            var characterName = root.Q<Label>("characterName");
            characterName.text = convo.Sender.charName;
            messageList = root.Q<ScrollView>("messages");
            firstMessageTemplate = firstMsgTemplate;
            firstMessageRightTemplate = firstMsgRightTemplate;
            plainMessageTemplate = plainMsgTemplate;
            plainHintTemplate = pHintTemplate;
            hintConfirmationTemplate = hintConfTemplate;
            listEntries = new Queue<Message>(convo.messages);
            sender = convo.Sender;
            self = me;
        }

        public IEnumerator TypeMessages()
        {
            TemplateContainer newListEntry;
                while(listEntries.Count > 0){
                    if(!isActive)
                        yield return null;
                    else{
                        if(!waitingForHint){
                            Message msg = listEntries.Dequeue();
                            if(msg.hasHint)
                            {
                                waitingForHint = true;
                                newListEntry = HintQuestion(msg);
                            }
                            else
                            {
                                if(isFirstMsg){
                                    newListEntry = FirstMessage(msg, sender);
                                    isFirstMsg = false;
                                }else{
                                    newListEntry = Message( msg);
                                }
                            }
                            messageList.Add(newListEntry);
                        }
                        yield return new WaitForSeconds(1f);
                    }
                }
        }

        void WantsHint(Message msg)
        {
            SendAMessageFromMe(msg.hint.hintLabel);
            Message newMsg = new(msg.hint.hintText);
            messageList.Add(FirstMessage(newMsg, sender));
            messageList.Add(Confirmation());
            waitingForHint = true;
        }

        void NoHint(string msgBody)
        {
            SendAMessageFromMe(msgBody);
        }

        void SendAMessageFromMe(string msgBody)
        {
            waitingForHint = !waitingForHint;

            Message msg = new(msgBody);
            messageList.RemoveAt(messageList.childCount-1);
            messageList.Add(FirstMessage(msg, self, false));
        }

        TemplateContainer Confirmation()
        {
            TemplateContainer newListEntry = hintConfirmationTemplate.Instantiate();
            IUIController newListEntryLogic = new HintController();
            SetHint(ref newListEntry, ref newListEntryLogic);
            return newListEntry;
        }

        TemplateContainer HintQuestion(Message msg)
        {
            TemplateContainer newListEntry = plainHintTemplate.Instantiate();
            IUIController newListEntryLogic = new HintQuestionController();
            SetHint(ref newListEntry, ref newListEntryLogic);
            ((HintQuestionController)newListEntryLogic).SetHint(msg);
            ((HintQuestionController)newListEntryLogic).OnUserWantsHint += () => {
                WantsHint(msg);
            };
            return newListEntry;
        }

        void SetHint(ref TemplateContainer newListEntry, ref IUIController newListEntryLogic)
        {
            SetUIElem(ref newListEntry, ref newListEntryLogic);
            ((HintController)newListEntryLogic).OnUserWantsNoHint += () => {
                NoHint("I've got this.");
                isFirstMsg = true;
            };
        }

        TemplateContainer FirstMessage(Message msg, Character sender, bool left = true)
        {
            TemplateContainer newListEntry = left ? firstMessageTemplate.Instantiate() : firstMessageRightTemplate.Instantiate();
            IUIController newListEntryLogic = new FirstMessageController();
            SetMessage(ref newListEntry, ref newListEntryLogic, msg);
            ((FirstMessageController) newListEntryLogic).SetSenderData(sender);
            return newListEntry;
        }

        void SetUIElem(ref TemplateContainer newListEntry, ref IUIController newListEntryLogic){
            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);
        }

        void SetMessage(ref TemplateContainer newListEntry, ref IUIController newListEntryLogic, Message msg)
        {
            SetUIElem(ref newListEntry, ref newListEntryLogic);
            ((MessageController)newListEntryLogic).SetMessageData(msg);
        }

        TemplateContainer Message(Message msg)
        {  
            TemplateContainer newListEntry = plainMessageTemplate.Instantiate();
            IUIController newListEntryLogic = new MessageController();
            SetMessage(ref newListEntry, ref newListEntryLogic, msg);
            return newListEntry;
        }
    }
}