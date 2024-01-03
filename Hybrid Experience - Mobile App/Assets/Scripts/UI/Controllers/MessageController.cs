using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UIElements;

public class MessageController
{
    private VisualElement profilePicture;

    public void SetVisualElement(VisualElement elem)
    {
        profilePicture = elem.Q<VisualElement>("pfp");
    }

    public void SetMessageData(Character Sender)
    {
        profilePicture.style.backgroundImage = new StyleBackground(Sender.charSprite);
    }
}
