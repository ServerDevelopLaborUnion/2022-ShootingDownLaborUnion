using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    Player,
    Enemy,
    Bullet
}

[Serializable]
public class EntityData
{
    public string UUID = null;
    public string OwnerUUID = null;
    public string Name = null;
    public Vector2 Position
    {
        get
        { 
            if (parantEntity != null) return parantEntity.transform.position;
            return position;
        }

        set
        {
            if (parantEntity != null) parantEntity.transform.position = value;
            position = value;
        }
    }
    public Quaternion Rotation
    {
        get
        {
            if (parantEntity != null) return parantEntity.transform.rotation;
            return rotation;
        }

        set
        {
            if (parantEntity != null) parantEntity.transform.rotation = value;
            rotation = value;
        }
    }
    public EntityType Type;
    public CharacterStat entityStat = null;

    private Vector2 position;
    private Quaternion rotation;

    [NonSerialized]
    Entity parantEntity = null;

    public EntityData(string uuid, string ownerUUID, string name, Vector2 position, Quaternion rotation, EntityType type, Entity entity = null)
    {
        UUID = uuid;
        OwnerUUID = ownerUUID;
        Name = name;
        Position = position;
        Rotation = rotation;
        Type = type;
        parantEntity = entity;
    }
}
