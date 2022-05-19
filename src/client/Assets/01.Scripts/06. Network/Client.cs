using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

    public class EntityCreateEventArgs : EventArgs
    {
        public EntityData Data { get; set; }

        public EntityCreateEventArgs(EntityData data)
        {
            Data = data;
        }
    }

    public class EntityMoveEventArgs : EventArgs
    {
        public string EntityUUID { get; set; }
        public Vector2 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public EntityMoveEventArgs(string entityId, Vector2 position, Quaternion rotation)
        {
            EntityUUID = entityId;
            Position = position;
            Rotation = rotation;
        }
    }

    public class EntityRemoveEventArgs : EventArgs
    {
        public string EntityUUID { get; set; }

        public EntityRemoveEventArgs(string entityId)
        {
            EntityUUID = entityId;
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
        
        public static event EventHandler<EntityCreateEventArgs> OnEntityCreateMessage;
        
        public static event EventHandler<EntityMoveEventArgs> OnEntityMoveMessage;
        public static event EventHandler<EntityRemoveEventArgs> OnReentityMoveMessage;
        #endregion
        
        public static Account Account { get; private set; }

        public static ConnectionState ConnectionState => _connectionState;
        public static string SessionID { get; private set; }
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

        public static void SendPacket(uint type, Google.Protobuf.IMessage message)
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

            _clientWebSocket.SendAsync(new ArraySegment<byte>(Packet.Make((ushort)type, data)), WebSocketMessageType.Binary, true, CancellationToken.None).Wait();
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

        public static bool CheckIsOwnedEntity(Entity entity)
        {
            return string.Compare(SessionID, entity.Data.OwnerUUID) == 0;
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
            OnConnectionMessage += (sender, e) =>
            {
                SessionID = e.SessionId;
            };
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
            OnEntityCreateMessage = null;
            OnEntityMoveMessage = null;

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
                Debug.LogWarning(ex.Message + "\n" + ex.StackTrace);
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
                            var entityCreateMessage = Protobuf.Client.EntityCreate.Parser.ParseFrom(buffer);
                            int entityType = JObject.Parse(entityCreateMessage.Entity.Data)["type"].Value<int>();
                            MainTask.Enqueue(() => OnEntityCreateMessage?.Invoke(this, new EntityCreateEventArgs(
                                new EntityData
                                (
                                    entityCreateMessage.Entity.UUID,
                                    entityCreateMessage.Entity.OwnerUUID,
                                    entityCreateMessage.Entity.Name,
                                    new Vector2(entityCreateMessage.Entity.Position.X, entityCreateMessage.Entity.Position.Y),
                                    new Quaternion(entityCreateMessage.Entity.Rotation.X, entityCreateMessage.Entity.Rotation.Y, entityCreateMessage.Entity.Rotation.Z, entityCreateMessage.Entity.Rotation.W),
                                    (EntityType)entityType
                                )
                            )));
                            break;
                        case 3:
                            var entityMoveMessage = Protobuf.Client.EntityMove.Parser.ParseFrom(buffer);
                            //Debug.Log($"Entity {entityMoveMessage.EntityUUID} moved to {entityMoveMessage.Position.X}, {entityMoveMessage.Position.Y}");
                            MainTask.Enqueue(() => OnEntityMoveMessage?.Invoke(this, new EntityMoveEventArgs(
                                entityMoveMessage.EntityUUID,
                                new Vector2(entityMoveMessage.Position.X, entityMoveMessage.Position.Y),
                                new Quaternion(entityMoveMessage.Rotation.X, entityMoveMessage.Rotation.Y, entityMoveMessage.Rotation.Z, entityMoveMessage.Rotation.W)
                            )));
                            break;
                        case 4:
                            var entityRemoveMessage = Protobuf.Client.EntityRemove.Parser.ParseFrom(buffer);
                            MainTask.Enqueue(() => OnReentityMoveMessage?.Invoke(this, new EntityRemoveEventArgs(
                                entityRemoveMessage.EntityUUID
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

        public static void ApplyEntityMove(Entity entity)
        {
            if (_connectionState == ConnectionState.Connected)
            {
                var moveEntityRequest = new Protobuf.Server.EntityMoveRequest();
                moveEntityRequest.EntityUUID = entity.Data.UUID;
                moveEntityRequest.Position = entity.Data.Position.ToProtobuf();
                moveEntityRequest.Rotation = entity.Data.Rotation.ToProtobuf();


                SendPacket(2, moveEntityRequest);
            }
            else if (_connectionState == ConnectionState.Disconnected)
            {
                Debug.LogWarning("You are not connected to the server.");
            }
        }
        #endregion
    }
}
