using UnityEngine;
using UnityEngine.UIElements;

public class HintController : IUIController
{
    private Button hint;
    public void SetVisualElement(VisualElement elem)
    {
        hint = elem.Q<Button>("Hint");
    }

    public void SetMessageData(Message msg)
    {
        hint.text = msg.hint.hintLabel;
    }
}
