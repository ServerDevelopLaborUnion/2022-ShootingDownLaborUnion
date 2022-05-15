using System.ComponentModel;
using System;
using System.Net.WebSockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.IO;
using System.Linq;

public class Client : MonoSingleton<Client>
{
    #region Client Event Arguments
    public class ConnectionEventArgs : EventArgs
    {
        public string SessionId { get; set; }

        public ConnectionEventArgs(string sessionId)
        {
            SessionId = sessionId;
        }
    }

    public class LoginResponseEventArgs : EventArgs
    {
        public bool Success { get; set; }
        public string Token { get; set; }

        public LoginResponseEventArgs(bool success, string token)
        {
            Success = success;
            Token = token;
        }
    }

    public class CreateEntityEventArgs : EventArgs
    {
        public Entity Entity { get; set; }

        public CreateEntityEventArgs(Entity entity)
        {
            Entity = entity;
        }
    }

    public class MoveEntityEventArgs : EventArgs
    {
        public int EntityId { get; set; }
        public Vector2 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public MoveEntityEventArgs(int entityId, Vector2 position, Quaternion rotation)
        {
            EntityId = entityId;
            Position = position;
            Rotation = rotation;
        }
    }
    #endregion

    #region WebSocket Events
    public static event Action<byte[], WebSocketReceiveResult> OnMessageReceived;
    public static event Action<string> OnConnected;
    public static event Action<string> OnDisconnected;
    #endregion

    #region Client Events
    public static event EventHandler<ConnectionEventArgs> OnConnectionMessage;
    public static event EventHandler<LoginResponseEventArgs> OnLoginResponseMessage;
    public static event EventHandler<CreateEntityEventArgs> OnCreateEntityMessage;
    public static event EventHandler<MoveEntityEventArgs> OnMoveEntityMessage;
    #endregion

    [SerializeField] ConnectionState _connectionState = ConnectionState.None;
    Task _clentTask = null;
    ClientWebSocket _clientWebSocket = null;

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
        try
        {
            _connectionState = ConnectionState.Connecting;
            Debug.Log("Connecting...");
            _clientWebSocket = new ClientWebSocket();

            var uri = new Uri("ws://localhost:3000/");
            await _clientWebSocket.ConnectAsync(uri, CancellationToken.None);

            if (_clientWebSocket.State == WebSocketState.Open)
            {
                _connectionState = ConnectionState.Connected;
                OnConnected?.Invoke("Connected");
                Debug.Log("Connected");

                var memoryStream = new MemoryStream();

                while (_clientWebSocket.State == WebSocketState.Open)
                {
                    var buffer = new ArraySegment<byte>(new byte[1024]);
                    var result = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                        _connectionState = ConnectionState.Disconnected;
                        OnDisconnected?.Invoke("Disconnected");
                        Debug.Log("Disconnected");
                        break;
                    }
                    else
                    {
                        memoryStream.Write(buffer.Array, 0, result.Count);
                        if (result.EndOfMessage)
                        {
                            OnMessageReceived?.Invoke(memoryStream.ToArray(), result);
                            ReceiveMessage(memoryStream.ToArray(), result);
                            memoryStream.SetLength(0);
                        }
                    }
                }
            }

            _connectionState = ConnectionState.Disconnected;
            OnDisconnected?.Invoke("Disconnected");
            Debug.Log("Disconnected");
        }
        catch (Exception ex)
        {
            _connectionState = ConnectionState.Disconnected;
            Debug.LogWarning(ex.Message);
        }
    }

    void ReceiveMessage(byte[] buffer, WebSocketReceiveResult result)
    {
        if (result.MessageType == WebSocketMessageType.Text)
        {
            var text = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Debug.Log(text);
        }
        else
        {
            Debug.Log("Received binary data. Length: " + result.Count);
            int type = BitConverter.ToUInt16(buffer, 0);
            buffer = buffer.Skip(2).ToArray();

            switch (type)
            {
                case 0:
                    var connectionMessage = Protobuf.Client.Connection.Parser.ParseFrom(buffer);
                    OnConnectionMessage?.Invoke(this, new ConnectionEventArgs(connectionMessage.SessionId));
                    break;
                case 1:
                    var loginResponseMessage = Protobuf.Client.LoginResponse.Parser.ParseFrom(buffer);
                    OnLoginResponseMessage?.Invoke(this, new LoginResponseEventArgs(loginResponseMessage.Success, loginResponseMessage.Token));
                    break;
                case 2:
                    var createEntityMessage = Protobuf.Client.CreateEntity.Parser.ParseFrom(buffer);
                    OnCreateEntityMessage?.Invoke(this, new CreateEntityEventArgs(
                        new Entity(createEntityMessage.Entity.Id, createEntityMessage.Entity.Name,
                            new Vector2(createEntityMessage.Entity.Position.X, createEntityMessage.Entity.Position.Y),
                            new Quaternion(createEntityMessage.Entity.Rotation.X, createEntityMessage.Entity.Rotation.Y, createEntityMessage.Entity.Rotation.Z, createEntityMessage.Entity.Rotation.W))
                    ));
                    break;
                case 3:
                    var moveEntityMessage = Protobuf.Client.MoveEntity.Parser.ParseFrom(buffer);
                    OnMoveEntityMessage?.Invoke(this, new MoveEntityEventArgs(moveEntityMessage.EntityId, 
                        new Vector2(moveEntityMessage.Position.X, moveEntityMessage.Position.Y), 
                        new Quaternion(moveEntityMessage.Rotation.X, moveEntityMessage.Rotation.Y, moveEntityMessage.Rotation.Z, moveEntityMessage.Rotation.W)
                    ));
                    break;
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (_clientWebSocket != null)
        {
            _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            _connectionState = ConnectionState.Disconnected;
            OnDisconnected?.Invoke("Disconnected");
        }
    }

    void Update()
    {
        if (_connectionState == ConnectionState.Disconnected)
            Initialize();
    }
}
