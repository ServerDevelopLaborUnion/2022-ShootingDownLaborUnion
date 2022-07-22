using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using WebSocket;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WebSocket.Client.OnEntityCreateMessage += Client_OnEntityCreateMessage;
        WebSocket.Client.OnEntityReMoveMessage += Client_OnEntityReMoveMessage;
        WebSocket.Client.OnEntityMoveMessage += Client_OnEntityMoveMessage;
        WebSocket.Client.OnEntityEventMessage += Client_OnEntityEventMessage;
    }

    private void Client_OnEntityEventMessage(object sender, EntityEventArgs e)
    {
        Entity.InvokeEvent(e.EntityUUID, e.EventName);
        //TODO DoAttack -> CharacterEvent.InvokeEvent(DoAttack)
        //TODO DoFlipLeft -> CharacterEvent.InvokeEvent(DoFlipLeft)
        //TODO DoFlipRight -> CharacterEvent.InvokeEvent(DoFlipRight)
    }

    private void Client_OnEntityMoveMessage(object sender, WebSocket.EntityMoveEventArgs e)
    {
        Entity temp = NetworkManager.Instance.entityList.Find((x) => x.Data.UUID == e.EntityUUID);
        if (temp == null)
            return;
        temp.transform.position = e.Position;
        CharacterMove tempMove = temp.GetComponent<CharacterMove>();
        if(tempMove != null)
            tempMove.MoveAgent(e.TargetPosition);
    }

    private void Client_OnEntityReMoveMessage(object sender, WebSocket.EntityRemoveEventArgs e)
    {
        Entity.DeleteEntity(e.EntityUUID);
        Debug.Log($"{e.EntityUUID} Has Removed");
    }

    private void Client_OnEntityCreateMessage(object sender, WebSocket.EntityCreateEventArgs e)
    {
        Entity temp = Entity.EntityCreate(e.Data);
        if (e.Data.Type == EntityType.Player)
            NetworkManager.Instance.playerList.Add(temp);
        NetworkManager.Instance.entityList.Add(temp);
        Debug.Log($"{e.Data.UUID} Has Created");
        StartCoroutine(SetHostClient(e.Data));
    }

    private IEnumerator SetHostClient(EntityData e)
    {
        yield return new WaitForSeconds(1f);
        if(e.Type == EntityType.Player)
        {
            if (NetworkManager.Instance.playerList.Count == 1)
            {
                e.parantEntity.GetComponent<PlayerBase>().RoomHost = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
