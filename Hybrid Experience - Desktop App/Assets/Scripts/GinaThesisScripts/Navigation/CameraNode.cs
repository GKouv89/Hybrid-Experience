using UnityEngine;
using System;
using System.Collections.Generic;

public enum MovementDirection { Forward, Left, Right, Backward }

public class CameraNode : MonoBehaviour
{
    public string nodeName;
    
    [Serializable]
    public class NodeConnection
    {
        public MovementDirection direction;
        public CameraNode targetNode; // Remove [System.NonSerialized]
    }

    public List<NodeConnection> connections = new List<NodeConnection>();
}
