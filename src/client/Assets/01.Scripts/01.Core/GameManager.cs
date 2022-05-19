using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WebSocket.Client.OnEntityCreateMessage += Client_OnEntityCreateMessage;
        WebSocket.Client.OnEntityReMoveMessage += Client_OnEntityReMoveMessage;
        WebSocket.Client.OnEntityMoveMessage += Client_OnEntityMoveMessage;
    }

    private void Client_OnEntityMoveMessage(object sender, WebSocket.EntityMoveEventArgs e)
    {
        Entity temp = NetworkManager.Instance.entityList.Find((x) => x.Data.UUID == e.EntityUUID);
        temp.transform.SetPositionAndRotation(e.Position, e.Rotation);
    }

    private void Client_OnEntityReMoveMessage(object sender, WebSocket.EntityRemoveEventArgs e)
    {
        Entity.DeleteEntity(e.EntityUUID);
        Debug.Log($"{e.EntityUUID} Has Removed");
    }

    private void Client_OnEntityCreateMessage(object sender, WebSocket.EntityCreateEventArgs e)
    {
        NetworkManager.Instance.entityList.Add(Entity.EntityCreate(e.Data));
        Debug.Log($"{e.Data.UUID} Has Created");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
