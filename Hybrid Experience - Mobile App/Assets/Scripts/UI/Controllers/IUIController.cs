using UnityEngine.UIElements;

interface IUIController
{
    public void SetVisualElement(VisualElement elem);
}

interface IMessageController : IUIController
{
    public void SetMessageData(Message msg);
}