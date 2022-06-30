import { Quaternion } from "./Quaternion";
import { Vector2 } from "./Vector2";

export class Entity {
    UUID: string;
    OwnerUUID: string;
    Name: string;
    Position: Vector2;
    Rotation: Quaternion;
    Data: string;
    constructor(uuid: string, ownerId: string, name: string, position: Vector2, rotation: Quaternion, data: string) {
        this.UUID = uuid;
        this.OwnerUUID = ownerId;
        this.Name = name;
        this.Position = position;
        this.Rotation = rotation;
        this.Data = data;
    }
}