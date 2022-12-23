using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;

public class TestSocketConnection : MonoBehaviour
{
    public SocketIOUnity socket;
    public Button emitButton;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("clicked");
        var uri = new Uri("http://192.168.1.11:8000/");
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
            Debug.Log("socket.OnConnected");
            socket.EmitStringAsJSON("begin_chat", "{\"roomNo\": 0, \"type\": \"mobile\"}");
            Debug.Log("blah");
            EnableEmitButton();
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
            Debug.Log(response);
            String res = response.GetValue<String>();
            Debug.Log(res);
        });

        Debug.Log("Connecting...");
        socket.Connect();        
    }

    void EnableEmitButton(){
        emitButton.gameObject.SetActive(true);
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
        
        // socket.Emit("helloMobile", "hi from cellphone");
        // socket.Emit("hello", texbytes);
        socket.Emit("helloToRoomMobile", "hi from mobile");
    }

    public void Update(){
        // Debug.Log(emitButton);
        // Debug.Log(emitButton.IsActive());
        emitButton.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        socket.Disconnect();
        // Debug.Print("Disconnected");
        Debug.Log("Disconnected");
    }
}
