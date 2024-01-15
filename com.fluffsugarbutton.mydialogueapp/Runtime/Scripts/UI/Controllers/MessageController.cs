using UnityEngine.UIElements;

namespace DialogueApp
{
    public class MessageController : IMessageController
    {
        protected Label body;
        public virtual void SetVisualElement(VisualElement elem){
            body = elem.Q<Label>("Body");
        }
        public virtual void SetMessageData(Message msg){
            body.text = msg.messageBody;
        }
    }
}