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
        if (WebSocket.Client.CheckIsOwnedEntity(temp))
        {
            newObject.AddComponent<CharacterInput>().InitEvent();
        }
        
        if (data.entityStat != null)
            temp.EntityObject.GetComponent<CharacterBase>().InitStat(data.entityStat);
        return temp;
    }
}