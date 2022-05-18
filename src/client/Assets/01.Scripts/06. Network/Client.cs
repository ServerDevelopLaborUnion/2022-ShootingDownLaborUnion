using System;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace WebSocket
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
        public string UserUUID { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

        public LoginResponseEventArgs(bool success, string userUUID, string username, string token)
        {
            Success = success;
            UserUUID = userUUID;
            Username = username;
            Token = token;
        }
    }

    public class CreateEntityEventArgs : EventArgs
    {
        public EntityData Data { get; set; }

        public CreateEntityEventArgs(EntityData data)
        {
            Data = data;
        }
    }

    public class MoveEntityEventArgs : EventArgs
    {
        public string EntityId { get; set; }
        public Vector2 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public MoveEntityEventArgs(string entityId, Vector2 position, Quaternion rotation)
        {
            EntityId = entityId;
            Position = position;
            Rotation = rotation;
        }
    }
    #endregion

    public class Client : MonoSingleton<Client>
    {
        [RuntimeInitializeOnLoadMethod]
        static void OnSecondRuntimeMethodLoad()
        {
            Initialize(true);
        }

        #region WebSocket Events
        
        public static event Action<string> OnConnected;
        
        public static event Action<byte[], WebSocketReceiveResult> OnMessageReceived;
        
        public static event Action<string> OnDisconnected;
        
        public static event Action<ConnectionState> OnConnectionStateChanged;
        #endregion

        #region Client Events
        
        public static event EventHandler<ConnectionEventArgs> OnConnectionMessage;
        
        public static event EventHandler<LoginResponseEventArgs> OnLoginResponseMessage;
        
        public static event EventHandler<CreateEntityEventArgs> OnCreateEntityMessage;
        
        public static event EventHandler<MoveEntityEventArgs> OnMoveEntityMessage;
        #endregion
        
        public static Account Account { get; private set; }

        public static ConnectionState ConnectionState => _connectionState;
        private static ConnectionState _connectionState { 
            get => _connectionStateValue;
            set 
            {
                _connectionStateValue = value;
                MainTask.Enqueue(() => OnConnectionStateChanged?.Invoke(value));
            }
        }
        private static ConnectionState _connectionStateValue = ConnectionState.None;
        private static Task _clentTask = null;
        private static ClientWebSocket _clientWebSocket = null;

        #region WebSocket Methods

        public void Initialize()
        {
            if (_clientWebSocket != null)
            {
                _clientWebSocket.Dispose();
                _clientWebSocket = null;
            }

            ClearEvents();
            _clentTask = Task.Run(ClientTask);
        }

        public static void  SendPacket(uint type, Google.Protobuf.IMessage message)
        {
            if (_connectionState == ConnectionState.Connected)
            {
                byte[] buffer = new byte[message.CalculateSize()];
                using (var memoryStream = new Google.Protobuf.CodedOutputStream(buffer))
                {
                    message.WriteTo(memoryStream);
                }

                SendPacket(type, buffer);
            }
            else if (_connectionState == ConnectionState.Disconnected)
            {
                Debug.LogWarning("You are not connected to the server.");
            }
        }

        public static void SendPacket(uint type, byte[] data)
        {
            if (_clientWebSocket == null)
                return;

            if (_clientWebSocket.State != WebSocketState.Open)
                return;

            _clientWebSocket.SendAsync(new ArraySegment<byte>(Packet.Make((ushort)type, data)), WebSocketMessageType.Binary, true, CancellationToken.None);
        }

        public static void Disconnect()
        {
            if (_clientWebSocket != null)
            {
                _clientWebSocket.Dispose();
                _clientWebSocket = null;
            }
            _connectionState = ConnectionState.Disconnected;
            MainTask.Enqueue(() => OnDisconnected?.Invoke("Disconnected"));
            Debug.Log("Disconnected");
        }

        public static void SendMessage(byte[] buffer)
        {
            if (_clientWebSocket.State == WebSocketState.Open)
            {
                _clientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            Initialize();

            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void Update()
        {
            if (_connectionState == ConnectionState.Disconnected)
                Initialize();
        }

        private void OnApplicationQuit()
        {
            Disconnect();
        }

        private void OnSceneUnloaded(Scene scene)
        {
            ClearEvents();
        }

        private static void InitializeEvents()
        {
            OnLoginResponseMessage += (sender, e) =>
            {
                if (e.Success)
                {
                    _connectionState = ConnectionState.LoggedIn;
                    Account = new Account(e.UserUUID, e.Username, e.Token);
                    Debug.Log($"Logged in as {e.Username}");
                    Debug.Log($"User UUID: {e.UserUUID}");
                    Debug.Log($"Token: {e.Token}");
                }
            };
        }

        private static void ClearEvents()
        {
            OnMessageReceived = null;
            OnConnected = null;
            OnDisconnected = null;

            OnConnectionMessage = null;
            OnLoginResponseMessage = null;
            OnCreateEntityMessage = null;
            OnMoveEntityMessage = null;

            InitializeEvents();
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
                    MainTask.Enqueue(() => OnConnected?.Invoke("Connected"));
                    Debug.Log("Connected");

                    var memoryStream = new MemoryStream();

                    while (_clientWebSocket.State == WebSocketState.Open)
                    {
                        var buffer = new ArraySegment<byte>(new byte[1024]);
                        var result = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            break;
                        }
                        else
                        {
                            memoryStream.Write(buffer.Array, 0, result.Count);
                            if (result.EndOfMessage)
                            {
                                MainTask.Enqueue(() => OnMessageReceived?.Invoke(memoryStream.ToArray(), result));
                                ReceiveMessage(memoryStream.ToArray(), result);
                                memoryStream.SetLength(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _connectionState = ConnectionState.Disconnected;
                Debug.LogWarning(ex.Message);
            }

            Disconnect();
        }

        private void ReceiveMessage(byte[] buffer, WebSocketReceiveResult result)
        {
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var text = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Debug.Log(text);
            }
            else
            {
                Debug.Log("Received binary data. Length: " + result.Count);
                var typeBuffer = new byte[2];
                Array.Copy(buffer, 0, typeBuffer, 0, 2);
                Array.Reverse(typeBuffer);
                var type = BitConverter.ToUInt16(typeBuffer, 0);
                buffer = buffer.Skip(2).ToArray();

                MainTask.Enqueue(() =>
                {
                    switch (type)
                    {
                        case 0:
                            var connectionMessage = Protobuf.Client.Connection.Parser.ParseFrom(buffer);
                            MainTask.Enqueue(() => OnConnectionMessage?.Invoke(this, new ConnectionEventArgs(
                                connectionMessage.SessionId
                            )));
                            break;
                        case 1:
                            var loginResponseMessage = Protobuf.Client.LoginResponse.Parser.ParseFrom(buffer);
                            MainTask.Enqueue(() => OnLoginResponseMessage?.Invoke(this, new LoginResponseEventArgs(
                                loginResponseMessage.Success,
                                loginResponseMessage.UserUUID,
                                loginResponseMessage.Username,
                                loginResponseMessage.Token
                            )));
                            break;
                        case 2:
                            var createEntityMessage = Protobuf.Client.CreateEntity.Parser.ParseFrom(buffer);
                            MainTask.Enqueue(() => OnCreateEntityMessage?.Invoke(this, new CreateEntityEventArgs(
                                new EntityData
                                (
                                    createEntityMessage.Entity.UUID,
                                    createEntityMessage.Entity.OwnerUUID,
                                    createEntityMessage.Entity.Name,
                                    new Vector2(createEntityMessage.Entity.Position.X, createEntityMessage.Entity.Position.Y),
                                    new Quaternion(createEntityMessage.Entity.Rotation.X, createEntityMessage.Entity.Rotation.Y, createEntityMessage.Entity.Rotation.Z, createEntityMessage.Entity.Rotation.W)
                                )
                            )));
                            break;
                        case 3:
                            var moveEntityMessage = Protobuf.Client.MoveEntity.Parser.ParseFrom(buffer);
                            MainTask.Enqueue(() => OnMoveEntityMessage?.Invoke(this, new MoveEntityEventArgs(
                                moveEntityMessage.EntityId,
                                new Vector2(moveEntityMessage.Position.X, moveEntityMessage.Position.Y),
                                new Quaternion(moveEntityMessage.Rotation.X, moveEntityMessage.Rotation.Y, moveEntityMessage.Rotation.Z, moveEntityMessage.Rotation.W)
                            )));
                            break;
                    }
                });
            }
        }
        #endregion

        #region Functional methods
        public static void Login(string username, string password)
        {
            if (_connectionState == ConnectionState.Connected)
            {
                var loginRequest = new Protobuf.Server.LoginRequest();
                loginRequest.Username = username;
                loginRequest.Password = password;
                
                SendPacket(0, loginRequest);
            }
            else if (_connectionState == ConnectionState.Disconnected)
            {
                Debug.LogWarning("You are not connected to the server.");
            }
            else if (_connectionState == ConnectionState.LoggedIn)
            {
                Debug.LogWarning("You are already logged in.");
            }
        }

        public static void Login(string token)
        {
            if (_connectionState == ConnectionState.Connected)
            {
                var tokenLoginRequest = new Protobuf.Server.TokenLoginRequest();
                tokenLoginRequest.Token = token;

                SendPacket(1, tokenLoginRequest);
            }
            else if (_connectionState == ConnectionState.Disconnected)
            {
                Debug.LogWarning("You are not connected to the server.");
            }
            else if (_connectionState == ConnectionState.LoggedIn)
            {
                Debug.LogWarning("You are already logged in.");
            }
        }
        #endregion
    }
}
