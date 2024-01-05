using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UIElements;

public class FirstMessageController : MessageController
{
    private VisualElement profilePicture;
    private Label senderName;
    public override void SetVisualElement(VisualElement elem)
    {
        base.SetVisualElement(elem);
        profilePicture = elem.Q<VisualElement>("pfp");
        senderName = elem.Q<Label>("Sender");
    }

    public void SetSenderData(Character Sender)
    {
        profilePicture.style.backgroundImage = new StyleBackground(Sender.charSprite);
        senderName.text = Sender.charName;
    }
}
