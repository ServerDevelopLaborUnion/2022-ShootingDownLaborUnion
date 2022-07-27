using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using WebSocket;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    ChatManager chatManager;
    // Start is called before the first frame update
    void Start()
    {
        chatManager = GameObject.Find("ChatObject").GetComponent<ChatManager>();

        WebSocket.Client.OnEntityCreateMessage += Client_OnEntityCreateMessage;
        WebSocket.Client.OnEntityReMoveMessage += Client_OnEntityReMoveMessage;
        WebSocket.Client.OnEntityMoveMessage += Client_OnEntityMoveMessage;
        WebSocket.Client.OnEntityEventMessage += Client_OnEntityEventMessage;
        WebSocket.Client.OnChatMessage += Client_OnChatMessage;
    }

    private void Client_OnChatMessage(object sender, ChatMessageEventArgs e)
    {
        chatManager.GetChatMessage(e.Message);
    }

    private void Client_OnEntityEventMessage(object sender, EntityEventArgs e)
    {
        Entity.InvokeAction(e.EntityUUID, e.EventName);
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
                PlayerBase HostPlayer = e.parantEntity.GetComponent<PlayerBase>();
                HostPlayer.RoomHost = true;
                e.parantEntity.gameObject.AddComponent<StatManager>().UpdateText(HostPlayer, HostPlayer.Weapon);
                GameObject.Find("VirtualCam").GetComponent<CinemachineVirtualCamera>().Follow = e.parantEntity.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
