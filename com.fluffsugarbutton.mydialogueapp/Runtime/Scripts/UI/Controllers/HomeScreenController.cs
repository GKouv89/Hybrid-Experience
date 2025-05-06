using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueApp
{
    public class HomeScreenController
    {
        VisualElement root;
        VisualTreeAsset chatPreviewTemplate;
        VisualElement chatsContainer;
        List<Conversation> chats;
        List<GameObject> panels;
        public void Initialize(VisualElement root, List<Conversation> chats, List<GameObject> panels, VisualTreeAsset chatPreview){
            Debug.Log("HomeScreenController Initialization");
            this.root = root;
            chatsContainer = root.Q<VisualElement>("HomeScreen");
            chatPreviewTemplate = chatPreview;
            this.chats = chats;
            this.panels = panels;
        }

        public void FillHomeScreen()
        {
            TemplateContainer newListEntry;
            ChatPreviewController newListEntryLogic;
            int panelNo = 0;
            foreach(Conversation chat in chats)
            {
                newListEntry = chatPreviewTemplate.Instantiate();
                newListEntryLogic = new();
                newListEntry.userData = newListEntryLogic;
                newListEntryLogic.SetVisualElement(newListEntry);
                newListEntryLogic.SetSenderData(chat.Sender);
                GameObject currPanel = panels[panelNo];
                newListEntryLogic.SetPanel(currPanel);
                currPanel.GetComponent<ChatView>().ChatState += (state) => ToggleHomeScreen(state);
                chatsContainer.Add(newListEntry);
                panelNo++;
            }
            DebugMe();
        }

        void ToggleHomeScreen(bool state)
        {
            Debug.Log("Home screen will " + (state ? "close" : "open"));
            root.style.display = state ? DisplayStyle.None : DisplayStyle.Flex;
        }

        public void DebugMe()
        {
            Debug.Log("HomeScreenController has this many entries: " + chatsContainer.childCount);
            for(int i = 0; i < chatsContainer.childCount; i++)
            {
                Debug.Log(chatsContainer[i].userData);
                Debug.Log(((FirstMessageController) chatsContainer[i].userData).senderName);
            }
        }
    }
}
