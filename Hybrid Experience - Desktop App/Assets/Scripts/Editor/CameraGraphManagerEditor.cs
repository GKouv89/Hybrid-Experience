// Editor/CameraGraphManagerEditor.cs
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraGraphManager))]
public class CameraGraphManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CameraGraphManager manager = (CameraGraphManager)target;
        SerializedProperty cameraNodesProp = serializedObject.FindProperty("cameraNodes");
        
        EditorGUILayout.PropertyField(cameraNodesProp, new GUIContent("Camera Nodes"), true);

        if (GUILayout.Button("Capture Scene View as New Camera Node"))
        {
            CreateNodeFromSceneView(manager);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void CreateNodeFromSceneView(CameraGraphManager manager)
    {
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView == null)
        {
            Debug.LogWarning("No active scene view.");
            return;
        }

        // Create a new GameObject for the camera node
        GameObject nodeObj = new GameObject($"CameraNode_{manager.cameraNodes.Count}");
        nodeObj.transform.SetPositionAndRotation(
            sceneView.camera.transform.position,
            sceneView.camera.transform.rotation
        );

        // Add CameraNode component
        CameraNode newNode = nodeObj.AddComponent<CameraNode>();
        newNode.nodeName = nodeObj.name;

        // Add to manager's list
        manager.cameraNodes.Add(newNode);
        
        // Mark dirty
        EditorUtility.SetDirty(manager);
        EditorUtility.SetDirty(newNode);
    }
}
