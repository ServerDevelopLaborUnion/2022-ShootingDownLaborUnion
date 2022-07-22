import { storage } from "../storage";
import { Client } from "../types/Client";
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
        const entity = storage.server.rooms.get("testRoom")?.entities.get(EntityCreateRequest.EntityUUID);
        if (entity !== undefined) {
            if (client.sessionId == entity.OwnerUUID) {
                entity.Position = EntityCreateRequest.Position;
                client.room?.broadcast(proto.client.encode(proto.client.EntityMove, {
                    EntityUUID: EntityCreateRequest.EntityUUID,
                    Position: EntityCreateRequest.Position,
                    Rotation: EntityCreateRequest.Rotation,
                }), client);
            }
        }
    }
}

