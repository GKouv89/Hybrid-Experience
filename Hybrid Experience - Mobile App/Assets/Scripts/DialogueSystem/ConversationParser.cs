using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

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
                    string hintText = sr.ReadLine();
                    Hint hint = new(hintLabel, hintText);
                    msg = new(line, hint);
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
