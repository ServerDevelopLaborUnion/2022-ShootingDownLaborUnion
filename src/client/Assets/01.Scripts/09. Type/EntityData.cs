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
    public Vector2 Position;
    public Quaternion Rotation;
    public EntityType Type;
    public CharacterStat entityStat = null;

    public EntityData(string uuid, string ownerUUID, string name, Vector2 position, Quaternion rotation, EntityType type)
    {
        UUID = uuid;
        OwnerUUID = ownerUUID;
        Name = name;
        Position = position;
        Rotation = rotation;
        Type = type;
    }
}
