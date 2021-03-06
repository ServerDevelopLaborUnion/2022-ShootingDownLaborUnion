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
        public Vector2 TargetPosition { get; set; }
        public Quaternion Rotation { get; set; }

        public EntityMoveEventArgs(string entityId, Vector2 position, Vector2 targetPos, Quaternion rotation)
        {
            EntityUUID = entityId;
            Position = position;
            TargetPosition = targetPos;
            Rotation = rotation;
        }
    }

    public class EntityEventArgs : EventArgs
    {
        public string EntityUUID { get; set; }
        public string EventName { get; set; }

        public EntityEventArgs(string entityId, string eventName)
        {
            EntityUUID = entityId;
            EventName = eventName;
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

    public class RoomCreateEventArgs : EventArgs
    {
        public bool Success { get; set; }
        public string RoomUUID { get; set; }
        public string RoomName { get; set; }

        public RoomCreateEventArgs(bool success, string roomUUID, string roomName)
        {
            Success = success;
            RoomUUID = roomUUID;
            RoomName = roomName;
        }
    }

    public class RoomJoinEventArgs : EventArgs
    {
        public bool Success { get; set; }
        public Room Room { get; set; }

        public RoomJoinEventArgs(bool success, Room room)
        {
            Success = success;
            Room = room;
        }
    }

    public class RoomLeaveEventArgs : EventArgs
    {
        public bool Success { get; set; }

        public RoomLeaveEventArgs(bool success)
        {
            Success = success;
        }
    }

    public class RoomListEventArgs : EventArgs
    {
        public List<RoomInfo> Rooms { get; set; }

        public RoomListEventArgs(List<RoomInfo> roomInfos)
        {
            Rooms = roomInfos;
        }
    }

    public class ChatMessageEventArgs : EventArgs
    {
        public string UUID { get; set; }
        public string Message { get; set; }

        public ChatMessageEventArgs(string uuid, string message)
        {
            UUID = uuid;
            Message = message;
        }
    }

    public class StartGameEventArgs : EventArgs
    {
        
    }

    public class SetRoleEventArgs : EventArgs
    {
        public string UUD { get; set; }
        public RoleType Role { get; set; }
        public bool IsReady { get; set; }

        public SetRoleEventArgs(string uud, RoleType role, bool isReady)
        {
            UUD = uud;
            Role = role;
            IsReady = isReady;
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
        public static event EventHandler<EntityRemoveEventArgs> OnEntityReMoveMessage;
        public static event EventHandler<EntityEventArgs> OnEntityEventMessage;
        public static event EventHandler<RoomCreateEventArgs> OnRoomCreateMessage;
        public static event EventHandler<RoomJoinEventArgs> OnRoomJoinMessage;
        public static event EventHandler<RoomLeaveEventArgs> OnRoomLeaveMessage;
        public static event EventHandler<RoomListEventArgs> OnRoomListMessage;
        public static event EventHandler<ChatMessageEventArgs> OnChatMessage;
        public static event EventHandler<StartGameEventArgs> OnStartGameMessage;
        public static event EventHandler<SetRoleEventArgs> OnSetRoleMessage;
        public static Dictionary<string, Action<string>> OnRoomEvent = new Dictionary<string, Action<string>>();
        public static Dictionary<string, Action<string>> OnUserEvent = new Dictionary<string, Action<string>>();
        #endregion

        public static Account Account { get; private set; }

        public static ConnectionState ConnectionState => _connectionState;
        public static string SessionID { get; private set; }
        private static ConnectionState _connectionState
        {
            get => _connectionStateValue;
            set
            {
                _connectionStateValue = value;
                MainTask.Enqueue(() => OnConnectionStateChanged?.Invoke(value));
            }
        }

        public static List<RoomInfo> RoomList { get; internal set; }

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
            SessionID = "";
            _connectionState = ConnectionState.Disconnected;
            MainTask.Enqueue(() => OnDisconnected?.Invoke("Disconnected"));
            ToolbarControl.UpdateUI();
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
            if (_clientWebSocket == null || _clientWebSocket.State == WebSocketState.Closed)
                Initialize();
        }

        private void OnDestroy()
        {
            Client.Disconnect();
            Debug.LogWarning($"Client Destroyed");
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
                ToolbarControl.UpdateUI();

                Storage.CurrentUser = new User(e.SessionId, "TestUser");
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
                _clientWebSocket = new ClientWebSocket();

                var uri = new Uri("ws://141.164.53.243:5000/");
                // var uri = new Uri("ws://localhost:3000/");
                await _clientWebSocket.ConnectAsync(uri, CancellationToken.None);

                if (_clientWebSocket.State == WebSocketState.Open)
                {
                    _connectionState = ConnectionState.Connected;
                    MainTask.Enqueue(() => OnConnected?.Invoke("Connected"));
                    ToolbarControl.UpdateUI();

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
                            ToolbarControl.UpdateUI();
                            break;
                        case 2:
                            var entityCreateMessage = Protobuf.Client.EntityCreate.Parser.ParseFrom(buffer);
                            Debug.Log(entityCreateMessage.Entity);
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
                                new Vector2(entityMoveMessage.TargetPosition.X, entityMoveMessage.TargetPosition.Y),
                                new Quaternion(entityMoveMessage.Rotation.X, entityMoveMessage.Rotation.Y, entityMoveMessage.Rotation.Z, entityMoveMessage.Rotation.W)
                            )));
                            break;
                        case 4:
                            var entityRemoveMessage = Protobuf.Client.EntityRemove.Parser.ParseFrom(buffer);
                            MainTask.Enqueue(() => OnEntityReMoveMessage?.Invoke(this, new EntityRemoveEventArgs(
                                entityRemoveMessage.EntityUUID
                            )));
                            break;
                        case 6:
                            var entityEventMessage = Protobuf.Client.EntityEvent.Parser.ParseFrom(buffer);
                            Debug.Log($"Entity {entityEventMessage.EntityUUID} triggered event {entityEventMessage.EventName}");
                            MainTask.Enqueue(() => OnEntityEventMessage?.Invoke(this, new EntityEventArgs(
                                entityEventMessage.EntityUUID,
                                entityEventMessage.EventName
                            )));
                            break;
                        case 7:
                            var roomCreateResponseMessage = Protobuf.Client.RoomCreateResponse.Parser.ParseFrom(buffer);
                            MainTask.Enqueue(() => OnRoomCreateMessage?.Invoke(this, new RoomCreateEventArgs(
                                roomCreateResponseMessage.Success,
                                roomCreateResponseMessage.RoomUUID,
                                roomCreateResponseMessage.RoomName
                            )));
                            break;
                        case 8:
                            var roomJoinResponseMessage = Protobuf.Client.RoomJoinResponse.Parser.ParseFrom(buffer);
                            Room room = new Room(new RoomInfo(roomJoinResponseMessage.Room.Info.UUID, roomJoinResponseMessage.Room.Info.Name, roomJoinResponseMessage.Room.Info.IsPrivate, roomJoinResponseMessage.Room.Info.PlayerCount), new List<User>());
                            room.Users.AddRange(roomJoinResponseMessage.Room.Users.Select(user => new User(user.UUID, user.Name, (WeaponType)user.Weapon, (RoleType)user.Role, user.IsReady, user.IsMaster)));
                            MainTask.Enqueue(() => OnRoomJoinMessage?.Invoke(this, new RoomJoinEventArgs(
                                roomJoinResponseMessage.Success,
                                room
                            )));
                            break;
                        case 9:
                            var roomLeaveResponseMessage = Protobuf.Client.RoomLeaveResponse.Parser.ParseFrom(buffer);
                            MainTask.Enqueue(() => OnRoomLeaveMessage?.Invoke(this, new RoomLeaveEventArgs(
                                roomLeaveResponseMessage.Success
                            )));
                            break;
                        case 10:
                            var roomListResponseMessage = Protobuf.Client.RoomListResponse.Parser.ParseFrom(buffer);
                            var rooms = new List<RoomInfo>();
                            foreach (var roomInfo in roomListResponseMessage.Info)
                            {
                                rooms.Add(new RoomInfo(roomInfo.UUID, roomInfo.Name, roomInfo.IsPrivate, roomInfo.PlayerCount));
                            }
                            MainTask.Enqueue(() => OnRoomListMessage?.Invoke(this, new RoomListEventArgs(
                                rooms
                            )));
                            break;
                        case 11:
                            var ChatMessage = Protobuf.Client.ChatMessage.Parser.ParseFrom(buffer);
                            MainTask.Enqueue(() => OnChatMessage?.Invoke(this, new ChatMessageEventArgs(
                                ChatMessage.UUID,
                                ChatMessage.Message
                            )));
                            break;
                        case 12:
                            var StartGameMessage = Protobuf.Client.StartGame.Parser.ParseFrom(buffer);
                            MainTask.Enqueue(() => OnStartGameMessage?.Invoke(this, new StartGameEventArgs()));
                            break;
                        case 13:
                            var SetRoleMessage = Protobuf.Client.SetRole.Parser.ParseFrom(buffer);
                            MainTask.Enqueue(() => OnSetRoleMessage?.Invoke(this, new SetRoleEventArgs(
                                SetRoleMessage.UUID,
                                (RoleType)SetRoleMessage.Role,
                                SetRoleMessage.IsReady
                            )));
                            break;
                        case 14:
                            var RoomEventMessage = Protobuf.Client.RoomEvent.Parser.ParseFrom(buffer);
                            if (Storage.CurrentRoom == null) break;
                            if (Storage.CurrentRoom.Info.UUID == RoomEventMessage.RoomUUID)
                            {
                                MainTask.Enqueue(() => OnRoomEvent[RoomEventMessage.EventName].Invoke(RoomEventMessage.EventData));
                            }
                            break;
                        case 15:
                            var UserEventMessage = Protobuf.Client.UserEvent.Parser.ParseFrom(buffer);
                            if (Storage.CurrentRoom == null) break;
                            if (Storage.CurrentRoom.Users.Any(user => user.UUID == UserEventMessage.UserUUID))
                            {
                                MainTask.Enqueue(() => OnUserEvent[UserEventMessage.EventName].Invoke(UserEventMessage.EventData));
                            }
                            break;
                        default:
                            Debug.LogWarning($"Unknown message type {type}");
                            break;
                    }
                });
            }
        }
        #endregion

        #region Functional methods
        public static void SubscribeRoomEvent(string eventName, Action<string> action)
        {
            if (OnRoomEvent.ContainsKey(eventName))
            {
                OnRoomEvent[eventName] += action;
            }
            else
            {
                OnRoomEvent.Add(eventName, action);
            }
        }

        public static void SubscribeUserEvent(string eventName, Action<string> action)
        {
            if (OnUserEvent.ContainsKey(eventName))
            {
                OnUserEvent[eventName] += action;
            }
            else
            {
                OnUserEvent.Add(eventName, action);
            }
        }
        public static void Login(string username, string password)
        {
            if (_connectionState != ConnectionState.LoggedIn)
            {
                var loginRequest = new Protobuf.Server.LoginRequest();
                loginRequest.Username = username;
                loginRequest.Password = password;

                SendPacket(0, loginRequest);
            }
            else
            {
                Debug.LogWarning("You are already logged in.");
            }
        }

        public static void Login(string token)
        {
            if (_connectionState != ConnectionState.LoggedIn)
            {
                var tokenLoginRequest = new Protobuf.Server.TokenLoginRequest();
                tokenLoginRequest.Token = token;

                SendPacket(1, tokenLoginRequest);
            }
            else
            {
                Debug.LogWarning("You are already logged in.");
            }
        }

        public static void ApplyEntityMove(Entity entity)
        {
            if (Storage.CurrentUser.UUID == entity.Data.OwnerUUID)
            {
                var moveEntityRequest = new Protobuf.Server.EntityMoveRequest();
                moveEntityRequest.EntityUUID = entity.Data.UUID;
                moveEntityRequest.Position = entity.Data.Position.ToProtobuf();
                moveEntityRequest.TargetPosition = entity.Data.TargetPosition.ToProtobuf();
                moveEntityRequest.Rotation = entity.Data.Rotation.ToProtobuf();


                SendPacket(2, moveEntityRequest);
            }
        }

        public static void ApplyEntityAction(Entity entity, string eventName)
        {
            if (_connectionState == ConnectionState.Connected)
            {
                var entityEventRequest = new Protobuf.Server.EntityEventRequest();
                entityEventRequest.EntityUUID = entity.Data.UUID;
                entityEventRequest.EventName = eventName;

                SendPacket(4, entityEventRequest);
            }
            else if (_connectionState == ConnectionState.Disconnected)
            {
                Debug.LogWarning("You are not connected to the server.");
            }
        }

        public static void ApplyEntityFunc(Entity entity, string eventName, string message)
        {
            if (_connectionState == ConnectionState.Connected)
            {
                var entityEventRequest = new Protobuf.Server.EntityEventRequest();
                entityEventRequest.EntityUUID = entity.Data.UUID;
                entityEventRequest.EventName = eventName;
                entityEventRequest.EventData = message;

                SendPacket(4, entityEventRequest);
            }
            else if (_connectionState == ConnectionState.Disconnected)
            {
                Debug.LogWarning("You are not connected to the server.");
            }
        }
        public static void CreateEntityEvent(Entity entity)
        {
            var createEntityRequest = new Protobuf.Server.EntityCreateRequest();

            createEntityRequest.Entity = new Protobuf.Entity();
            createEntityRequest.Entity.UUID = entity.Data.UUID;
            createEntityRequest.Entity.OwnerUUID = entity.Data.OwnerUUID;
            createEntityRequest.Entity.Name = entity.Data.Name;
            createEntityRequest.Entity.Position = entity.Data.Position.ToProtobuf();
            createEntityRequest.Entity.Rotation = entity.Data.Rotation.ToProtobuf();
            createEntityRequest.Entity.Data = $"{{\"type\": {(int)entity.Data.Type}}}";

            SendPacket(5, createEntityRequest);
        }

        public static void CreateRoom(string name, string password)
        {
            var createRoomRequest = new Protobuf.Server.RoomCreateRequest();
            createRoomRequest.Name = name;
            createRoomRequest.Password = password;

            SendPacket(6, createRoomRequest);
        }

        public static void JoinRoom(string uuid, string password)
        {
            var joinRoomRequest = new Protobuf.Server.RoomJoinRequest();
            joinRoomRequest.RoomUUID = uuid;
            joinRoomRequest.Password = password;

            SendPacket(7, joinRoomRequest);
        }

        public static void LeaveRoom()
        {
            var leaveRoomRequest = new Protobuf.Server.RoomLeaveRequest();

            SendPacket(8, leaveRoomRequest);
        }

        public static void GetRoomList()
        {
            var roomListRequest = new Protobuf.Server.RoomListRequest();

            SendPacket(9, roomListRequest);
        }

        public static void EntityDespawn(Entity entity)
        {
            var entityDespawnRequest = new Protobuf.Server.EneityDespawnRequest();
            entityDespawnRequest.EntityUUID = entity.Data.UUID;

            SendPacket(10, entityDespawnRequest);
        }

        public static void SendChatMessage(string message)
        {
            var entityChatRequest = new Protobuf.Server.ChatRequest();
            entityChatRequest.Message = message;
            SendPacket(11, entityChatRequest);
        }

        public static void StartGame()
        {
            var startGameRequest = new Protobuf.Server.StartGameRequest();

            SendPacket(12, startGameRequest);
            //TODO: 방 안에있는 모든 플레이어 씬 로딩
        }

        public static void SetRole(int role, bool isReady)
        {
            var setRoleRequest = new Protobuf.Server.SetRoleRequest();
            setRoleRequest.Role = role;
            setRoleRequest.IsReady = isReady;

            SendPacket(13, setRoleRequest);

            // TODO: OnUpdateRole실행
        }

        public static void RoomEvent(string name, string data)
        {
            var roomEventRequest = new Protobuf.Server.RoomEvent();
            roomEventRequest.RoomUUID = Storage.CurrentRoom.Info.UUID;
            roomEventRequest.EventName = name;
            roomEventRequest.EventData = data;

            SendPacket(14, roomEventRequest);
        }

        public static void UserEvent(string name, string data)
        {
            var userEventRequest = new Protobuf.Server.UserEvent();
            userEventRequest.UserUUID = Storage.CurrentUser.UUID;
            userEventRequest.EventName = name;
            userEventRequest.EventData = data;

            SendPacket(15, userEventRequest);
        }
        #endregion
    }
}
