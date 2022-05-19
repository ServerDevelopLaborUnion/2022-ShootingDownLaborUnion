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
        WebSocket.Client.OnCreateEntityMessage += Client_OnCreateEntityMessage;
        WebSocket.Client.OnRemoveEntityMessage += Client_OnRemoveEntityMessage;
        WebSocket.Client.OnMoveEntityMessage += Client_OnMoveEntityMessage;
    }

    private void Client_OnMoveEntityMessage(object sender, WebSocket.MoveEntityEventArgs e)
    {
        Entity temp = NetworkManager.Instance.entityList.Find((x) => x.Data.UUID == e.EntityId);
        temp.transform.SetPositionAndRotation(e.Position, e.Rotation);
    }

    private void Client_OnRemoveEntityMessage(object sender, WebSocket.RemoveEntityEventArgs e)
    {
        Entity.DeleteEntity(e.EntityId);
    }

    private void Client_OnCreateEntityMessage(object sender, WebSocket.CreateEntityEventArgs e)
    {
        NetworkManager.Instance.entityList.Add(Entity.CreateEntity(e.Data));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
