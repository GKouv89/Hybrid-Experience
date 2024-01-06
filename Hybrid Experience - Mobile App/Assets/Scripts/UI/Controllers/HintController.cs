using UnityEngine;
using UnityEngine.UIElements;

public class HintController : IUIController
{
    private Button hint;
    private Button confirm;
    private bool hintStatus = true;
    private bool WaitingForHint{
        get {return hintStatus;}
        set {
            hintStatus = value;
            OnHintStatusChange();
        }
    }
    public delegate void OnHintStatusChangeDelegate();
    public event OnHintStatusChangeDelegate OnHintStatusChange;

    public void SetVisualElement(VisualElement elem)
    {
        hint = elem.Q<Button>("Hint");
        confirm = elem.Q<Button>("Confirm");
        hint.clickable.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        hint.clicked += ChangeHintStatus;
        confirm.clickable.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        confirm.clicked += ChangeHintStatus;
    }

    public void SetMessageData(Message msg)
    {
        hint.text = msg.hint.hintLabel;
    }

    void ChangeHintStatus()
    {
        WaitingForHint = false;
        DisableCurrentHint();
    }

    void DisableCurrentHint()
    {
        hint.clicked -= ChangeHintStatus;
        confirm.clicked -= ChangeHintStatus;
        OnHintStatusChange = null;
    }
}
