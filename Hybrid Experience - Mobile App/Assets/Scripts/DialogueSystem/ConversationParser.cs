using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UIElements;

public class ConversationParser
{
    public static Conversation LoadConversation(string ConversationName, string base_url)
    {
        string path = Path.Combine(base_url, ConversationName + ".txt");
        Conversation loading = ScriptableObject.CreateInstance<Conversation>();
        loading.conversationName = ConversationName;
        try
        {
            using StreamReader sr = File.OpenText(path);
            Message msg;
            // Reading the first line will allow us to know which character to assign as the sender
            string characterName = sr.ReadLine();
            Character sender = (Character) AssetDatabase.LoadAssetAtPath($"Assets/Scripts/DialogueSystem/Characters/{characterName}.asset", typeof(Character));
            loading.Sender = sender;
            while (sr.Peek() >= 0)
            {
                string line = sr.ReadLine();
                Debug.Log(line);
                if(sr.Peek() == '[')
                {
                    string hintLabel = sr.ReadLine();
                    hintLabel = hintLabel[1..^1];
                    string hintText = sr.ReadLine();
                    hintText = hintText[1..^1];
                    msg = new(line, hintLabel, hintText);
                }
                else
                {
                    msg = new(line);
                }
                loading.messages.Add(msg);
            }
            return loading;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }
}
