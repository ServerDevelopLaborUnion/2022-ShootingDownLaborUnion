using System.ComponentModel;
using System;
using System.Net.WebSockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using System.Text;


public class Client : MonoSingleton<Client>
{
    Task _clentTask = null;
    ClientWebSocket _clientWebSocket = null;
    [SerializeField] ConnectionState _connectionState = ConnectionState.None;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _clentTask = Task.Run(ClientTask);
    }

    private async Task ClientTask()
    {
        _connectionState = ConnectionState.Connecting;
        Debug.Log("Connecting...");
        _clientWebSocket = new ClientWebSocket();

        var uri = new Uri("ws://localhost:3000/");
        await _clientWebSocket.ConnectAsync(uri, CancellationToken.None);

        if (_clientWebSocket.State == WebSocketState.Open)
        {
            _connectionState = ConnectionState.Connected;
            Debug.Log("Connected");

            var sendBuffer = new ArraySegment<byte>(new Byte[1024]);
            await _clientWebSocket.SendAsync(sendBuffer, WebSocketMessageType.Binary, true, CancellationToken.None);

            while (_clientWebSocket.State == WebSocketState.Open)
            {
                var buffer = new ArraySegment<byte>(new byte[1048576]);
                var result = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);

                Debug.Log(result.Count);
            }
        }

        _connectionState = ConnectionState.Disconnected;
        Debug.Log("Disconnected");
    }

    void Update()
    {
        if (_connectionState == ConnectionState.Disconnected)
            Initialize();
    }
}
