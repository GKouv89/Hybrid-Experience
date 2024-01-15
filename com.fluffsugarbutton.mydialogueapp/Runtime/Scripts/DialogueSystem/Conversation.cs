using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueApp
{
    [Serializable]
    public struct Hint
    {
        public string hintLabel;
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
        public Hint hint; // Normally I would have added this as nullable, but unity doesn't serialize nullable types. So, I end up having to create
        // a hint but adding null fields to it. 

        public Message(string body)
        {
            messageBody = body;
            hasHint = false;
            hint = new(null, null);
        }

        public Message(string body, string hintLabel, string hintText)
        {
            hasHint = true;
            hint = new Hint(hintLabel, hintText);
            messageBody = body;
        }
    }

    public class Conversation : ScriptableObject
    {
        public string conversationName;
        public List<Message> messages = new();
        public Character Sender;
    }
}