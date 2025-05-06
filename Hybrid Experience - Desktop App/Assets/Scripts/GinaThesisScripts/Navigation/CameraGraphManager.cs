using UnityEngine;
using System.Collections.Generic;

public class CameraGraphManager : MonoBehaviour
{
    public List<CameraNode> cameraNodes = new List<CameraNode>();
    [SerializeField] private CameraNode currentNode;

    public void MoveInDirection(MovementDirection direction)
    {
        if (currentNode == null)
        {
            Debug.LogWarning("Current camera node is not set.");
            return;
        }

        CameraNode.NodeConnection connection = currentNode.connections.Find(c => c.direction == direction);

        if (connection != null && connection.targetNode != null)
        {
            // Move camera logic here
        }
        else
        {
            Debug.Log($"No available camera node in direction: {direction}");
        }
    }
}
