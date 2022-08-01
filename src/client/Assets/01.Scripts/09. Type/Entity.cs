using System;
using Cinemachine;
using UnityEngine;

[System.Serializable]
public class Entity : MonoBehaviour
{
    public GameObject EntityObject { get; set; }
    [field : SerializeField]
    public EntityData Data { get; private set; }


    public static Entity EntityCreate(EntityData data)
    {
        GameObject prefab = Resources.Load("Prefabs/" + data.Type.ToString()) as GameObject;
        var newObject = Instantiate(prefab, data.Position, data.Rotation);
        newObject.name = data.Name;
        Entity temp = newObject.GetComponent<Entity>();
        temp.Data = data;
        temp.Data.parantEntity = temp;
        temp.EntityObject = newObject;
        newObject.transform.SetPositionAndRotation(temp.Data.Position, temp.Data.Rotation);
        if(data.Type == EntityType.Player)
        if (WebSocket.Client.CheckIsOwnedEntity(temp))
        {
            PlayerBase HostPlayer = data.parantEntity.GetComponent<PlayerBase>();
            data.parantEntity.gameObject.AddComponent<StatManager>().UpdateText(HostPlayer, HostPlayer.Weapon);
            GameObject.Find("VirtualCam").GetComponent<CinemachineVirtualCamera>().Follow = data.parantEntity.transform;
            newObject.AddComponent<CharacterInput>()?.InitEvent();
        }
        
        if (data.entityStat != null)
            temp.EntityObject.GetComponent<CharacterBase>()?.InitStat(data.entityStat);
        return temp;
    }

    public static void InvokeAction(string uuid, string eventName)
    {
        foreach (var entity in NetworkManager.Instance.entityList)
        {
            if (string.Compare(entity.Data.UUID, uuid) == 0)
            {
                entity.EntityObject.GetComponent<CharacterEvent>()?.InvokeAction(eventName);
                break;
            }
        }
    }
    public static void InvokeFunc(string uuid, string eventName, string message)
    {
        foreach (var entity in NetworkManager.Instance.entityList)
        {
            if (string.Compare(entity.Data.UUID, uuid) == 0)
            {
                entity.EntityObject.GetComponent<CharacterEvent>()?.InvokeAction(eventName);
                break;
            }
        }
    }

    public static void DeleteEntity(string uuid)
    {
        foreach (var entity in NetworkManager.Instance.entityList)
        {
            if (string.Compare(entity.Data.UUID, uuid) == 0)
            {
                NetworkManager.Instance.entityList.Remove(entity);
                Debug.Log($"{entity.Data.UUID} deleted");
                Destroy(entity.gameObject);
                break;
            }
        }
    }


    public void SetEntityPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
    }

}
