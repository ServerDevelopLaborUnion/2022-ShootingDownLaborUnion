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
        WebSocket.Client.OnReentityMoveMessage += Client_OnReentityMoveMessage;
        WebSocket.Client.OnEntityMoveMessage += Client_OnEntityMoveMessage;
    }

    private void Client_OnEntityMoveMessage(object sender, WebSocket.EntityMoveEventArgs e)
    {
        Entity temp = NetworkManager.Instance.entityList.Find((x) => x.Data.UUID == e.EntityUUID);
        temp.transform.SetPositionAndRotation(e.Position, e.Rotation);
    }

    private void Client_OnReentityMoveMessage(object sender, WebSocket.EntityRemoveEventArgs e)
    {
        Entity.DeleteEntity(e.EntityUUID);
    }

    private void Client_OnEntityCreateMessage(object sender, WebSocket.EntityCreateEventArgs e)
    {
        NetworkManager.Instance.entityList.Add(Entity.EntityCreate(e.Data));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
