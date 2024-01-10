using UnityEngine;
using UnityEngine.UIElements;

public class HintController : IUIController
{
    private Button confirm;
    private bool wantsHint = false;
    public bool UserWantsHint{
        get {return wantsHint;}
        set {
            Debug.Log("wantsHint: " + wantsHint);
            wantsHint = value;
            if(wantsHint)
            {
                OnUserWantsHint();
            }
            else
            {
                OnUserWantsNoHint();
            }
        }
    }
    public delegate void OnHintStatusChangeDelegate();
    public event OnHintStatusChangeDelegate OnUserWantsHint;
    public event OnHintStatusChangeDelegate OnUserWantsNoHint;

    public virtual void SetVisualElement(VisualElement elem)
    {
        confirm = elem.Q<Button>("Confirm");
        confirm.clickable.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        confirm.clicked += NoHint;
    }

    void NoHint()
    {
        UserWantsHint = false;
        DisableCurrentHint();
    }

    protected virtual void  DisableCurrentHint()
    {
        confirm.clicked -= NoHint;
        OnUserWantsHint = null;
        OnUserWantsNoHint = null;
    }
}
