using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;

namespace DialogueApp
{
    public class ConversationCreator : EditorWindow
    {
        string conversationsPath = "Assets/Scripts/DialogueSystem/Conversations";
        string conversationName;
        string assetPath = "Assets/Scripts/DialogueSystem/Conversations";
        [MenuItem("Window / Custom Controls")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<ConversationCreator>("Conversation Creator", typeof(SceneView));
        }
        private void OnGUI()
        {
            conversationsPath = EditorGUILayout.TextField("Path to read convo from: ", conversationsPath);
            conversationName = EditorGUILayout.TextField("Conversation Name: ", conversationName);
            assetPath = EditorGUILayout.TextField("Path for asset storage: ", assetPath);

            if(GUILayout.Button("Read Conversation", GUILayout.Width(150), GUILayout.Height(25)))
            {
                Conversation convo = ConversationParser.LoadConversation(conversationName, conversationsPath);
                foreach (Message msg in convo.messages)
                {
                    if(msg.hasHint){
                        Debug.Log(msg.hint.hintLabel);
                        Debug.Log(msg.hint.hintText);
                    }
                }
                string path = $"{assetPath}/{conversationName}.asset";
                AssetDatabase.CreateAsset(convo, path);
                AssetDatabase.SaveAssets();
            }
        }
    }
}