using UnityEngine;

public class Entity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Vector2 Position { get; set; }
    public Quaternion Rotation { get; set; }

    public Entity(int id, string name, Vector2 position, Quaternion rotation)
    {
        Id = id;
        Name = name;
        Position = position;
        Rotation = rotation;
    }
}