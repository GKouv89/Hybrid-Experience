using UnityEngine;
using UnityEngine.UIElements;

public class HintQuestionController : HintController
{
    private Button hint;

    public override void SetVisualElement(VisualElement elem)
    {
        base.SetVisualElement(elem);
        hint = elem.Q<Button>("Hint");
        hint.clickable.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        hint.clicked += WantsHint;
    }

    public void SetHint(Message msg)
    {
        Debug.Log("Hint: " + hint);
        Debug.Log("Message: " + msg);
        hint.text = msg.hint.hintLabel;
    }

    void WantsHint()
    {
        UserWantsHint = true;
        DisableCurrentHint();
    }

    protected override void DisableCurrentHint()
    {
        base.DisableCurrentHint();
        hint.clicked -= WantsHint;
    }
}
