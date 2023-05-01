using System;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

// using Debug = System.Diagnostics.Debug;


public class TestConnect : MonoBehaviour
{
    public SocketIOUnity socket;

    // Start is called before the first frame update
    void Start()
    {
        var uri = new Uri("http://localhost:8000/");
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
                {
                    {"token", "UNITY" }
                }
            ,
            EIO = 4
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        socket.JsonSerializer = new NewtonsoftJsonSerializer();

        ///// reserved socketio events
        socket.OnConnected += (sender, e) =>
        {
            // Debug.Print("socket.OnConnected");
            Debug.Log("socket.OnConnected");
            // socket.Emit("begin_chat");
            socket.EmitStringAsJSON("begin_chat", "{\"roomNo\": 0, \"type\": \"desktop\"}");
        };
        
        socket.OnDisconnected += (sender, e) =>
        {
            // Debug.Print("disconnect: " + e);
            Debug.Log("socket.OnDisconnected");
        };
        socket.OnReconnectAttempt += (sender, e) =>
        {
            // Debug.Print($"{DateTime.Now} Reconnecting: attempt = {e}");
            Debug.Log("socket.OnReconnectAttempt");
        };
        // ////
        socket.On("serverMessage", (response) => {
            Debug.Log("in serverMessage response handler");
            String res = response.GetValue<String>();
            Debug.Log(res);
        });

        Debug.Log("Connecting...");
        socket.Connect();
    }

    public void EmitTest(){
        // Texture2D tex = new Texture2D(128, 128);
        // Color[] pixels= tex.GetPixels();
        // Color fillColor = Color.white;
 
        // for(int i = 0; i < pixels.Length; ++i)
        // {
        //     pixels[i] = fillColor;
        // }
        
        // tex.SetPixels(pixels);
        // tex.Apply();


        // byte[] texbytes = tex.EncodeToPNG();
        
        // // socket.Emit("hello", "hi from desktop");
        // socket.Emit("hello", texbytes);
        socket.Emit("helloToRoomDesktop", "hi from desktop");
    }

    void OnDestroy()
    {
        socket.Disconnect();
        // Debug.Print("Disconnected");
        Debug.Log("Disconnected");
    }
}