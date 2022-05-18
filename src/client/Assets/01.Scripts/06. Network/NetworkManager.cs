using System;
using UnityEngine;

public class NetworkManager : MonoSingleton<NetworkManager>
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

    [RuntimeInitializeOnLoadMethod]
    static void OnSecondRuntimeMethodLoad()
    {
        Initialize(true);
    }

    public static string LocalPlayerID { get; private set; }

    public static void Instantiate(NetworkObject networkObject, Vector3 position, Quaternion rotation, string ownerID = null)
    {
        if (networkObject == null)
            return;

        if (string.IsNullOrEmpty(ownerID))
            ownerID = NetworkManager.LocalPlayerID;

        networkObject.transform.position = position;
        networkObject.transform.rotation = rotation;
    }
}