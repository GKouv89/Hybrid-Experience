using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Hint
{
    [TextAreaAttribute]
    public string hintLabel;
    [TextAreaAttribute]
    public string hintText;

    public Hint(string label, string text)
    {
        hintLabel = label;
        hintText = text;
    }
}

[Serializable]
public struct Message
{
    [TextAreaAttribute]
    public string messageBody;
    public bool hasHint; // Nice to add: Only show hintText when hasHint is true. Custom inspector windows with UI Toolkit.  
    [SerializeField]
    public Hint? hint;

    public Message(string body)
    {
        messageBody = body;
        hasHint = false;
        hint = null;
    }

    public Message(string body, Hint h)
    {
        hasHint = true;
        hint = h;
        messageBody = body;
    }
}

public class Conversation : ScriptableObject
{
    public string conversationName;
    public List<Message> messages = new();
    public Character Sender;
}
