using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoSingleton<NetworkManager>
{

    public List<Entity> entityList = new List<Entity>();

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