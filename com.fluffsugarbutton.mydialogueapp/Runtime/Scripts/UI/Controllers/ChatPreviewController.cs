using UnityEngine.UIElements;
using UnityEngine;

namespace DialogueApp
{
    public class ChatPreviewController : FirstMessageController {
    bool hasChatBeenOpened = false;
    bool HasChatBeenOpened{
        get {return hasChatBeenOpened;}
        set{
            if(value && !hasChatBeenOpened)
                EnableChat();
            hasChatBeenOpened = value;
        }
    }
    GameObject panel;
    public override void SetVisualElement(VisualElement elem)
    {
        base.SetVisualElement(elem);
        body.style.display = DisplayStyle.None;
        elem.AddManipulator(new Clickable(evt => {
            HasChatBeenOpened = true;
            ShowChat();
            Debug.Log("Clicky clicky");
        }));
    }

    public void SetPanel(GameObject panel)
    {
        this.panel = panel;
    }

    void EnableChat()
    {
        panel.SetActive(true);
    }

    void ShowChat()
    {
        panel.GetComponent<ChatView>().ShowChat();
    }
}
}