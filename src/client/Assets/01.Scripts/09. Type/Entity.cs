using UnityEngine;

[System.Serializable]
public class Entity : MonoBehaviour
{
    public GameObject EntityObject { get; set; }
    public string Data { get; set; }


    public static Entity CreateEntity(EntityData data)
    {
        Entity temp = new Entity();
        GameObject prefab = Resources.Load("Prefab/" + data.Type.ToString()) as GameObject;
        temp.EntityObject = Instantiate(prefab, data.Position, data.Rotation);
        if (data.entityStat != null)
            temp.EntityObject.GetComponent<CharacterBase>().InitStat(data.entityStat);
        return temp;
    }
}