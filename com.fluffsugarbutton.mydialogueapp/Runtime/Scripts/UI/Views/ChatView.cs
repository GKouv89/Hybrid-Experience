using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueApp
{
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

        bool isChatOpen = false;
        bool IsChatOpen{
            get{return isChatOpen;}
            set{
                isChatOpen = value;
                ChatState(isChatOpen);
            }
        }
        public delegate void isChatOpenDelegate(bool isChatOpen);
        public event isChatOpenDelegate ChatState;

        Button backButton;
        void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            backButton = uiDocument.rootVisualElement.Q<Button>("backButton");
            backButton.clickable.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
            backButton.clicked += () => HideChat();

            ChatController controller = new();
            controller.Initialize(this, uiDocument.rootVisualElement, aConvo, self, firstMessageTemplate, plainMessageTemplate, firstMessageRightTemplate, hintTemplate, hintConfirmationTemplate);
            StartCoroutine(controller.TypeMessages());
        }

        public void ShowChat()
        {
            GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.Flex;
            IsChatOpen = true;
        }

        public void HideChat()
        {
            GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
            IsChatOpen = false;
        }
    }
}
