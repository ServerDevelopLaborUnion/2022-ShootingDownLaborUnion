using UnityEngine;

[System.Serializable]
public class Entity : MonoBehaviour
{
    public string UUID { get; set; }
    public string OwnerUUID { get; set; }
    public string Name { get; set; }
    public Vector2 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public string Data { get; set; }

    public Entity()
    {
        
    }

    public Entity(string uuid, string ownerUUID, string name, Vector2 position, Quaternion rotation, string data)
    {
        UUID = uuid;
        OwnerUUID = ownerUUID;
        Name = name;
        Position = position;
        Rotation = rotation;
        Data = data;
    }

    private Entity CreateEntity(EntityData data)
    {

        GameObject newGameObjct = Instantiate(data.prefab, );




        return temp;
    }
}