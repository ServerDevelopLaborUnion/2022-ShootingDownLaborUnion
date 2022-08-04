import { v4 } from "uuid";
import { storage } from "../storage";
import { Client } from "../types/Client";
import { Entity } from "../types/Entity";
import { Quaternion } from "../types/Quaternion";
import { Vector2 } from "../types/Vector2";
import { getLogger } from "../util/logger";
import proto from "../util/proto";

const Logger = getLogger('EntityCreateRequest');

const id = 5;
const type = 'EntityCreateRequest';

module.exports = {
    id: id,
    type: type,
    receive: async (client: Client, buffer: Buffer) => {
        if (!proto.server.verify(type, buffer)) {
            Logger.warn(`Invalid packet from ${client.sessionId}`);
            return;
        }

        const EntityCreateRequest: any = proto.server.decode(type, buffer);
        console.log(EntityCreateRequest);
        const entity = new Entity(v4(), client.sessionId, EntityCreateRequest.Name, new Vector2(0, 0), new Quaternion(0, 0, 0, 0), EntityCreateRequest.Entity.Data);
        if (entity !== undefined) {
            if (client.sessionId == entity.OwnerUUID) {
                console.log(entity);
                entity.Position = EntityCreateRequest.Entity.Position;
                entity.Rotation = EntityCreateRequest.Entity.Rotation;
                storage.server.rooms.get('test')?.addEntity(entity);
            }
        }
    }
}

