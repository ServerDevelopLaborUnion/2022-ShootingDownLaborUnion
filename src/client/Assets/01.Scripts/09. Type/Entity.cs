using UnityEngine;

[System.Serializable]
public class Entity : MonoBehaviour
{
    public GameObject EntityObject { get; set; }
    [field : SerializeField]
    public EntityData Data { get; private set; }


    public static Entity CreateEntity(EntityData data)
    {
        GameObject prefab = Resources.Load("Prefab/" + data.Type.ToString()) as GameObject;
        var newObject = Instantiate(prefab, data.Position, data.Rotation);
        newObject.name = data.Name;
        Entity temp = newObject.GetComponent<Entity>();
        temp.Data = data;
        temp.Data.parantEntity = temp;
        newObject.transform.SetPositionAndRotation(temp.Data.Position, temp.Data.Rotation);
        if (WebSocket.Client.CheckIsOwnedEntity(temp))
        {
            newObject.AddComponent<CharacterInput>().InitEvent();
        }
        
        if (data.entityStat != null)
            temp.EntityObject.GetComponent<CharacterBase>().InitStat(data.entityStat);
        return temp;
    }

    public static void DeleteEntity(string uuid)
    {
        foreach (var entity in NetworkManager.Instance.entityList)
        {
            if (string.Compare(entity.Data.UUID, uuid) == 0)
            {
                NetworkManager.Instance.entityList.Remove(entity);
                Destroy(entity.gameObject);
                break;
            }
        }
    }


}