using UnityEngine;

public static class ExtensionForProtobuf
{
    public static Protobuf.Vector2 ToProtobuf(this Vector2 vector)
    {
        return new Protobuf.Vector2()
        {
            X = vector.x,
            Y = vector.y
        };
    }

    public static Protobuf.Quaternion ToProtobuf(this Quaternion quaternion)
    {
        return new Protobuf.Quaternion()
        {
            X = quaternion.x,
            Y = quaternion.y,
            Z = quaternion.z,
            W = quaternion.w
        };
    }
}