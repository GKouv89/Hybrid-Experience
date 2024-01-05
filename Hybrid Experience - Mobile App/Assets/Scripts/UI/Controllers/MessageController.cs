using UnityEngine.UIElements;

public class MessageController : IUIController
{
    protected Label body;
    public virtual void SetVisualElement(VisualElement elem){
        body = elem.Q<Label>("Body");
    }
    public virtual void SetMessageData(Message msg){
        body.text = msg.messageBody;
    }
}